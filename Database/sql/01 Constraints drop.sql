--======================================================================================
--[User]
--Create
ALTER TABLE [dbo].[User] DROP 
CONSTRAINT User_Default_Id;
ALTER TABLE [dbo].[User] DROP 
CONSTRAINT User_Default_LastPasswordUpdate;
ALTER TABLE [dbo].[User] DROP 
CONSTRAINT User_Default_IsHideProfile;
ALTER TABLE [dbo].[User] DROP 
CONSTRAINT User_CHECK_IsHideProfile;
ALTER TABLE [dbo].[User] DROP 
CONSTRAINT User_UNIQUE_Login;

--======================================================================================
--[Notification]
--Create
ALTER TABLE [dbo].[Notification] DROP
CONSTRAINT Notification_Default_Id;
ALTER TABLE [dbo].[Notification] DROP
CONSTRAINT Notification_Default_Created;
ALTER TABLE [dbo].[Notification] DROP
CONSTRAINT Notification_Default_IsReadedByUser;
ALTER TABLE [dbo].[Notification] DROP
CONSTRAINT Notification_CHECK_IsReadedByUser;

--======================================================================================
--[Url]
--Create
ALTER TABLE [dbo].[Url] DROP
CONSTRAINT Url_UNIQUE_Path;

--======================================================================================
--======================================================================================
--[Person]
--Create
ALTER TABLE [dbo].[Person] DROP
CONSTRAINT Person_Default_Created;
ALTER TABLE [dbo].[Person] DROP
CONSTRAINT Person_Default_IsStudent;
ALTER TABLE [dbo].[Person] DROP
CONSTRAINT Person_Default_IsPublicProfile;
ALTER TABLE [dbo].[Person] DROP
CONSTRAINT Person_CHECK_IsStudent;
ALTER TABLE [dbo].[Person] DROP
CONSTRAINT Person_CHECK_IsPublicProfile;
ALTER TABLE [dbo].[Person] DROP
CONSTRAINT Person_CHECK_BirthDate;
ALTER TABLE [dbo].[Person] DROP
CONSTRAINT Person_UNIQUE_UrlSegment;
ALTER TABLE [dbo].[Person] DROP
CONSTRAINT Person_UNIQUE_ContactEmail;
ALTER TABLE [dbo].[Person] DROP
CONSTRAINT Person_UNIQUE_ContactPhoneNum;

--======================================================================================
--======================================================================================
--[Company]
--Create
ALTER TABLE [dbo].[Company] DROP
CONSTRAINT Company_Default_Created;
ALTER TABLE [dbo].[Company] DROP
CONSTRAINT Company_UNIQUE_UrlSegment;
ALTER TABLE [dbo].[Company] DROP
CONSTRAINT Company_UNIQUE_ContactEmail;
ALTER TABLE [dbo].[Company] DROP
CONSTRAINT Company_UNIQUE_Name;
ALTER TABLE [dbo].[Company] DROP
CONSTRAINT Company_UNIQUE_Regon;

--======================================================================================
--[Branch]
--Create
ALTER TABLE [dbo].[Branch] DROP
CONSTRAINT Branch_Default_Id;
ALTER TABLE [dbo].[Branch] DROP
CONSTRAINT Branch_UNIQUE_UrlSegment;

--======================================================================================
--[Offer]
--Create
ALTER TABLE [dbo].[Offer] DROP
CONSTRAINT Offer_Default_Id;
ALTER TABLE [dbo].[Offer] DROP
CONSTRAINT Offer_Default_IsForStudents;
ALTER TABLE [dbo].[Offer] DROP
CONSTRAINT Offer_CHECK_MinSalary;
ALTER TABLE [dbo].[Offer] DROP
CONSTRAINT Offer_CHECK_MaxSalary;
ALTER TABLE [dbo].[Offer] DROP
CONSTRAINT Offer_CHECK_IsNegotiatedSalary;
ALTER TABLE [dbo].[Offer] DROP
CONSTRAINT Offer_CHECK_IsForStudents;

--======================================================================================
--[BranchOffer]
--Create
ALTER TABLE [dbo].[BranchOffer] DROP
CONSTRAINT BranchOffer_Default_Id;
ALTER TABLE [dbo].[BranchOffer] DROP
CONSTRAINT BranchOffer_Default_Created;
ALTER TABLE [dbo].[BranchOffer] DROP
CONSTRAINT BranchOffer_Default_PublishStart;
ALTER TABLE [dbo].[BranchOffer] DROP
CONSTRAINT BranchOffer_Default_PublishEnd;
ALTER TABLE [dbo].[BranchOffer] DROP
CONSTRAINT BranchOffer_Default_LastUpdate;
ALTER TABLE [dbo].[BranchOffer] DROP
CONSTRAINT BranchOffer_CHECK_PublishEnd;
ALTER TABLE [dbo].[BranchOffer] DROP
CONSTRAINT BranchOffer_CHECK_WorkStart;
ALTER TABLE [dbo].[BranchOffer] DROP
CONSTRAINT BranchOffer_CHECK_WorkEnd;
ALTER TABLE [dbo].[BranchOffer] DROP
CONSTRAINT BranchOffer_UNIQUE_CONNECTION;

--======================================================================================
--[Recruitment]
--Create
ALTER TABLE [dbo].[Recruitment] DROP
CONSTRAINT Recruitment_Default_Id;
ALTER TABLE [dbo].[Recruitment] DROP
CONSTRAINT Recruitment_Default_Created;
ALTER TABLE [dbo].[Recruitment] DROP
CONSTRAINT Recruitment_CHECK_IsAccepted;

--======================================================================================
--[Internship]
--Create
ALTER TABLE [dbo].[Internship] DROP
CONSTRAINT Internship_Default_Created;
ALTER TABLE [dbo].[Internship] DROP
CONSTRAINT Internship_Default_ContractEndDate;


--======================================================================================
--[Comment]
--Create
ALTER TABLE [dbo].[Comment] DROP
CONSTRAINT CHECK_Comment_Evaluation;

--======================================================================================
--[Exception]
--Create
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
--Create
ALTER TABLE [dbo].[Address] DROP
CONSTRAINT Default_Address_Id;

--======================================================================================