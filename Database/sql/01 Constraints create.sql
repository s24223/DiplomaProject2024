--======================================================================================
--[User]
--Create
ALTER TABLE [dbo].[User]
ADD 
CONSTRAINT User_Default_Id DEFAULT NEWID() FOR [Id],
CONSTRAINT User_Default_LastPasswordUpdate DEFAULT GETDATE() FOR [LastPasswordUpdate],
CONSTRAINT User_Default_IsHideProfile DEFAULT 'N' FOR [IsHideProfile],
CONSTRAINT User_CHECK_IsHideProfile CHECK (UPPER([IsHideProfile]) IN ('Y', 'N')),
CONSTRAINT User_UNIQUE_Login UNIQUE ([Login]);

--======================================================================================
--[Notification]
--Create
ALTER TABLE [dbo].[Notification]
ADD 
CONSTRAINT Notification_Default_Id DEFAULT NEWID() FOR [Id],
CONSTRAINT Notification_Default_Created DEFAULT GETDATE() FOR [Created],
CONSTRAINT Notification_Default_IsReadedByUser DEFAULT 'N' FOR [IsReadedByUser],
CONSTRAINT Notification_CHECK_IsReadedByUser CHECK (UPPER([IsReadedByUser]) IN ('Y', 'N'));

--======================================================================================
--[Url]
--Create
ALTER TABLE [dbo].[Url]
ADD 
CONSTRAINT Url_UNIQUE_Path UNIQUE ([Path], [UserId]);

--======================================================================================
--======================================================================================
--[Person]
--Create
ALTER TABLE [dbo].[Person]
ADD 
CONSTRAINT Person_Default_Created DEFAULT CAST(GETDATE() AS DATE) FOR [Created],
CONSTRAINT Person_Default_IsStudent DEFAULT 'N' FOR [IsStudent],
CONSTRAINT Person_Default_IsPublicProfile DEFAULT 'N' FOR [IsPublicProfile],
CONSTRAINT Person_CHECK_IsStudent CHECK (UPPER([IsStudent]) IN ('Y', 'N')),
CONSTRAINT Person_CHECK_IsPublicProfile CHECK (UPPER([IsPublicProfile]) IN ('Y', 'N')),
CONSTRAINT Person_CHECK_BirthDate CHECK ([BirthDate] < CAST(GETDATE() AS DATE) OR [BirthDate] IS NULL),
--CONSTRAINT Person_UNIQUE_UrlSegment UNIQUE ([UrlSegment]),
--CONSTRAINT Person_UNIQUE_ContactPhoneNum UNIQUE ([ContactPhoneNum]),
CONSTRAINT Person_UNIQUE_ContactEmail UNIQUE ([ContactEmail]);

--======================================================================================
--======================================================================================
--[Company]
--Create
ALTER TABLE [dbo].[Company]
ADD 
CONSTRAINT Company_Default_Created DEFAULT CAST(GETDATE() AS DATE) FOR [Created],
--CONSTRAINT Company_UNIQUE_UrlSegment UNIQUE ([UrlSegment]),
CONSTRAINT Company_UNIQUE_ContactEmail UNIQUE ([ContactEmail]),
CONSTRAINT Company_UNIQUE_Name UNIQUE ([Name]),
CONSTRAINT Company_UNIQUE_Regon UNIQUE ([Regon]);

--======================================================================================
--[Branch]
--Create
ALTER TABLE [dbo].[Branch]
ADD 
CONSTRAINT Branch_Default_Id DEFAULT NEWID() FOR [Id];
--CONSTRAINT Branch_UNIQUE_UrlSegment UNIQUE ([CompanyId], [UrlSegment]);

--======================================================================================
--[Offer]
--Create
ALTER TABLE [dbo].[Offer]
ADD 
CONSTRAINT Offer_Default_Id DEFAULT NEWID() FOR [Id],
CONSTRAINT Offer_Default_IsForStudents DEFAULT 'N' FOR [IsForStudents],
CONSTRAINT Offer_CHECK_MinSalary CHECK ([MinSalary] >= 0 OR [MinSalary] IS NULL),
CONSTRAINT Offer_CHECK_MaxSalary CHECK ((
[MaxSalary] >= 0 AND
[MinSalary] IS NOT NULL AND 
[MaxSalary] >= [MinSalary] 
) OR [MaxSalary] IS NULL),
CONSTRAINT Offer_CHECK_IsNegotiatedSalary CHECK (UPPER([IsNegotiatedSalary]) IN ('Y', 'N') OR [IsNegotiatedSalary] IS NULL),
CONSTRAINT Offer_CHECK_IsForStudents CHECK (UPPER([IsForStudents]) IN ('Y', 'N'));

--======================================================================================
--[BranchOffer]
--Create
ALTER TABLE [dbo].[BranchOffer]
ADD 
CONSTRAINT BranchOffer_Default_Id DEFAULT NEWID() FOR [Id],
CONSTRAINT BranchOffer_Default_Created DEFAULT GETDATE() FOR [Created],
CONSTRAINT BranchOffer_Default_PublishStart DEFAULT GETDATE() FOR [PublishStart],
CONSTRAINT BranchOffer_Default_PublishEnd DEFAULT GETDATE() FOR [PublishEnd],
CONSTRAINT BranchOffer_Default_LastUpdate DEFAULT GETDATE() FOR [LastUpdate],
CONSTRAINT BranchOffer_CHECK_PublishEnd CHECK ((
[PublishStart] IS NOT NULL AND
[PublishEnd] >= [PublishStart]
)OR [PublishEnd] IS NULL),
CONSTRAINT BranchOffer_CHECK_WorkStart CHECK ((
[PublishEnd] IS NOT NULL AND
[WorkStart] > CAST([PublishEnd] AS DATE)) 
OR [WorkStart] IS NULL),
CONSTRAINT BranchOffer_CHECK_WorkEnd CHECK ((
[WorkStart] IS NOT NULL AND
[WorkEnd] >= [WorkStart] 
) OR [WorkEnd] IS NULL);
--CONSTRAINT BranchOffer_UNIQUE_CONNECTION UNIQUE ([BranchId], [OfferId], [Created]);

--======================================================================================
--[Recruitment]
--Create
ALTER TABLE [dbo].[Recruitment]
ADD 
CONSTRAINT Recruitment_Default_Id DEFAULT NEWID() FOR [Id],
CONSTRAINT Recruitment_Default_Created DEFAULT GETDATE() FOR [Created],
CONSTRAINT Recruitment_CHECK_IsAccepted CHECK (UPPER([IsAccepted]) IN ('Y', 'N') OR [IsAccepted] IS NULL),
CONSTRAINT Recruitment_UNIQUE UNIQUE ([PersonId], [BranchOfferId]);

--======================================================================================
--[Internship]
--Create
ALTER TABLE [dbo].[Internship]
ADD 
CONSTRAINT Internship_Default_Created DEFAULT GETDATE() FOR [Created],
CONSTRAINT Internship_Default_ContractEndDate CHECK (
[ContractEndDate] >= [ContractStartDate] OR [ContractEndDate] IS NULL);

--======================================================================================
--[Comment]
--Create
ALTER TABLE [dbo].[Comment]
ADD 
CONSTRAINT CHECK_Comment_Evaluation CHECK ([Evaluation] BETWEEN 1 AND 5 OR [Evaluation] IS NULL);

--======================================================================================
--[Exception]
--Create
ALTER TABLE [dbo].[Exception]
ADD 
CONSTRAINT Default_Exception_Id DEFAULT NEWID() FOR [Id],
CONSTRAINT Default_Exception_Created DEFAULT GETDATE() FOR [Created],
CONSTRAINT Default_Exception_Status DEFAULT 'C' FOR [Status],
CONSTRAINT CHECK_Exception_Status CHECK (UPPER([Status]) IN ('C', 'V', 'D'));

--======================================================================================
--[Address]
--Create
ALTER TABLE [dbo].[Address]
ADD 
CONSTRAINT Default_Address_Id DEFAULT NEWID() FOR [Id];

--======================================================================================