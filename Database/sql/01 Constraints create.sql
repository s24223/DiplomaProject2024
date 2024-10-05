--======================================================================================
--[User]
--Create
ALTER TABLE [dbo].[User]
ADD 
CONSTRAINT Default_User_Id DEFAULT NEWID() FOR [Id],
CONSTRAINT Default_User_LastPasswordUpdate DEFAULT GETDATE() FOR [LastPasswordUpdate],
CONSTRAINT UNIQUE_User_Login UNIQUE ([Login]);

--======================================================================================
--[UserProblem]
--Create
ALTER TABLE [dbo].[UserProblem]
ADD 
CONSTRAINT Default_UserProblem_Id DEFAULT NEWID() FOR [Id],
CONSTRAINT Default_UserProblem_Status DEFAULT 'C' FOR [Status],
CONSTRAINT Default_UserProblem_Created DEFAULT GETDATE() FOR [Created],
CONSTRAINT CHECK_UserProblem_Status CHECK (UPPER([Status]) IN ('C', 'V', 'D', 'A'));

--======================================================================================
--[Url]
--Create
ALTER TABLE [dbo].[Url]
ADD 
CONSTRAINT UNIQUE_Url_Path UNIQUE ([Path], [UserId]);

--======================================================================================
--[Person]
--Create
ALTER TABLE [dbo].[Person]
ADD 
CONSTRAINT Default_Person_Created DEFAULT CAST(GETDATE() AS DATE) FOR [Created],
CONSTRAINT Default_Person_IsStudent DEFAULT 'N' FOR [IsStudent],
CONSTRAINT Default_Person_IsPublicProfile DEFAULT 'N' FOR [IsPublicProfile],
CONSTRAINT CHECK_Person_IsStudent CHECK (UPPER([IsStudent]) IN ('Y', 'N')),
CONSTRAINT CHECK_Person_IsPublicProfile CHECK (UPPER([IsPublicProfile]) IN ('Y', 'N')),
CONSTRAINT UNIQUE_Person_UrlSegment UNIQUE ([UrlSegment]),
CONSTRAINT UNIQUE_Person_ContactEmail UNIQUE ([ContactEmail]);

--======================================================================================
--[Company]
--Create
ALTER TABLE [dbo].[Company]
ADD 
CONSTRAINT Default_Company_Created DEFAULT CAST(GETDATE() AS DATE) FOR [Created],
CONSTRAINT UNIQUE_Company_UrlSegment UNIQUE ([UrlSegment]),
CONSTRAINT UNIQUE_Company_ContactEmail UNIQUE ([ContactEmail]),
CONSTRAINT UNIQUE_Company_Name UNIQUE ([Name]),
CONSTRAINT UNIQUE_Company_Regon UNIQUE ([Regon]);

--======================================================================================
--[Branch]
--Create
ALTER TABLE [dbo].[Branch]
ADD 
CONSTRAINT Default_Branch_Id DEFAULT NEWID() FOR [Id],
CONSTRAINT UNIQUE_Branch_UrlSegment UNIQUE ([CompanyId], [UrlSegment]);

--======================================================================================
--[Offer]
--Create
ALTER TABLE [dbo].[Offer]
ADD 
CONSTRAINT Default_Offer_Id DEFAULT NEWID() FOR [Id],
CONSTRAINT Default_Offer_IsForStudents DEFAULT 'N' FOR [IsForStudents],
CONSTRAINT CHECK_Offer_MinSalary CHECK ([MinSalary] >= 0 OR [MinSalary] IS NULL),
CONSTRAINT CHECK_Offer_MaxSalary CHECK ((
[MaxSalary] >= 0 AND
[MinSalary] IS NOT NULL AND 
[MaxSalary] >= [MinSalary] 
) OR [MaxSalary] IS NULL),
CONSTRAINT CHECK_Offer_IsNegotiatedSalary CHECK (UPPER([IsNegotiatedSalary]) IN ('Y', 'N') OR [IsNegotiatedSalary] IS NULL),
CONSTRAINT CHECK_Offer_IsForStudents CHECK (UPPER([IsForStudents]) IN ('Y', 'N'));

--======================================================================================
--[BranchOffer]
--Create
ALTER TABLE [dbo].[BranchOffer]
ADD 
CONSTRAINT Default_BranchOffer_PublishStart DEFAULT GETDATE() FOR [PublishStart],
CONSTRAINT Default_BranchOffer_PublishEnd DEFAULT GETDATE() FOR [PublishEnd],
CONSTRAINT Default_BranchOffer_LastUpdate DEFAULT GETDATE() FOR [LastUpdate],
CONSTRAINT CHECK_BranchOffer_PublishEnd CHECK ([PublishEnd] >= GETDATE() OR [PublishEnd] IS NULL),
CONSTRAINT CHECK_BranchOffer_WorkStart CHECK ((
[WorkStart] >= CAST(GETDATE() AS DATE) AND 
[PublishEnd] IS NOT NULL AND
[WorkStart] > CAST([PublishEnd] AS DATE)) 
OR [WorkStart] IS NULL),
CONSTRAINT CHECK_BranchOffer_WorkEnd CHECK ((
[WorkEnd] >= CAST(GETDATE() AS DATE) AND 
[WorkStart] IS NOT NULL AND
[WorkEnd] >= [WorkStart] 
) OR [WorkEnd] IS NULL);

--======================================================================================
--[Recruitment]
--Create
ALTER TABLE [dbo].[Recruitment]
ADD 
CONSTRAINT Default_Recruitment_ApplicationDate DEFAULT GETDATE() FOR [ApplicationDate],
CONSTRAINT CHECK_Recruitment_IsAccepted CHECK (UPPER([IsAccepted]) IN ('Y', 'N') OR [IsAccepted] IS NULL);

--======================================================================================
--[Internship]
--Create
ALTER TABLE [dbo].[Internship]
ADD 
CONSTRAINT Default_Internship_Id DEFAULT NEWID() FOR [Id];

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