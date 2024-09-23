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
		JOIN HierarchyDownWithName AS [HD] ON [A].[ParentDivisionId]  = [HD].Id
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

GO

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

GO

--TESTS
EXECUTE SelectByStreetNameAndDivisionName
@DivisionName = 'WARSZAWA', @StreetName = 'OKOPOWA';

EXECUTE SelectAdministrativeDivisionUp
@DivisionId = 47207;

select * from Street