-- Created by Vertabelo (http://vertabelo.com)
-- Last modification date: 2024-10-20 12:01:06.525

-- tables
-- Table: Address
CREATE TABLE Address (
    Id uniqueidentifier  NOT NULL,
    DivisionId int  NOT NULL,
    StreetId int  NOT NULL,
    BuildingNumber nvarchar(100)  NOT NULL,
    ApartmentNumber nvarchar(100)  NULL,
    ZipCode nvarchar(10)  NOT NULL,
    CONSTRAINT Address_pk PRIMARY KEY  (Id)
);

-- Table: AdministrativeDivision
CREATE TABLE AdministrativeDivision (
    Id int  NOT NULL,
    Name nvarchar(100)  NOT NULL,
    ParentDivisionId int  NULL,
    AdministrativeTypeId int  NOT NULL,
    CONSTRAINT AdministrativeDivision_pk PRIMARY KEY  (Id)
);

-- Table: AdministrativeType
CREATE TABLE AdministrativeType (
    Id int  NOT NULL,
    Name nvarchar(100)  NOT NULL,
    CONSTRAINT AdministrativeType_pk PRIMARY KEY  (Id)
);

-- Table: Branch
CREATE TABLE Branch (
    CompanyId uniqueidentifier  NOT NULL,
    AddressId uniqueidentifier  NULL,
    Id uniqueidentifier  NOT NULL,
    UrlSegment varchar(100)  NULL,
    Name nvarchar(100)  NULL,
    Description nvarchar(max)  NULL,
    CONSTRAINT Branch_pk PRIMARY KEY  (Id)
);

-- Table: BranchCharacteristicsList
CREATE TABLE BranchCharacteristicsList (
    BranchId uniqueidentifier  NOT NULL,
    CharacteristicId int  NOT NULL,
    QualityId int  NULL,
    CONSTRAINT BranchCharacteristicsList_pk PRIMARY KEY  (BranchId)
);

-- Table: BranchOffer
CREATE TABLE BranchOffer (
    Id uniqueidentifier  NOT NULL,
    BranchId uniqueidentifier  NOT NULL,
    OfferId uniqueidentifier  NOT NULL,
    Created datetime  NOT NULL,
    PublishStart datetime  NOT NULL,
    PublishEnd datetime  NULL,
    WorkStart date  NULL,
    WorkEnd date  NULL,
    LastUpdate datetime  NOT NULL,
    CONSTRAINT BranchOffer_pk PRIMARY KEY  (Id)
);

-- Table: Characteristic
CREATE TABLE Characteristic (
    Id int  NOT NULL,
    Name nvarchar(100)  NOT NULL,
    Description ntext  NOT NULL,
    CharacteristicTypeId int  NOT NULL,
    CONSTRAINT Characteristic_pk PRIMARY KEY  (Id)
);

-- Table: CharacteristicColocation
CREATE TABLE CharacteristicColocation (
    ParentCharacteristicId int  NOT NULL,
    ChildCharacteristicId int  NOT NULL,
    CONSTRAINT CharacteristicColocation_pk PRIMARY KEY  (ParentCharacteristicId,ChildCharacteristicId)
);

-- Table: CharacteristicType
CREATE TABLE CharacteristicType (
    Id int  NOT NULL,
    Name nvarchar(100)  NOT NULL,
    Description ntext  NOT NULL,
    CONSTRAINT CharacteristicType_pk PRIMARY KEY  (Id)
);

-- Table: Comment
CREATE TABLE Comment (
    InternshipId uniqueidentifier  NOT NULL,
    CommentTypeId int  NOT NULL,
    Created datetime  NOT NULL,
    Description nvarchar(max)  NOT NULL,
    Evaluation int  NULL,
    CONSTRAINT Comment_pk PRIMARY KEY  (CommentTypeId,Created,InternshipId)
);

-- Table: CommentType
CREATE TABLE CommentType (
    Id int  NOT NULL,
    Name nvarchar(100)  NOT NULL,
    Description nvarchar(200)  NOT NULL,
    CONSTRAINT CommentType_pk PRIMARY KEY  (Id)
);

-- Table: Company
CREATE TABLE Company (
    UserId uniqueidentifier  NOT NULL,
    Created date  NOT NULL,
    Name nvarchar(100)  NULL,
    Regon varchar(14)  NULL,
    ContactEmail nvarchar(100)  NULL,
    UrlSegment varchar(100)  NULL,
    LogoUrl nvarchar(max)  NULL,
    Description nvarchar(max)  NULL,
    CONSTRAINT Company_pk PRIMARY KEY  (UserId)
);

-- Table: DivisionStreet
CREATE TABLE DivisionStreet (
    DivisionId int  NOT NULL,
    StreetId int  NOT NULL,
    CONSTRAINT DivisionStreet_pk PRIMARY KEY  (DivisionId,StreetId)
);

-- Table: Exception
CREATE TABLE Exception (
    Id uniqueidentifier  NOT NULL,
    Created datetime  NOT NULL,
    ExceptionType varchar(100)  NOT NULL,
    Message nvarchar(max)  NOT NULL,
    AdditionalData nvarchar(max)  NULL,
    Status char(1)  NOT NULL,
    CONSTRAINT Exception_pk PRIMARY KEY  (Id)
);

-- Table: Internship
CREATE TABLE Internship (
    Id uniqueidentifier  NOT NULL,
    ContractNumber nvarchar(100)  NOT NULL,
    Created datetime  NOT NULL,
    ContractStartDate date  NOT NULL,
    ContractEndDate date  NULL,
    CONSTRAINT Internship_pk PRIMARY KEY  (Id)
);

-- Table: Notification
CREATE TABLE Notification (
    UserId uniqueidentifier  NULL,
    Email nvarchar(100)  NULL,
    NotificationSenderId int  NOT NULL,
    NotificationStatusId int  NOT NULL,
    Id uniqueidentifier  NOT NULL,
    Created datetime  NOT NULL,
    Completed datetime  NULL,
    PreviousProblemId uniqueidentifier  NULL,
    IdAppProblem uniqueidentifier  NULL,
    UserMessage nvarchar(max)  NULL,
    Response nvarchar(max)  NULL,
    IsReadedByUser char(1)  NOT NULL,
    CONSTRAINT Notification_pk PRIMARY KEY  (Id)
);

-- Table: NotificationSender
CREATE TABLE NotificationSender (
    Id int  NOT NULL,
    Name nvarchar(100)  NOT NULL,
    Description nvarchar(200)  NOT NULL,
    CONSTRAINT NotificationSender_pk PRIMARY KEY  (Id)
);

-- Table: NotificationStatus
CREATE TABLE NotificationStatus (
    Id int  NOT NULL,
    Name nvarchar(100)  NOT NULL,
    CONSTRAINT NotificationStatus_pk PRIMARY KEY  (Id)
);

-- Table: Offer
CREATE TABLE Offer (
    Id uniqueidentifier  NOT NULL,
    Name nvarchar(100)  NOT NULL,
    Description nvarchar(max)  NOT NULL,
    MinSalary money  NULL,
    MaxSalary money  NULL,
    IsNegotiatedSalary char(1)  NULL,
    IsForStudents char(1)  NOT NULL,
    CONSTRAINT Offer_pk PRIMARY KEY  (Id)
);

-- Table: OfferCharacteristicsList
CREATE TABLE OfferCharacteristicsList (
    OfferId uniqueidentifier  NOT NULL,
    CharacteristicId int  NOT NULL,
    QualityId int  NULL,
    CONSTRAINT OfferCharacteristicsList_pk PRIMARY KEY  (OfferId)
);

-- Table: Person
CREATE TABLE Person (
    UserId uniqueidentifier  NOT NULL,
    AddressId uniqueidentifier  NULL,
    Created date  NOT NULL,
    Name nvarchar(100)  NULL,
    Surname nvarchar(100)  NULL,
    BirthDate date  NULL,
    ContactEmail nvarchar(100)  NULL,
    ContactPhoneNum varchar(11)  NULL,
    UrlSegment varchar(100)  NULL,
    LogoUrl nvarchar(max)  NULL,
    Description nvarchar(max)  NULL,
    IsStudent char(1)  NOT NULL,
    IsPublicProfile char(1)  NOT NULL,
    CONSTRAINT Person_pk PRIMARY KEY  (UserId)
);

-- Table: PersonCharacteristicsList
CREATE TABLE PersonCharacteristicsList (
    PersonId uniqueidentifier  NOT NULL,
    CharacteristicId int  NOT NULL,
    QualityId int  NULL,
    CONSTRAINT PersonCharacteristicsList_pk PRIMARY KEY  (PersonId)
);

-- Table: Quality
CREATE TABLE Quality (
    Id int  NOT NULL,
    Name nvarchar(100)  NOT NULL,
    Description ntext  NOT NULL,
    CharacteristicTypeId int  NULL,
    CONSTRAINT Quality_pk PRIMARY KEY  (Id)
);

-- Table: Recruitment
CREATE TABLE Recruitment (
    PersonId uniqueidentifier  NOT NULL,
    BranchOfferId uniqueidentifier  NOT NULL,
    Id uniqueidentifier  NOT NULL,
    Created datetime  NOT NULL,
    CvUrl nvarchar(100)  NULL,
    PersonMessage nvarchar(max)  NULL,
    CompanyResponse nvarchar(max)  NULL,
    IsAccepted char(1)  NULL,
    CONSTRAINT Recruitment_pk PRIMARY KEY  (Id)
);

-- Table: Street
CREATE TABLE Street (
    Id int  NOT NULL,
    Name nvarchar(100)  NOT NULL,
    AdministrativeTypeId int  NULL,
    CONSTRAINT Street_pk PRIMARY KEY  (Id)
);

-- Table: Url
CREATE TABLE Url (
    UserId uniqueidentifier  NOT NULL,
    UrlTypeId int  NOT NULL,
    Created datetime  NOT NULL,
    Path nvarchar(800)  NOT NULL,
    Name nvarchar(100)  NULL,
    Description nvarchar(max)  NULL,
    CONSTRAINT Url_pk PRIMARY KEY  (Created,UrlTypeId,UserId)
);

-- Table: UrlType
CREATE TABLE UrlType (
    Id int  NOT NULL,
    Name nvarchar(100)  NOT NULL,
    Description nvarchar(200)  NOT NULL,
    CONSTRAINT UrlType_pk PRIMARY KEY  (Id)
);

-- Table: User
CREATE TABLE "User" (
    Id uniqueidentifier  NOT NULL,
    Login nvarchar(100)  NULL,
    Salt nvarchar(max)  NOT NULL,
    Password nvarchar(max)  NOT NULL,
    CreatedProfileUrlSegment nvarchar(max)  NULL,
    LastPasswordUpdate datetime  NOT NULL,
    LastLoginIn datetime  NULL,
    RefreshToken nvarchar(max)  NULL,
    ExpiredToken datetime  NULL,
    ResetPasswordInitiated datetime  NULL,
    ResetPasswordUrlSegment nvarchar(max)  NULL,
    IsHideProfile char(1)  NOT NULL,
    CONSTRAINT User_pk PRIMARY KEY  (Id)
);

-- foreign keys
-- Reference: Address_Division (table: Address)
ALTER TABLE Address ADD CONSTRAINT Address_Division
    FOREIGN KEY (DivisionId)
    REFERENCES AdministrativeDivision (Id);

-- Reference: Address_Street (table: Address)
ALTER TABLE Address ADD CONSTRAINT Address_Street
    FOREIGN KEY (StreetId)
    REFERENCES Street (Id);

-- Reference: AdministrativeDivision_AdministrativeType (table: AdministrativeDivision)
ALTER TABLE AdministrativeDivision ADD CONSTRAINT AdministrativeDivision_AdministrativeType
    FOREIGN KEY (AdministrativeTypeId)
    REFERENCES AdministrativeType (Id);

-- Reference: BranchCharacteristicsList_Branch (table: BranchCharacteristicsList)
ALTER TABLE BranchCharacteristicsList ADD CONSTRAINT BranchCharacteristicsList_Branch
    FOREIGN KEY (BranchId)
    REFERENCES Branch (Id);

-- Reference: BranchCharacteristicsList_Characteristic (table: BranchCharacteristicsList)
ALTER TABLE BranchCharacteristicsList ADD CONSTRAINT BranchCharacteristicsList_Characteristic
    FOREIGN KEY (CharacteristicId)
    REFERENCES Characteristic (Id);

-- Reference: BranchCharacteristicsList_Quality (table: BranchCharacteristicsList)
ALTER TABLE BranchCharacteristicsList ADD CONSTRAINT BranchCharacteristicsList_Quality
    FOREIGN KEY (QualityId)
    REFERENCES Quality (Id);

-- Reference: BranchOffer_Branch (table: BranchOffer)
ALTER TABLE BranchOffer ADD CONSTRAINT BranchOffer_Branch
    FOREIGN KEY (BranchId)
    REFERENCES Branch (Id);

-- Reference: BranchOffer_Offer (table: BranchOffer)
ALTER TABLE BranchOffer ADD CONSTRAINT BranchOffer_Offer
    FOREIGN KEY (OfferId)
    REFERENCES Offer (Id);

-- Reference: Branch_Address (table: Branch)
ALTER TABLE Branch ADD CONSTRAINT Branch_Address
    FOREIGN KEY (AddressId)
    REFERENCES Address (Id);

-- Reference: Branch_Company (table: Branch)
ALTER TABLE Branch ADD CONSTRAINT Branch_Company
    FOREIGN KEY (CompanyId)
    REFERENCES Company (UserId);

-- Reference: CharacteristicColocation_Characteristic (table: CharacteristicColocation)
ALTER TABLE CharacteristicColocation ADD CONSTRAINT CharacteristicColocation_Characteristic
    FOREIGN KEY (ParentCharacteristicId)
    REFERENCES Characteristic (Id);

-- Reference: CharacteristicColocation_CharacteristicP (table: CharacteristicColocation)
ALTER TABLE CharacteristicColocation ADD CONSTRAINT CharacteristicColocation_CharacteristicP
    FOREIGN KEY (ChildCharacteristicId)
    REFERENCES Characteristic (Id);

-- Reference: Characteristic_CharacteristicType (table: Characteristic)
ALTER TABLE Characteristic ADD CONSTRAINT Characteristic_CharacteristicType
    FOREIGN KEY (CharacteristicTypeId)
    REFERENCES CharacteristicType (Id);

-- Reference: Comment_CommentType (table: Comment)
ALTER TABLE Comment ADD CONSTRAINT Comment_CommentType
    FOREIGN KEY (CommentTypeId)
    REFERENCES CommentType (Id);

-- Reference: Comment_Internship (table: Comment)
ALTER TABLE Comment ADD CONSTRAINT Comment_Internship
    FOREIGN KEY (InternshipId)
    REFERENCES Internship (Id);

-- Reference: Company_User (table: Company)
ALTER TABLE Company ADD CONSTRAINT Company_User
    FOREIGN KEY (UserId)
    REFERENCES "User" (Id);

-- Reference: DivisionStreet_AdministrativeDivision (table: DivisionStreet)
ALTER TABLE DivisionStreet ADD CONSTRAINT DivisionStreet_AdministrativeDivision
    FOREIGN KEY (DivisionId)
    REFERENCES AdministrativeDivision (Id);

-- Reference: DivisionStreet_Street (table: DivisionStreet)
ALTER TABLE DivisionStreet ADD CONSTRAINT DivisionStreet_Street
    FOREIGN KEY (StreetId)
    REFERENCES Street (Id);

-- Reference: Division_Division (table: AdministrativeDivision)
ALTER TABLE AdministrativeDivision ADD CONSTRAINT Division_Division
    FOREIGN KEY (ParentDivisionId)
    REFERENCES AdministrativeDivision (Id);

-- Reference: Internship_Recruitment (table: Internship)
ALTER TABLE Internship ADD CONSTRAINT Internship_Recruitment
    FOREIGN KEY (Id)
    REFERENCES Recruitment (Id);

-- Reference: Notification_NotificationSender (table: Notification)
ALTER TABLE Notification ADD CONSTRAINT Notification_NotificationSender
    FOREIGN KEY (NotificationSenderId)
    REFERENCES NotificationSender (Id);

-- Reference: Notification_NotificationStatus (table: Notification)
ALTER TABLE Notification ADD CONSTRAINT Notification_NotificationStatus
    FOREIGN KEY (NotificationStatusId)
    REFERENCES NotificationStatus (Id);

-- Reference: Notification_User (table: Notification)
ALTER TABLE Notification ADD CONSTRAINT Notification_User
    FOREIGN KEY (UserId)
    REFERENCES "User" (Id);

-- Reference: OfferCharacteristicsList_Characteristic (table: OfferCharacteristicsList)
ALTER TABLE OfferCharacteristicsList ADD CONSTRAINT OfferCharacteristicsList_Characteristic
    FOREIGN KEY (CharacteristicId)
    REFERENCES Characteristic (Id);

-- Reference: OfferCharacteristicsList_Offer (table: OfferCharacteristicsList)
ALTER TABLE OfferCharacteristicsList ADD CONSTRAINT OfferCharacteristicsList_Offer
    FOREIGN KEY (OfferId)
    REFERENCES Offer (Id);

-- Reference: OfferCharacteristicsList_Quality (table: OfferCharacteristicsList)
ALTER TABLE OfferCharacteristicsList ADD CONSTRAINT OfferCharacteristicsList_Quality
    FOREIGN KEY (QualityId)
    REFERENCES Quality (Id);

-- Reference: PersonCharacteristicsList_Characteristic (table: PersonCharacteristicsList)
ALTER TABLE PersonCharacteristicsList ADD CONSTRAINT PersonCharacteristicsList_Characteristic
    FOREIGN KEY (CharacteristicId)
    REFERENCES Characteristic (Id);

-- Reference: PersonCharacteristicsList_Person (table: PersonCharacteristicsList)
ALTER TABLE PersonCharacteristicsList ADD CONSTRAINT PersonCharacteristicsList_Person
    FOREIGN KEY (PersonId)
    REFERENCES Person (UserId);

-- Reference: PersonCharacteristicsList_Quality (table: PersonCharacteristicsList)
ALTER TABLE PersonCharacteristicsList ADD CONSTRAINT PersonCharacteristicsList_Quality
    FOREIGN KEY (QualityId)
    REFERENCES Quality (Id);

-- Reference: Person_Address (table: Person)
ALTER TABLE Person ADD CONSTRAINT Person_Address
    FOREIGN KEY (AddressId)
    REFERENCES Address (Id);

-- Reference: Person_User (table: Person)
ALTER TABLE Person ADD CONSTRAINT Person_User
    FOREIGN KEY (UserId)
    REFERENCES "User" (Id);

-- Reference: Quality_CharacteristicType (table: Quality)
ALTER TABLE Quality ADD CONSTRAINT Quality_CharacteristicType
    FOREIGN KEY (CharacteristicTypeId)
    REFERENCES CharacteristicType (Id);

-- Reference: Recruitment_BranchOffer (table: Recruitment)
ALTER TABLE Recruitment ADD CONSTRAINT Recruitment_BranchOffer
    FOREIGN KEY (BranchOfferId)
    REFERENCES BranchOffer (Id);

-- Reference: Recruitment_Person (table: Recruitment)
ALTER TABLE Recruitment ADD CONSTRAINT Recruitment_Person
    FOREIGN KEY (PersonId)
    REFERENCES Person (UserId);

-- Reference: Street_AdministrativeType (table: Street)
ALTER TABLE Street ADD CONSTRAINT Street_AdministrativeType
    FOREIGN KEY (AdministrativeTypeId)
    REFERENCES AdministrativeType (Id);

-- Reference: Url_UrlType (table: Url)
ALTER TABLE Url ADD CONSTRAINT Url_UrlType
    FOREIGN KEY (UrlTypeId)
    REFERENCES UrlType (Id);

-- Reference: Url_User (table: Url)
ALTER TABLE Url ADD CONSTRAINT Url_User
    FOREIGN KEY (UserId)
    REFERENCES "User" (Id);

-- End of file.

