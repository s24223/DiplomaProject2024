--======================================================================================
--[User]
--Drop
ALTER TABLE [dbo].[User]
DROP CONSTRAINT Default_User_Id;
ALTER TABLE [dbo].[User]
DROP CONSTRAINT Default_User_LastUpdatePassword;
ALTER TABLE [dbo].[User]
DROP CONSTRAINT UNIQUE_User_LoginEmail;

--Create
ALTER TABLE [dbo].[User]
ADD 
CONSTRAINT Default_User_Id DEFAULT NEWID() FOR [Id],
CONSTRAINT Default_User_LastUpdatePassword DEFAULT GETDATE() FOR [LastUpdatePassword],
CONSTRAINT UNIQUE_User_LoginEmail UNIQUE ([LoginEmail]);

--======================================================================================
--[Company]
--Drop
ALTER TABLE [dbo].[Company]
DROP CONSTRAINT Default_Company_CreateDate;
ALTER TABLE [dbo].[Company]
DROP CONSTRAINT UNIQUE_Company_UrlSegment;
ALTER TABLE [dbo].[Company]
DROP CONSTRAINT UNIQUE_Company_ContactEmail;
ALTER TABLE [dbo].[Company]
DROP CONSTRAINT UNIQUE_Company_Name;
ALTER TABLE [dbo].[Company]
DROP CONSTRAINT UNIQUE_Company_Regon;

--Create
ALTER TABLE [dbo].[Company]
ADD 
CONSTRAINT Default_Company_CreateDate DEFAULT CAST(GETDATE() AS DATE) FOR [CreateDate],
CONSTRAINT UNIQUE_Company_UrlSegment UNIQUE ([UrlSegment]),
CONSTRAINT UNIQUE_Company_ContactEmail UNIQUE ([ContactEmail]),
CONSTRAINT UNIQUE_Company_Name UNIQUE ([Name]),
CONSTRAINT UNIQUE_Company_Regon UNIQUE ([Regon]);


--======================================================================================
--[Person]
--Drop
ALTER TABLE [dbo].[Person]
DROP CONSTRAINT Default_Person_CreateDate;
ALTER TABLE [dbo].[Person]
DROP CONSTRAINT Default_Person_IsStudent;
ALTER TABLE [dbo].[Person]
DROP CONSTRAINT Default_Person_IsPublicProfile;
ALTER TABLE [dbo].[Person]
DROP CONSTRAINT CHECK_Person_IsStudent;
ALTER TABLE [dbo].[Person]
DROP CONSTRAINT CHECK_Person_IsPublicProfile;
ALTER TABLE [dbo].[Person]
DROP CONSTRAINT UNIQUE_Person_UrlSegment;
ALTER TABLE [dbo].[Person]
DROP CONSTRAINT UNIQUE_Person_ContactEmail;

--Create
ALTER TABLE [dbo].[Person]
ADD 
CONSTRAINT Default_Person_CreateDate DEFAULT CAST(GETDATE() AS DATE) FOR [CreateDate],
CONSTRAINT Default_Person_IsStudent DEFAULT 'N' FOR [IsStudent],
CONSTRAINT Default_Person_IsPublicProfile DEFAULT 'N' FOR [IsPublicProfile],
CONSTRAINT CHECK_Person_IsStudent CHECK (UPPER([IsStudent]) IN ('Y', 'N')),
CONSTRAINT CHECK_Person_IsPublicProfile CHECK (UPPER([IsPublicProfile]) IN ('Y', 'N')),
CONSTRAINT UNIQUE_Person_UrlSegment UNIQUE ([UrlSegment]),
CONSTRAINT UNIQUE_Person_ContactEmail UNIQUE ([ContactEmail]);

--======================================================================================
--[UserProblem]
--Drop
ALTER TABLE [dbo].[UserProblem]
DROP CONSTRAINT Default_UserProblem_Id;
ALTER TABLE [dbo].[UserProblem]
DROP CONSTRAINT Default_UserProblem_Status;
ALTER TABLE [dbo].[UserProblem]
DROP CONSTRAINT Default_UserProblem_DateTime;
ALTER TABLE [dbo].[UserProblem]
DROP CONSTRAINT CHECK_UserProblem_Status;

--Create
ALTER TABLE [dbo].[UserProblem]
ADD 
CONSTRAINT Default_UserProblem_Id DEFAULT NEWID() FOR [Id],
CONSTRAINT Default_UserProblem_Status DEFAULT 'C' FOR [Status],
CONSTRAINT Default_UserProblem_DateTime DEFAULT GETDATE() FOR [DateTime],
CONSTRAINT CHECK_UserProblem_Status CHECK (UPPER([Status]) IN ('C', 'V', 'D'));

--======================================================================================
--[Branch]
--Drop
ALTER TABLE [dbo].[Branch]
DROP CONSTRAINT Default_Branch_Id;
ALTER TABLE [dbo].[Branch]
DROP CONSTRAINT UNIQUE_Branch_UrlSegment;

--Create
ALTER TABLE [dbo].[Branch]
ADD 
CONSTRAINT Default_Branch_Id DEFAULT NEWID() FOR [Id],
CONSTRAINT UNIQUE_Branch_UrlSegment UNIQUE ([CompanyId], [UrlSegment]);

--======================================================================================
--[Offer]
--Drop
ALTER TABLE [dbo].[Offer]
DROP CONSTRAINT Default_Offer_Id;
ALTER TABLE [dbo].[Offer]
DROP CONSTRAINT Default_Offer_ForStudents;
ALTER TABLE [dbo].[Offer]
DROP CONSTRAINT CHECK_Offer_MinSalary;
ALTER TABLE [dbo].[Offer]
DROP CONSTRAINT CHECK_Offer_MaxSalary;
ALTER TABLE [dbo].[Offer]
DROP CONSTRAINT CHECK_Offer_NegotiatedSalary;
ALTER TABLE [dbo].[Offer]
DROP CONSTRAINT CHECK_Offer_ForStudents;
--Create
ALTER TABLE [dbo].[Offer]
ADD 
CONSTRAINT Default_Offer_Id DEFAULT NEWID() FOR [Id],
CONSTRAINT Default_Offer_ForStudents DEFAULT 'N' FOR [ForStudents],
CONSTRAINT CHECK_Offer_MinSalary CHECK ([MinSalary] >= 0 OR [MinSalary] IS NULL),
CONSTRAINT CHECK_Offer_MaxSalary CHECK ((
[MaxSalary] >= 0 AND
[MinSalary] IS NOT NULL AND 
[MaxSalary] >= [MinSalary] 
) OR [MaxSalary] IS NULL),
CONSTRAINT CHECK_Offer_NegotiatedSalary CHECK (UPPER([NegotiatedSalary]) IN ('Y', 'N') OR [NegotiatedSalary] IS NULL),
CONSTRAINT CHECK_Offer_ForStudents CHECK (UPPER([ForStudents]) IN ('Y', 'N'));

--======================================================================================
--[BranchOffer]
--Drop
ALTER TABLE [dbo].[BranchOffer]
DROP CONSTRAINT Default_BranchOffer_PublishStart;
ALTER TABLE [dbo].[BranchOffer]
DROP CONSTRAINT Default_BranchOffer_PublishEnd;
ALTER TABLE [dbo].[BranchOffer]
DROP CONSTRAINT Default_BranchOffer_LastUpdate;
ALTER TABLE [dbo].[BranchOffer]
DROP CONSTRAINT CHECK_BranchOffer_PublishEnd;
ALTER TABLE [dbo].[BranchOffer]
DROP CONSTRAINT CHECK_BranchOffer_WorkStart;
ALTER TABLE [dbo].[BranchOffer]
DROP CONSTRAINT CHECK_BranchOffer_WorkEnd;

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
--Drop
ALTER TABLE [dbo].[Recruitment]
DROP CONSTRAINT Default_Recruitment_ApplicationDate;
ALTER TABLE [dbo].[Recruitment]
DROP CONSTRAINT CHECK_Recruitment_AcceptedRejected;

--Create
ALTER TABLE [dbo].[Recruitment]
ADD 
CONSTRAINT Default_Recruitment_ApplicationDate DEFAULT GETDATE() FOR [ApplicationDate],
CONSTRAINT CHECK_Recruitment_AcceptedRejected CHECK (UPPER([AcceptedRejected]) IN ('Y', 'N') OR [AcceptedRejected] IS NULL);

--======================================================================================
--[Internship]
--Drop
ALTER TABLE [dbo].[Internship]
DROP CONSTRAINT Default_Internship_Id;

--Create
ALTER TABLE [dbo].[Internship]
ADD 
CONSTRAINT Default_Internship_Id DEFAULT NEWID() FOR [Id];

--======================================================================================
--[Comment]
--Drop
ALTER TABLE [dbo].[Comment]
DROP CONSTRAINT CHECK_Comment_Evaluation;

--Create
ALTER TABLE [dbo].[Comment]
ADD 
CONSTRAINT CHECK_Comment_Evaluation CHECK ([Evaluation] BETWEEN 1 AND 5 OR [Evaluation] IS NULL);

--======================================================================================
--[Exception]
--Drop
ALTER TABLE [dbo].[Exception]
DROP CONSTRAINT Default_Exception_Id;
ALTER TABLE [dbo].[Exception]
DROP CONSTRAINT Default_Exception_DateTime;
ALTER TABLE [dbo].[Exception]
DROP CONSTRAINT Default_Exception_Status;
ALTER TABLE [dbo].[Exception]
DROP CONSTRAINT CHECK_Exception_Status;

--Create
ALTER TABLE [dbo].[Exception]
ADD 
CONSTRAINT Default_Exception_Id DEFAULT NEWID() FOR [Id],
CONSTRAINT Default_Exception_DateTime DEFAULT GETDATE() FOR [DateTime],
CONSTRAINT Default_Exception_Status DEFAULT 'C' FOR [Status],
CONSTRAINT CHECK_Exception_Status CHECK (UPPER([Status]) IN ('C', 'V', 'D'));

--======================================================================================
--[Address]
--Drop
ALTER TABLE [dbo].[Address]
DROP CONSTRAINT Default_Address_Id;

--Create
ALTER TABLE [dbo].[Address]
ADD 
CONSTRAINT Default_Address_Id DEFAULT NEWID() FOR [Id];

--======================================================================================