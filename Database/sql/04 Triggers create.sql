CREATE OR ALTER TRIGGER BranchOffer_CONFLICT_TIME
ON [BranchOffer]
INSTEAD OF INSERT, UPDATE
AS
BEGIN 
	DECLARE @BranchId uniqueidentifier;
	DECLARE @OfferId uniqueidentifier;
	DECLARE @PublishStart datetime;

	SELECT 
			@BranchId = BranchId, 
			@OfferId = OfferId, 
			@PublishStart = PublishStart 
	FROM inserted;

	DECLARE @IdExist uniqueidentifier;
	DECLARE @PublishStartExist datetime;
	DECLARE @PublishEndExist datetime;
	SELECT 
		@IdExist = Id,
		@PublishStartExist = PublishStart,
		@PublishEndExist = PublishEnd
	
	FROM [BranchOffer] WHERE 
			BranchId = @BranchId AND 
			OfferId = @OfferId AND
			PublishEnd >= @PublishStart;

	IF (
		@IdExist IS NOT NULL
	)
	BEGIN
		DECLARE @ERROR_DATA nvarchar(200);
		SET @ERROR_DATA = '[ID]: ' +CAST(@IdExist AS nvarchar(36))
				+ '; [PublishStart]: ' + CONVERT(varchar(20), @PublishStartExist, 120) + 
				+ '; [PublishEnd]: ' +  CONVERT(varchar(20), @PublishEndExist, 120) +';';
		RAISERROR('BranchOffer_CONFLICT_TIME, Nie mo¿na dodaæ lub zaktualizowaæ rekordu, poniewa¿ istnieje aktywny rekord dla %s.', 16, 1, @ERROR_DATA);
        ROLLBACK TRANSACTION;  -- Cofniêcie transakcji
        RETURN;
	END;

	IF (INSERTING) THEN
		INSERT INTO BranchOffer 
		(
			[Id],
			[BranchId],
			[OfferId],
			[Created],
			[PublishStart],
			[PublishEnd],
			[WorkStart],
			[WorkEnd],
			[LastUpdate]
		)
		SELECT 
			[Id],
			[BranchId],
			[OfferId],
			[Created],
			[PublishStart],
			[PublishEnd],
			[WorkStart],
			[WorkEnd],
			[LastUpdate]
		FROM inserted;
	 ELSIF (UPDATING) THEN
		UPDATE BranchOffer
			SET 
				BranchId = inserted.BranchId,
				OfferId = inserted.OfferId,
				Created = inserted.Created,
				PublishStart = inserted.PublishStart,
				PublishEnd = inserted.PublishEnd,
				WorkStart = inserted.WorkStart,
				WorkEnd = inserted.WorkEnd,
				LastUpdate = inserted.LastUpdate
			FROM inserted
			WHERE BranchOffer.Id = inserted.Id;
	 END IF;
END;