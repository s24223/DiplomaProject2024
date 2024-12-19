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
CREATE OR ALTER PROCEDURE [dbo].[FIND_AdministrativeDivision_Street]
	@WOJ_NAME NVARCHAR(MAX),
	@POW_NAME NVARCHAR(MAX) = NULL,	
	@GMI_NAME NVARCHAR(MAX) = NULL,
	@CITY_NAME NVARCHAR(MAX),
	@DZIELNICA_NAME NVARCHAR(MAX) = NULL, -- OPTONAL, IF HAVE CHILDS USE
	@STREET_NAME NVARCHAR(MAX) = NULL
AS 
BEGIN

	DECLARE @ID INT;
	DECLARE @WOJ_ID INT;
	DECLARE @POW_ID INT = NULL;
	DECLARE @GMI_ID INT = NULL;	
	DECLARE @CITY_ID INT = NULL;	
	DECLARE @DZIELNICA_ID INT = NULL;
	DECLARE @STREET_ID INT = NULL;
	
	-- WOJEWODSTWO
	SELECT TOP 1 @WOJ_ID = ID FROM AdministrativeDivision 
	WHERE  
		AdministrativeDivision.Name LIKE @WOJ_NAME AND
		ParentDivisionId IS NULL;

	IF @WOJ_ID IS NOT NULL
		BEGIN
			SET @ID = @WOJ_ID;
		END
	ELSE
		BEGIN
			THROW 50010, 'WOJ NOT FOUND',1;
		END;

	--POWIAT, JESLI ISTNIEJE
	IF @POW_NAME IS NOT NULL
		BEGIN 
			SELECT TOP 1 @POW_ID = ID FROM AdministrativeDivision 
			WHERE 
				AdministrativeDivision.ParentDivisionId = @ID AND 
				AdministrativeDivision.Name LIKE @POW_NAME
			ORDER BY AdministrativeTypeId;

			IF @POW_ID IS NOT NULL
				BEGIN
					SET @ID = @POW_ID;
				END
			ELSE
				BEGIN
					THROW 50010, 'POW NOT FOUND',1;
				END;
		END;

	--GMINA, JESLI ISTNIEJE
	IF @GMI_NAME IS NOT NULL
		BEGIN 
			SELECT TOP 1 @GMI_ID = ID FROM AdministrativeDivision 
			WHERE 
				AdministrativeDivision.ParentDivisionId = @ID AND 
				AdministrativeDivision.Name LIKE @GMI_NAME
			ORDER BY AdministrativeTypeId;
			
			IF @GMI_ID IS NOT NULL
				BEGIN
					SET @ID = @GMI_ID;
				END
			ELSE
				BEGIN
					THROW 50010, 'GMI NOT FOUND',1;
				END;
		END;

	--SZUKAMY  PO DZECIACH
	WHILE @CITY_ID != @ID OR @CITY_ID IS NULL
		BEGIN 
			IF @CITY_ID IS NOT NULL
				BEGIN
					SET @ID = @CITY_ID;
				END	

			PRINT ('1 '+CAST (@CITY_ID AS VARCHAR));
			WITH CHILDS AS
			(
				SELECT  
					[AD].[Id] ,
					[AD].[Name],
					[AD].[ParentDivisionId] ,
					[AD].[AdministrativeTypeId] 
				FROM [dbo].[AdministrativeDivision] AS [AD]
				WHERE ParentDivisionId = @ID

				UNION ALL

				SELECT 
					[A].[Id] ,
					[A].[Name] ,
					[A].[ParentDivisionId] ,
					[A].[AdministrativeTypeId] 
				FROM [dbo].[AdministrativeDivision] AS [A]
				JOIN CHILDS AS C ON  A.ParentDivisionId = C.ID
			)
			SELECT TOP 1 @CITY_ID = ID FROM CHILDS AS C
			WHERE C.Name LIKE @CITY_NAME
			ORDER BY AdministrativeTypeId;

			PRINT ('2 '+CAST (@CITY_ID AS VARCHAR));
					
		END;
	
	-- TU MOZE POZNIEJ WYNIKNAC PROBLEM
	IF EXISTS (
		SELECT * FROM AdministrativeDivision AS AD
		WHERE AD.ParentDivisionId = @ID
		)
		BEGIN
			SELECT TOP 1 @DZIELNICA_ID = Id FROM AdministrativeDivision AS AD
			WHERE 
				AD.ParentDivisionId = @ID AND 
				AD.Name LIKE '%'+@DZIELNICA_NAME+'%';

			IF @DZIELNICA_ID IS NOT NULL
				BEGIN
					SET @ID = @DZIELNICA_ID;
				END
			ELSE
				BEGIN
					THROW 50010, 'DZIELNICA NOT FOUND',1;
				END;
		END;

	IF	@STREET_NAME IS NOT NULL AND EXISTS(
		SELECT * FROM DivisionStreet WHERE DivisionId = @ID 
		)
		BEGIN
			WITH STREET_STRING AS
			(
				SELECT 
					S.Id, 
					LTRIM(RTRIM(S.Name +' '+ISNULL(ADT.Name, ''))) AS STRING_NAME
				FROM AdministrativeDivision AS AD
				LEFT JOIN DivisionStreet AS DS ON AD.Id = DS.DivisionId
				LEFT JOIN Street AS S ON DS.StreetId = S.Id
				LEFT JOIN AdministrativeType AS ADT ON S.AdministrativeTypeId = ADT.Id
				WHERE AD.Id = @ID
			),
			STREET_comparison AS(
				SELECT 
					S.Id, 
					S.STRING_NAME,
					COUNT(1) AS MatchingWordsCount
				FROM STREET_STRING AS S
				CROSS APPLY STRING_SPLIT(@STREET_NAME, ' ') AS inputWords
				CROSS APPLY STRING_SPLIT(S.STRING_NAME, ' ') AS nameWords
				WHERE nameWords.value = inputWords.value
				GROUP BY S.Id, S.STRING_NAME
			)
			SELECT TOP 1 @STREET_ID = ID  
			FROM STREET_comparison
			ORDER BY MatchingWordsCount DESC;			
		END;

	SELECT @ID AS DIVISON_ID, @STREET_ID AS STREET_ID;
END;

--========================================================================================
GO
--========================================================================================

CREATE OR ALTER PROCEDURE DIVISION_IDS_SELECTOR
	@WOJ_NAME NVARCHAR(MAX), -- = 
	 @DIVISION_NAME NVARCHAR(MAX) = NULL -- = 'WARSZAWA'
AS 
BEGIN	
	DECLARE @WOJ_ID INT;
	SELECT TOP 1 @WOJ_ID =[AD].[Id]
	FROM [dbo].[AdministrativeDivision] AS [AD]
	WHERE 
		[AD].[ParentDivisionId] IS NULL AND
		[AD].[Name] LIKE '%'+@WOJ_NAME+'%';

	IF @DIVISION_NAME IS NULL
		BEGIN
			SELECT NULL AS 'DIV_ID',
			--[Name] ,
			--[ParentDivisionId] ,
			--[AdministrativeTypeId] ,
			--[Level] ,
			--[PathIds],
			@WOJ_ID AS 'WOJ_ID';
		END;
	ELSE
		BEGIN
			WITH SELECT_CHILS_WOJ AS 
			(
				SELECT 
					[Id] ,
					[Name] ,
					[ParentDivisionId] ,
					[AdministrativeTypeId] ,
					[Level] ,
					[PathIds] 
				FROM [dbo].[AdministrativeDivision] AS [AD]
				WHERE 
					[AD].[PathIds] LIKE ''+CAST(@WOJ_ID AS NVARCHAR)+'-%' OR 
					[AD].ParentDivisionId = @WOJ_ID
			),
			SELECT_SIMILAR_DIVISIONS AS 
			(
				SELECT 
					[Id] ,
					[Name] ,
					[ParentDivisionId] ,
					[AdministrativeTypeId] ,
					[Level] ,
					[PathIds] 
				FROM SELECT_CHILS_WOJ AS [C]
				CROSS APPLY (
					SELECT STRING_AGG(value, ' ') AS WordsSorted
					FROM STRING_SPLIT(@DIVISION_NAME, ' ')
					) AS inputWords
				CROSS APPLY (
					SELECT STRING_AGG(value, ' ') AS WordsSorted
					FROM STRING_SPLIT(C.Name, ' ')
					) AS nameWords
				WHERE inputWords.WordsSorted = nameWords.WordsSorted
			),
			SELECT_CHILDS_SIMILAR_DIVISIONS AS 
			(
				SELECT 
					[AD].[Id] ,
					[AD].[Name] ,
					[AD].[ParentDivisionId] ,
					[AD].[AdministrativeTypeId] ,
					[AD].[Level] ,
					[AD].[PathIds] 
				FROM [dbo].[AdministrativeDivision] AS [AD]
				JOIN SELECT_SIMILAR_DIVISIONS AS [PAR] 
				ON [AD].ID =  [PAR].[Id]

				UNION ALL

				SELECT 
					[AD2].[Id] ,
					[AD2].[Name] ,
					[AD2].[ParentDivisionId] ,
					[AD2].[AdministrativeTypeId] ,
					[AD2].[Level] ,
					[AD2].[PathIds] 
				FROM [dbo].[AdministrativeDivision] AS [AD2]
				JOIN  SELECT_CHILDS_SIMILAR_DIVISIONS AS [PAR2] ON
					[AD2].ParentDivisionId =  [PAR2].[Id]
			),
			SELECT_LOW_LEVEL_CHILDS AS 
			(
				SELECT 
					[SC].[Id] ,
					[SC].[Name] ,
					[SC].[ParentDivisionId] ,
					[SC].[AdministrativeTypeId] ,
					[SC].[Level] ,
					[SC].[PathIds] 
				FROM SELECT_CHILDS_SIMILAR_DIVISIONS AS [SC]
				WHERE NOT EXISTS 
					(
						SELECT 1 
						FROM [dbo].[AdministrativeDivision] AS [AD]
						WHERE [AD].[ParentDivisionId] = [SC].[Id]
					)
			)
			SELECT 
					DISTINCT
					[Id] AS 'DIV_ID',
					--[Name] ,
					--[ParentDivisionId] ,
					--[AdministrativeTypeId] ,
					--[Level] ,
					--[PathIds],
					@WOJ_ID AS 'WOJ_ID'
			FROM SELECT_LOW_LEVEL_CHILDS;
		END;	
END;
--========================================================================================
GO
--========================================================================================