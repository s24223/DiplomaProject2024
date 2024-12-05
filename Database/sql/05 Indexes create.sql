CREATE NONCLUSTERED INDEX IDX_AdministrativeDivision_ParentDivisionId
ON [dbo].[AdministrativeDivision] (ParentDivisionId)
INCLUDE (Id, Name, AdministrativeTypeId);
