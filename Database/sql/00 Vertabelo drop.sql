-- Created by Vertabelo (http://vertabelo.com)
-- Last modification date: 2024-10-20 12:01:06.525

-- foreign keys
ALTER TABLE Address DROP CONSTRAINT Address_Division;

ALTER TABLE Address DROP CONSTRAINT Address_Street;

ALTER TABLE AdministrativeDivision DROP CONSTRAINT AdministrativeDivision_AdministrativeType;

ALTER TABLE BranchCharacteristicsList DROP CONSTRAINT BranchCharacteristicsList_Branch;

ALTER TABLE BranchCharacteristicsList DROP CONSTRAINT BranchCharacteristicsList_Characteristic;

ALTER TABLE BranchCharacteristicsList DROP CONSTRAINT BranchCharacteristicsList_Quality;

ALTER TABLE BranchOffer DROP CONSTRAINT BranchOffer_Branch;

ALTER TABLE BranchOffer DROP CONSTRAINT BranchOffer_Offer;

ALTER TABLE Branch DROP CONSTRAINT Branch_Address;

ALTER TABLE Branch DROP CONSTRAINT Branch_Company;

ALTER TABLE CharacteristicColocation DROP CONSTRAINT CharacteristicColocation_Characteristic;

ALTER TABLE CharacteristicColocation DROP CONSTRAINT CharacteristicColocation_CharacteristicP;

ALTER TABLE Characteristic DROP CONSTRAINT Characteristic_CharacteristicType;

ALTER TABLE Comment DROP CONSTRAINT Comment_CommentType;

ALTER TABLE Comment DROP CONSTRAINT Comment_Internship;

ALTER TABLE Company DROP CONSTRAINT Company_User;

ALTER TABLE DivisionStreet DROP CONSTRAINT DivisionStreet_AdministrativeDivision;

ALTER TABLE DivisionStreet DROP CONSTRAINT DivisionStreet_Street;

ALTER TABLE AdministrativeDivision DROP CONSTRAINT Division_Division;

ALTER TABLE Internship DROP CONSTRAINT Internship_Recruitment;

ALTER TABLE Notification DROP CONSTRAINT Notification_NotificationSender;

ALTER TABLE Notification DROP CONSTRAINT Notification_NotificationStatus;

ALTER TABLE Notification DROP CONSTRAINT Notification_User;

ALTER TABLE OfferCharacteristicsList DROP CONSTRAINT OfferCharacteristicsList_Characteristic;

ALTER TABLE OfferCharacteristicsList DROP CONSTRAINT OfferCharacteristicsList_Offer;

ALTER TABLE OfferCharacteristicsList DROP CONSTRAINT OfferCharacteristicsList_Quality;

ALTER TABLE PersonCharacteristicsList DROP CONSTRAINT PersonCharacteristicsList_Characteristic;

ALTER TABLE PersonCharacteristicsList DROP CONSTRAINT PersonCharacteristicsList_Person;

ALTER TABLE PersonCharacteristicsList DROP CONSTRAINT PersonCharacteristicsList_Quality;

ALTER TABLE Person DROP CONSTRAINT Person_Address;

ALTER TABLE Person DROP CONSTRAINT Person_User;

ALTER TABLE Quality DROP CONSTRAINT Quality_CharacteristicType;

ALTER TABLE Recruitment DROP CONSTRAINT Recruitment_BranchOffer;

ALTER TABLE Recruitment DROP CONSTRAINT Recruitment_Person;

ALTER TABLE Street DROP CONSTRAINT Street_AdministrativeType;

ALTER TABLE Url DROP CONSTRAINT Url_UrlType;

ALTER TABLE Url DROP CONSTRAINT Url_User;

-- tables
DROP TABLE Address;

DROP TABLE AdministrativeDivision;

DROP TABLE AdministrativeType;

DROP TABLE Branch;

DROP TABLE BranchCharacteristicsList;

DROP TABLE BranchOffer;

DROP TABLE Characteristic;

DROP TABLE CharacteristicColocation;

DROP TABLE CharacteristicType;

DROP TABLE Comment;

DROP TABLE CommentType;

DROP TABLE Company;

DROP TABLE DivisionStreet;

DROP TABLE Exception;

DROP TABLE Internship;

DROP TABLE Notification;

DROP TABLE NotificationSender;

DROP TABLE NotificationStatus;

DROP TABLE Offer;

DROP TABLE OfferCharacteristicsList;

DROP TABLE Person;

DROP TABLE PersonCharacteristicsList;

DROP TABLE Quality;

DROP TABLE Recruitment;

DROP TABLE Street;

DROP TABLE Url;

DROP TABLE UrlType;

DROP TABLE "User";

-- End of file.

