CREATE NONCLUSTERED INDEX IDX_AdministrativeDivision_ParentDivisionId
ON [dbo].[AdministrativeDivision] (ParentDivisionId)
INCLUDE (Id, Name, AdministrativeTypeId);

CREATE NONCLUSTERED INDEX IDX_AdministrativeDivision_Name
ON [dbo].[AdministrativeDivision] (Name)
INCLUDE (Id, ParentDivisionId, AdministrativeTypeId);


CREATE NONCLUSTERED INDEX IDX_Street_Name
ON [dbo].[Street] (Name)
INCLUDE (Id, AdministrativeTypeId);

CREATE NONCLUSTERED INDEX IDX_DivisionStreet_Covered
ON [dbo].[DivisionStreet] (DivisionId, StreetId);
