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
CREATE OR ALTER TRIGGER Comment_DateLimit
ON [dbo].[Comment]
AFTER INSERT, UPDATE
AS 
BEGIN
	DECLARE @Id INT;
	DECLARE @EndDate date;
	DECLARE @Created datetime;
	DECLARE @Diff_ContractEndDate INT = NULL;
	DECLARE @EXIST_COUNT_VALUES INT;
	DECLARE @ThrowString NVARCHAR(50);

	--START SET TO DECALRE VALUES
	SELECT 
		@EndDate = Internship.ContractEndDate,
		@Id = I.CommentTypeId,
		@Created = I.Created
	FROM inserted I
	JOIN Internship ON I.InternshipId = Internship.Id;			

	SELECT @EXIST_COUNT_VALUES = COUNT(1) FROM INSERTED I 
	JOIN [Comment] ON 
		Comment.CommentTypeId = I.CommentTypeId AND 
		Comment.InternshipId = I.InternshipId and 
		Comment.Created <> I.Created;		 

	IF @EndDate IS NOT NULL 
		BEGIN
			SET @Diff_ContractEndDate = DATEDIFF(DAY, @EndDate, CONVERT(DATE, @Created));
		END;
	--END SET TO DECALRE VALUES
	--PRINT @EXIST_COUNT_VALUES;

	IF @Id = 1000 OR @Id = 1001 
		BEGIN
			IF @Diff_ContractEndDate IS NULL
				BEGIN 
					THROW 50003, 'INTERSHIP EndDate IS NULL',1;
				END		
			ELSE IF @Diff_ContractEndDate < 0
				BEGIN 
					SET @ThrowString = ''+ CAST(ABS(@Diff_ContractEndDate) AS NVARCHAR);
					THROW 50004, @ThrowString,1;	
				END	
			ELSE IF @EXIST_COUNT_VALUES > 0
				BEGIN
					THROW 50005, 'DUPLICATE END OPINION',1;
				END
		END
	ELSE 
		BEGIN 
			IF @Diff_ContractEndDate IS NULL OR @Diff_ContractEndDate < 0
				BEGIN
					IF @EXIST_COUNT_VALUES > 0
						BEGIN 			
							DECLARE @LastCreated datetime;
							DECLARE @Diff INT;

							--GET LAST DATE
							SELECT TOP 1 @LastCreated = Comment.Created  FROM INSERTED I 
							JOIN [Comment] ON 
								Comment.CommentTypeId = I.CommentTypeId AND 
								Comment.InternshipId = I.InternshipId 
							ORDER BY Comment.Created DESC;
			
							--SET DIFFERENT DAYS AND ID CommentTypeId
							SET @Diff = DATEDIFF(DAY, @LastCreated, @Created ); 
					

							IF @Id = 1 OR @Id = 2
								BEGIN 
									SET @Diff = @Diff -1;
								END
							ELSE IF @Id = 7 OR @Id = 8
								BEGIN 
									SET @Diff = @Diff -7;
								END
							ELSE IF @Id = 30 OR @Id = 31
								BEGIN 
									SET @Diff = @Diff -30;
								END
							ELSE IF @Id = 90 OR @Id = 91
								BEGIN 
									SET @Diff = @Diff -90;
								END
							ELSE IF @Id = 182 OR @Id = 183
								BEGIN 
									SET @Diff = @Diff -182;
								END
							ELSE IF @Id = 365 OR @Id = 366
								BEGIN 
									SET @Diff = @Diff -365;
								END
							ELSE IF @Id = 1004 OR @Id = 1005
								BEGIN 
									THROW 50006, 'DUPLICATE ENABLE PUBLISHING',1; 
								END;

							IF @Diff < 0
								BEGIN
									SET @ThrowString = ''+ CAST(ABS(@Diff) AS NVARCHAR);
									THROW 50007, @ThrowString,1; 
								END
					END	
				END
			ELSE
				BEGIN
					THROW 50008, 'UNABLE SET THIS TYPE OF COMMENT BECAUSE CONTRACT END',1;
				END
		END	
END;
--==============================================================================================
GO
--==============================================================================================
