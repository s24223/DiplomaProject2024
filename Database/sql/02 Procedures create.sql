CREATE OR ALTER PROCEDURE SelectByStreetNameAndDivisionName
	@DivisionName VARCHAR (MAX),
	@StreetName VARCHAR (MAX) 
AS 
BEGIN
	WITH HierarchyDownWithName AS 
	(
		SELECT  
			[AD].[Id] ,
			[AD].[Name],
			[AD].[ParentDivisionId] ,
			[AD].[AdministrativeTypeId] 
		FROM [dbo].[AdministrativeDivision] AS [AD]
		WHERE [AD].[Name] LIKE @DivisionName

		UNION ALL

		SELECT 
			[A].[Id] ,
			[A].[Name] ,
			[A].[ParentDivisionId] ,
			[A].[AdministrativeTypeId] 
		FROM [dbo].[AdministrativeDivision] AS [A] 
		JOIN HierarchyDownWithName AS [HD] ON [A].[ParentDivisionId]  = [HD].[Id]
	) 
	SELECT  
		DISTINCT [HDN].[Id] AS 'AdministrativeDivisionId',
		--[HDN].[Name] AS 'AdministrativeDivisionName',
		--[HDN].[ParentDivisionId] ,
		--[HDN].[AdministrativeTypeId] AS 'DivisionAdministrativeTypeId',	
		--[ATD].[Name] AS 'AdministrativeDivisionAdministrativeTypeName',
		[S].[Id] AS 'StreetId',
		[S].[Name] AS 'StreetName',
		[S].[AdministrativeTypeId] AS 'StreetAdministrativeTypeId',	
		[ATS].[Name] AS 'StreetAdministrativeTypeName'
	FROM HierarchyDownWithName AS [HDN]
	
	JOIN [dbo].[DivisionStreet] AS [DS] 
	ON [HDN].[Id] = [DS].[DivisionId]
	
	JOIN [dbo].[Street] AS [S] 
	ON [DS].[StreetId] = [S].[Id]
	
	JOIN [dbo].[AdministrativeType] AS [ATS] 
	ON [S].[AdministrativeTypeId] = [ATS].[Id]
	
	--JOIN [dbo].[AdministrativeType] AS [ATD] 
	--ON [HDN].[AdministrativeTypeId] = [ATD].[Id]
	
	WHERE [S].[Name] LIKE '%'+@StreetName+'%';
END;
--========================================================================================
GO
--========================================================================================
CREATE OR ALTER PROCEDURE SelectAdministrativeDivisionUp
	@DivisionId INT 
AS 
BEGIN
	WITH HierarchyUp AS 
	(
		SELECT  
			[AD].[Id] ,
			[AD].[Name] ,
			[AD].[ParentDivisionId] ,
			[AD].[AdministrativeTypeId],
			0 AS Level
		FROM [dbo].[AdministrativeDivision] AS [AD]
		WHERE [AD].[Id] = @DivisionId

		UNION ALL	

		SELECT  
			[ADF].[Id] ,
			[ADF].[Name] ,
			[ADF].[ParentDivisionId] ,
			[ADF].[AdministrativeTypeId],
			[HU].[Level] + 1 
		FROM [dbo].[AdministrativeDivision] AS [ADF] 
		JOIN HierarchyUp AS [HU] ON [ADF].Id  = [HU].[ParentDivisionId]
	) 
	SELECT  
			[HU].[Id] ,
			[HU].[Name] AS 'AdministrativeDivisionName',
			[HU].[ParentDivisionId] ,
			[HU].[AdministrativeTypeId],
			[AT].[Name] AS 'AdministrativeTypeName'
	FROM [HierarchyUp] AS [HU] 
	
	JOIN [dbo].[AdministrativeType] AS [AT] 
	ON [HU].[AdministrativeTypeId] = [AT].[Id]
	
	ORDER BY [HU].[Level] DESC;
END;
--========================================================================================
GO
--========================================================================================
CREATE OR ALTER PROCEDURE BranchesSorted
	@CompanyId uniqueidentifier,	
	@DivisionId INT,	
	@StreetId INT,
	@MaxItems INT,	
	@Page INT,
	@Ascending BIT --0 false, 1 true
AS
BEGIN
	DECLARE @SQL NVARCHAR(MAX);
	DECLARE @SortOrder NVARCHAR(5) = ' ASC ';
	IF @Ascending = 0 
	BEGIN
		SET @SortOrder = ' DESC ';
	END;
	pRINT @SortOrder
	SET @SQL = N'WITH HierarchyUpString AS 
			(				
				SELECT  
					[AD].[Id],
					[AD].[ParentDivisionId],
					0 AS Level,
					[B].[Id] AS "BranchId", 
					[S].[Id] AS "StreetId",
					[S].[Name] AS "StreetName"
				FROM [dbo].[AdministrativeDivision] AS [AD]
				JOIN [dbo].[Address] AS [A] ON [A].[DivisionId] = [AD].[Id]
				JOIN [dbo].[Branch] AS [B] ON [B].[AddressId] = [A].[Id]
				JOIN [dbo].[Company] AS [C] ON [B].[CompanyId] = [C].[UserId]	
				JOIN [dbo].[Street] AS [S] ON [A].[StreetId] = [S].[Id]
				WHERE [C].[UserId] = @companyId ' ;

	IF EXISTS(SELECT 1 FROM [dbo].[Street] WHERE [Street].[Id] = @StreetId ) 
	BEGIN
		SET @SQL = @SQL + N'AND [S].[Id] = @StreetId ';
	END;
	
	SET @SQL = @SQL+ N'

				UNION ALL  

				SELECT  
					[ADF].[Id],
					[ADF].[ParentDivisionId],
					[HU].[Level] + 1,
					[HU].[BranchId],
					[HU].[StreetId],
					[HU].[StreetName]
				FROM [dbo].[AdministrativeDivision] AS [ADF] 
				JOIN HierarchyUpString AS [HU] ON [ADF].Id = [HU].[ParentDivisionId]
			) 
			-- Zewnêtrzne zapytanie, które zwraca dane
			SELECT
				[HU].[BranchId],
				STRING_AGG(CAST([HU].[Id] AS VARCHAR), ''-'' ) WITHIN GROUP (ORDER BY [HU].[Level] DESC) AS CombinedIDs,
				COUNT(*) OVER() AS TotalRecords
			FROM [HierarchyUpString] AS [HU]
			WHERE 1 = 1 ';

	IF EXISTS(SELECT 1 FROM [dbo].[AdministrativeDivision] AS [AD] WHERE [AD].[Id] = @DivisionId ) 
	BEGIN
		SET @SQL = @SQL + N'AND [HU].[BranchId] IN (
			SELECT DISTINCT [BranchId]
		FROM [HierarchyUpString]
		WHERE [Id] = @DivisionId )';
	END;

	SET @SQL = @SQL + N'GROUP BY [HU].[BranchId], [HU].[StreetId], [HU].[StreetName]
			ORDER BY [CombinedIDs] '+@SortOrder+' , [HU].[StreetName], [HU].[StreetId]';
	SET @SQL = @SQL + N' OFFSET ((@Page - 1) * @maxItems) ROWS      
			FETCH NEXT @MaxItems ROWS ONLY ;';
	PRINT @SQL; 
	EXEC sp_executesql @SQL,
	N'@companyId UNIQUEIDENTIFIER, @DivisionId INT, @StreetId INT, @Page INT, @MaxItems INT', 
	@companyId, @DivisionId, @StreetId, @Page, @MaxItems;

END;

--========================================================================================
GO
--========================================================================================
CREATE OR ALTER PROCEDURE [dbo].[SelectDivisionIdsWithStreets]
	@DivisionId INT 
AS 
	BEGIN
		WITH HierarchyDownWithName AS 
		(
			SELECT  
				[AD].[Id] ,
				[AD].[Name],
				[AD].[ParentDivisionId] ,
				[AD].[AdministrativeTypeId] 
			FROM [dbo].[AdministrativeDivision] AS [AD]
			WHERE [AD].[Id] = @DivisionId

			UNION ALL

			SELECT 
				[A].[Id] ,
				[A].[Name] ,
				[A].[ParentDivisionId] ,
				[A].[AdministrativeTypeId] 
			FROM [dbo].[AdministrativeDivision] AS [A] 
			JOIN HierarchyDownWithName AS [HD] ON [A].[ParentDivisionId]  = [HD].[Id]
		) 
		SELECT  
			DISTINCT [HDN].[Id] AS 'AdministrativeDivisionId',
			[HDN].[Name]
		FROM HierarchyDownWithName AS [HDN]	
		JOIN [dbo].[DivisionStreet] AS [DS] 
		ON [HDN].[Id] = [DS].[DivisionId]
		ORDER BY [HDN].[Name], [HDN].[Id]
	END;

	
--========================================================================================
GO
--========================================================================================
CREATE OR ALTER PROCEDURE [dbo].[SelectDivisionIdsWithStreetsCount]
	@DivisionId INT 
AS 
	BEGIN
		WITH HierarchyDownWithName AS 
		(
			SELECT  
				[AD].[Id] ,
				[AD].[Name],
				[AD].[ParentDivisionId] ,
				[AD].[AdministrativeTypeId] 
			FROM [dbo].[AdministrativeDivision] AS [AD]
			WHERE [AD].[Id] = @DivisionId

			UNION ALL

			SELECT 
				[A].[Id] ,
				[A].[Name] ,
				[A].[ParentDivisionId] ,
				[A].[AdministrativeTypeId] 
			FROM [dbo].[AdministrativeDivision] AS [A] 
			JOIN HierarchyDownWithName AS [HD] ON [A].[ParentDivisionId]  = [HD].[Id]
		)
		SELECT  
			Count(DISTINCT [HDN].[Id])
		FROM HierarchyDownWithName AS [HDN]	
		JOIN [dbo].[DivisionStreet] AS [DS] 
		ON [HDN].[Id] = [DS].[DivisionId]
	END;
		
--========================================================================================
GO
--========================================================================================
CREATE OR ALTER PROCEDURE GET_PARAMITERS_BY_INTERSHIP
	@IntershipId uniqueidentifier,	
	@UserId uniqueidentifier
AS
BEGIN
	IF EXISTS 
	(
		SELECT * FROM Internship
		JOIN Recruitment ON Internship.Id = Recruitment.Id
		JOIN BranchOffer ON Recruitment.BranchOfferId = BranchOffer.Id
		JOIN Branch ON BranchOffer.BranchId = Branch.Id
		WHERE @UserId = Recruitment.PersonId OR @UserId =Branch.CompanyId
	)
		BEGIN
			DECLARE @TOTAL_COUNT INT;
			DECLARE @COMPANY_PUBLISH_ALLOWED_COUNT INT;			
			DECLARE @PERSON_PUBLISH_ALLOWED_COUNT INT;
						
			DECLARE @COMPANY_END INT;			
			DECLARE @PERSON_END INT;

			DECLARE @AVG_COMPANY_IN FLOAT;			
			DECLARE @AVG_PERSON_IN FLOAT;						

			SELECT @TOTAL_COUNT = COUNT(1) 
			FROM Comment
			JOIN Internship ON Comment.InternshipId = Internship.Id
			JOIN Recruitment ON Internship.Id = Recruitment.Id
			JOIN BranchOffer ON Recruitment.BranchOfferId = BranchOffer.Id
			JOIN Branch ON BranchOffer.BranchId = Branch.Id
			WHERE (@UserId = Recruitment.PersonId OR @UserId =Branch.CompanyId)

			SELECT @COMPANY_PUBLISH_ALLOWED_COUNT = COUNT(1) 
			FROM Comment
			JOIN Internship ON Comment.InternshipId = Internship.Id
			JOIN Recruitment ON Internship.Id = Recruitment.Id
			JOIN BranchOffer ON Recruitment.BranchOfferId = BranchOffer.Id
			JOIN Branch ON BranchOffer.BranchId = Branch.Id
			WHERE (@UserId = Recruitment.PersonId OR @UserId =Branch.CompanyId) AND
				Comment.CommentTypeId = 1004;
			
			SELECT @PERSON_PUBLISH_ALLOWED_COUNT = COUNT(1) 
			FROM Comment
			JOIN Internship ON Comment.InternshipId = Internship.Id
			JOIN Recruitment ON Internship.Id = Recruitment.Id
			JOIN BranchOffer ON Recruitment.BranchOfferId = BranchOffer.Id
			JOIN Branch ON BranchOffer.BranchId = Branch.Id
			WHERE (@UserId = Recruitment.PersonId OR @UserId =Branch.CompanyId) AND
				Comment.CommentTypeId = 1005;

			SELECT  @COMPANY_END = Comment.Evaluation 
			FROM Comment
			JOIN Internship ON Comment.InternshipId = Internship.Id
			JOIN Recruitment ON Internship.Id = Recruitment.Id
			JOIN BranchOffer ON Recruitment.BranchOfferId = BranchOffer.Id
			JOIN Branch ON BranchOffer.BranchId = Branch.Id
			WHERE (@UserId = Recruitment.PersonId OR @UserId =Branch.CompanyId) AND
				Comment.CommentTypeId = 1000
			ORDER BY Comment.Created DESC;

			SELECT  @PERSON_END = Comment.Evaluation 
			FROM Comment
			JOIN Internship ON Comment.InternshipId = Internship.Id
			JOIN Recruitment ON Internship.Id = Recruitment.Id
			JOIN BranchOffer ON Recruitment.BranchOfferId = BranchOffer.Id
			JOIN Branch ON BranchOffer.BranchId = Branch.Id
			WHERE (@UserId = Recruitment.PersonId OR @UserId =Branch.CompanyId)AND
				Comment.CommentTypeId = 1001
			ORDER BY Comment.Created DESC;

			SELECT  @AVG_COMPANY_IN = Comment.Evaluation 
			FROM Comment
			JOIN Internship ON Comment.InternshipId = Internship.Id
			JOIN Recruitment ON Internship.Id = Recruitment.Id
			JOIN BranchOffer ON Recruitment.BranchOfferId = BranchOffer.Id
			JOIN Branch ON BranchOffer.BranchId = Branch.Id
			WHERE (@UserId = Recruitment.PersonId OR @UserId =Branch.CompanyId) AND
				(
				Comment.CommentTypeId = 1 OR
				Comment.CommentTypeId = 7 OR
				Comment.CommentTypeId = 30 OR
				Comment.CommentTypeId = 90 OR				
				Comment.CommentTypeId = 182 OR			
				Comment.CommentTypeId = 365
				)
			ORDER BY Comment.Created DESC;

			SELECT  @AVG_PERSON_IN = Comment.Evaluation 
			FROM Comment
			JOIN Internship ON Comment.InternshipId = Internship.Id
			JOIN Recruitment ON Internship.Id = Recruitment.Id
			JOIN BranchOffer ON Recruitment.BranchOfferId = BranchOffer.Id
			JOIN Branch ON BranchOffer.BranchId = Branch.Id
			WHERE (@UserId = Recruitment.PersonId OR @UserId =Branch.CompanyId) AND
				(
				Comment.CommentTypeId = 2 OR
				Comment.CommentTypeId = 8 OR
				Comment.CommentTypeId = 31 OR
				Comment.CommentTypeId = 91 OR				
				Comment.CommentTypeId = 183 OR			
				Comment.CommentTypeId = 366
				)
			ORDER BY Comment.Created DESC;	

			SELECT 
				@TOTAL_COUNT AS 'TOTAL_COUNT',
				@COMPANY_PUBLISH_ALLOWED_COUNT AS 'COMPANY_PUBLISH_ALLOWED_COUNT',
				@PERSON_PUBLISH_ALLOWED_COUNT AS 'PERSON_PUBLISH_ALLOWED_COUNT',
				ISNULL(@COMPANY_END, -1) AS 'COMPANY_END',		
				ISNULL(@PERSON_END, -1) AS 'PERSON_END',
				ISNULL(@AVG_COMPANY_IN, -1) AS 'AVG_COMPANY_IN',
				ISNULL(@AVG_PERSON_IN, -1) AS 'AVG_PERSON_IN';
		END
	ELSE
		BEGIN
			THROW 50009, 'NOT FOUND ITEM BY PARAMITERS',1;
		END
END;
--========================================================================================
GO
--========================================================================================