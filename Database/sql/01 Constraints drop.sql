--======================================================================================
--[User]
--Drop
ALTER TABLE [dbo].[User] DROP 
CONSTRAINT Default_User_Id;
ALTER TABLE [dbo].[User] DROP 
CONSTRAINT Default_User_LastPasswordUpdate;
ALTER TABLE [dbo].[User] DROP 
CONSTRAINT UNIQUE_User_Login;

--======================================================================================
--[UserProblem]
--Drop
ALTER TABLE [dbo].[UserProblem] DROP 
CONSTRAINT Default_UserProblem_Id;
ALTER TABLE [dbo].[UserProblem] DROP 
CONSTRAINT Default_UserProblem_Status;
ALTER TABLE [dbo].[UserProblem] DROP 
CONSTRAINT Default_UserProblem_Created;
ALTER TABLE [dbo].[UserProblem] DROP 
CONSTRAINT CHECK_UserProblem_Status;

--======================================================================================
--[Url]
--Create
ALTER TABLE [dbo].[Url] DROP
CONSTRAINT UNIQUE_Url_Path;

--======================================================================================
--[Person]
--Drop
ALTER TABLE [dbo].[Person] DROP
CONSTRAINT Default_Person_Created;
ALTER TABLE [dbo].[Person] DROP
CONSTRAINT Default_Person_IsStudent;
ALTER TABLE [dbo].[Person] DROP
CONSTRAINT Default_Person_IsPublicProfile;
ALTER TABLE [dbo].[Person] DROP
CONSTRAINT CHECK_Person_IsStudent;
ALTER TABLE [dbo].[Person] DROP
CONSTRAINT CHECK_Person_IsPublicProfile;
ALTER TABLE [dbo].[Person] DROP
CONSTRAINT UNIQUE_Person_UrlSegment;
ALTER TABLE [dbo].[Person] DROP
CONSTRAINT UNIQUE_Person_ContactEmail;

--======================================================================================
--[Company]
--Drop
ALTER TABLE [dbo].[Company] DROP
CONSTRAINT Default_Company_Created;
ALTER TABLE [dbo].[Company] DROP
CONSTRAINT UNIQUE_Company_UrlSegment;
ALTER TABLE [dbo].[Company] DROP
CONSTRAINT UNIQUE_Company_ContactEmail;
ALTER TABLE [dbo].[Company] DROP
CONSTRAINT UNIQUE_Company_Name;
ALTER TABLE [dbo].[Company] DROP
CONSTRAINT UNIQUE_Company_Regon;

--======================================================================================
--[Branch]
--Drop
ALTER TABLE [dbo].[Branch] DROP
CONSTRAINT Default_Branch_Id;
ALTER TABLE [dbo].[Branch] DROP
CONSTRAINT UNIQUE_Branch_UrlSegment;

--======================================================================================
--[Offer]
--Drop
ALTER TABLE [dbo].[Offer] DROP
CONSTRAINT Default_Offer_Id;
ALTER TABLE [dbo].[Offer] DROP
CONSTRAINT Default_Offer_IsForStudents;
ALTER TABLE [dbo].[Offer] DROP
CONSTRAINT CHECK_Offer_MinSalary;
ALTER TABLE [dbo].[Offer] DROP
CONSTRAINT CHECK_Offer_MaxSalary;
ALTER TABLE [dbo].[Offer] DROP
CONSTRAINT CHECK_Offer_IsNegotiatedSalary;
ALTER TABLE [dbo].[Offer] DROP
CONSTRAINT CHECK_Offer_IsForStudents;

--======================================================================================
--[BranchOffer]
--Drop
ALTER TABLE [dbo].[BranchOffer] DROP
CONSTRAINT Default_BranchOffer_PublishStart;
ALTER TABLE [dbo].[BranchOffer] DROP
CONSTRAINT Default_BranchOffer_PublishEnd;
ALTER TABLE [dbo].[BranchOffer] DROP
CONSTRAINT Default_BranchOffer_LastUpdate;
ALTER TABLE [dbo].[BranchOffer] DROP
CONSTRAINT CHECK_BranchOffer_PublishEnd; 
ALTER TABLE [dbo].[BranchOffer] DROP
CONSTRAINT CHECK_BranchOffer_WorkStart;
ALTER TABLE [dbo].[BranchOffer] DROP
CONSTRAINT CHECK_BranchOffer_WorkEnd;

--======================================================================================
--[Recruitment]
--Drop
ALTER TABLE [dbo].[Recruitment] DROP 
CONSTRAINT Default_Recruitment_ApplicationDate;
ALTER TABLE [dbo].[Recruitment] DROP 
CONSTRAINT CHECK_Recruitment_IsAccepted;

--======================================================================================
--[Internship]
--Drop
ALTER TABLE [dbo].[Internship] DROP
CONSTRAINT Default_Internship_Id;

--======================================================================================
--[Comment]
--Drop
ALTER TABLE [dbo].[Comment] DROP
CONSTRAINT CHECK_Comment_Evaluation;

--======================================================================================
--[Exception]
--Drop
ALTER TABLE [dbo].[Exception] DROP
CONSTRAINT Default_Exception_Id;
ALTER TABLE [dbo].[Exception] DROP
CONSTRAINT Default_Exception_Created;
ALTER TABLE [dbo].[Exception] DROP
CONSTRAINT Default_Exception_Status;
ALTER TABLE [dbo].[Exception] DROP
CONSTRAINT CHECK_Exception_Status;

--======================================================================================
--[Address]
--Drop
ALTER TABLE [dbo].[Address] DROP
CONSTRAINT Default_Address_Id;

--======================================================================================