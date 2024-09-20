ALTER TABLE [dbo].[User]
ADD CONSTRAINT Default_User_Id DEFAULT NEWID() FOR Id;

ALTER TABLE [dbo].[Branch]
ADD CONSTRAINT Default_Branch_Id DEFAULT NEWID() FOR Id;

ALTER TABLE [dbo].[Offer]
ADD CONSTRAINT Default_Offer_Id DEFAULT NEWID() FOR Id;

ALTER TABLE [dbo].[Internship]
ADD CONSTRAINT Default_Internship_Id DEFAULT NEWID() FOR Id;

ALTER TABLE [dbo].[UserProblem]
ADD CONSTRAINT Default_UserProblem_Id DEFAULT NEWID() FOR Id;

ALTER TABLE [dbo].[Exception]
ADD CONSTRAINT Default_Exception_Id DEFAULT NEWID() FOR Id;

ALTER TABLE [dbo].[Address]
ADD CONSTRAINT Default_Address_Id DEFAULT NEWID() FOR Id;