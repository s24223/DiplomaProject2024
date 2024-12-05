CREATE OR ALTER TRIGGER Internship_UNIQUE_ContractNumber
ON [dbo].[Internship]
AFTER INSERT, UPDATE
AS 
BEGIN
	DECLARE @CompanyId uniqueidentifier; 
	DECLARE @ROW_COUNT INT;

	IF NOT EXISTS  
		( 
		SELECT 1 FROM INSERTED I 
		JOIN Recruitment ON I.Id = Recruitment.Id
		WHERE Recruitment.IsAccepted = 'Y'
		)
		BEGIN 
			THROW 50001, 'POSITIVE RECRUTMENT NOT EXIST',1; 	
		END
	ELSE 
		BEGIN
			SELECT TOP 1 @CompanyId = Branch.CompanyId
			FROM INSERTED I 
			JOIN Recruitment ON I.Id = Recruitment.Id
			JOIN BranchOffer ON Recruitment.BranchOfferId = BranchOffer.Id
			JOIN Branch ON BranchOffer.BranchId = Branch.Id
		END;

	WITH QWERY AS 
	(
		--DISTINCT
		SELECT Internship.ContractNumber
		FROM INSERTED I 
		JOIN Internship ON I.ContractNumber = Internship.ContractNumber
		JOIN Recruitment ON Internship.Id = Recruitment.Id
		JOIN BranchOffer ON Recruitment.BranchOfferId = BranchOffer.Id
		JOIN Branch ON BranchOffer.BranchId = Branch.Id
		WHERE 
			--Recruitment.IsAccepted = 'Y' AND 
			Branch.CompanyId = @CompanyId AND 
			Internship.Id != I.Id
	)
	SELECT @ROW_COUNT = COUNT(1) FROM QWERY;

	IF @ROW_COUNT > 0
	BEGIN 
		THROW 50002, 'DUPLICATE',1;
	END;
END;

--==============================================================================================
GO
--==============================================================================================
CREATE OR ALTER TRIGGER Recruitment_Invalid_BranchOffer
ON [dbo].[Recruitment]
AFTER INSERT, UPDATE
AS
BEGIN
	--SELECT SYSDATETIME()  ,SYSDATETIMEOFFSET()  ,SYSUTCDATETIME()  ,CURRENT_TIMESTAMP  ,GETDATE()  ,GETUTCDATE(); 
	DECLARE @NOW DATETIME;
	SET @NOW = SYSDATETIME();
	IF NOT EXISTS (
		SELECT 1 
		FROM INSERTED I 
		JOIN BranchOffer ON  I.BranchOfferId = BranchOffer.Id 
		WHERE 
			BranchOffer.PublishStart <= @NOW AND 
			(
				BranchOffer.PublishEnd IS NULL OR 
				(BranchOffer.PublishEnd IS NOT NULL AND BranchOffer.PublishEnd >= @NOW)
			)
		)
		BEGIN
			THROW 50003, 'NOT EXIST VALID BranchOffer',1;
		END
	ELSE
		BEGIN
			IF EXISTS 
			(
				SELECT 1 FROM INSERTED I 
				JOIN BranchOffer ON  I.BranchOfferId =BranchOffer.Id 
				JOIN BRANCH ON BranchOffer.BRANCHID = BRANCH.ID
				WHERE I.PersonId = BRANCH.CompanyId
			)
				BEGIN
					THROW 50004, 'PERSON APPLICATED INTO OFFER OF HIS COMPANY',1;				
				END;
		END;
END;

