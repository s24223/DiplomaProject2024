-- Created by Vertabelo (http://vertabelo.com)
-- Last modification date: 2024-09-19 12:19:12.976

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
    Id uniqueidentifier  NOT NULL,
    CompanyId uniqueidentifier  NOT NULL,
    AddressId uniqueidentifier  NOT NULL,
    UrlSegment varchar(100)  NULL,
    Name nvarchar(100)  NOT NULL,
    Description ntext  NULL,
    CONSTRAINT Branch_pk PRIMARY KEY  (Id)
);

-- Table: BranchCharacteristicsList
CREATE TABLE BranchCharacteristicsList (
    CharacteristicId int  NOT NULL,
    BranchId uniqueidentifier  NOT NULL,
    QualityId int  NULL,
    CONSTRAINT BranchCharacteristicsList_pk PRIMARY KEY  (CharacteristicId,BranchId)
);

-- Table: BranchOffer
CREATE TABLE BranchOffer (
    BranchId uniqueidentifier  NOT NULL,
    OfferId uniqueidentifier  NOT NULL,
    Created datetime  NOT NULL,
    PublishStart datetime  NOT NULL,
    PublishEnd datetime  NULL,
    WorkStart date  NULL,
    WorkEnd date  NULL,
    LastUpdate datetime  NOT NULL,
    CONSTRAINT BranchOffer_pk PRIMARY KEY  (BranchId,OfferId,Created)
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
    Published datetime  NOT NULL,
    Description ntext  NOT NULL,
    Evaluation int  NULL,
    CONSTRAINT Comment_pk PRIMARY KEY  (CommentTypeId,InternshipId,Published)
);

-- Table: CommentType
CREATE TABLE CommentType (
    Id int  NOT NULL,
    Name nvarchar(100)  NOT NULL,
    CONSTRAINT CommentType_pk PRIMARY KEY  (Id)
);

-- Table: Company
CREATE TABLE Company (
    UserId uniqueidentifier  NOT NULL,
    Logo image  NULL,
    UrlSegment varchar(100)  NULL,
    CreateDate date  NOT NULL,
    ContactEmail nvarchar(100)  NOT NULL,
    Name nvarchar(100)  NOT NULL,
    Regon varchar(50)  NOT NULL,
    Description ntext  NULL,
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
    DateTime datetime  NOT NULL,
    ExceptionType varchar(100)  NOT NULL,
    Message ntext  NOT NULL,
    AdditionalData ntext  NULL,
    Status char(1)  NOT NULL,
    CONSTRAINT Exception_pk PRIMARY KEY  (Id)
);

-- Table: Internship
CREATE TABLE Internship (
    Id uniqueidentifier  NOT NULL,
    ContractNumber nvarchar(100)  NOT NULL,
    PersonId uniqueidentifier  NOT NULL,
    BranchId uniqueidentifier  NOT NULL,
    OfferId uniqueidentifier  NOT NULL,
    Created datetime  NOT NULL,
    CONSTRAINT Internship_pk PRIMARY KEY  (Id)
);

-- Table: Offer
CREATE TABLE Offer (
    Id uniqueidentifier  NOT NULL,
    Name nvarchar(100)  NOT NULL,
    Description ntext  NOT NULL,
    MinSalary money  NULL,
    MaxSalary money  NULL,
    NegotiatedSalary char(1)  NULL,
    ForStudents char(1)  NOT NULL,
    CONSTRAINT Offer_pk PRIMARY KEY  (Id)
);

-- Table: OfferCharacteristicsList
CREATE TABLE OfferCharacteristicsList (
    CharacteristicId int  NOT NULL,
    OfferId uniqueidentifier  NOT NULL,
    QualityId int  NULL,
    CONSTRAINT OfferCharacteristicsList_pk PRIMARY KEY  (CharacteristicId,OfferId)
);

-- Table: Person
CREATE TABLE Person (
    UserId uniqueidentifier  NOT NULL,
    Logo image  NULL,
    UrlSegment varchar(100)  NULL,
    CreateDate date  NOT NULL,
    ContactEmail nvarchar(100)  NOT NULL,
    Name nvarchar(100)  NOT NULL,
    Surname nvarchar(100)  NOT NULL,
    BirthDate date  NULL,
    ContactPhoneNum varchar(20)  NULL,
    Description ntext  NULL,
    IsStudent char(1)  NOT NULL,
    IsPublicProfile char(1)  NOT NULL,
    AddressId uniqueidentifier  NULL,
    CONSTRAINT Person_pk PRIMARY KEY  (UserId)
);

-- Table: PersonCharacteristicsList
CREATE TABLE PersonCharacteristicsList (
    CharacteristicId int  NOT NULL,
    PersonId uniqueidentifier  NOT NULL,
    QualityId int  NULL,
    CONSTRAINT PersonCharacteristicsList_pk PRIMARY KEY  (CharacteristicId,PersonId)
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
    BranchId uniqueidentifier  NOT NULL,
    OfferId uniqueidentifier  NOT NULL,
    Created datetime  NOT NULL,
    ApplicationDate datetime  NOT NULL,
    CV image  NULL,
    PersonMessage ntext  NULL,
    CompanyResponse ntext  NULL,
    AcceptedRejected char(1)  NULL,
    CONSTRAINT Recruitment_pk PRIMARY KEY  (PersonId,BranchId,OfferId,Created)
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
    PublishDate datetime  NOT NULL,
    Url ntext  NOT NULL,
    Name nvarchar(100)  NULL,
    Description ntext  NULL,
    CONSTRAINT Url_pk PRIMARY KEY  (PublishDate,UrlTypeId,UserId)
);

-- Table: UrlType
CREATE TABLE UrlType (
    Id int  NOT NULL,
    Name nvarchar(100)  NOT NULL,
    Description ntext  NOT NULL,
    CONSTRAINT UrlType_pk PRIMARY KEY  (Id)
);

-- Table: User
CREATE TABLE "User" (
    Id uniqueidentifier  NOT NULL,
    LoginEmail nvarchar(100)  NOT NULL,
    Password nvarchar(max)  NOT NULL,
    Salt nvarchar(max)  NOT NULL,
    RefreshToken nvarchar(max)  NULL,
    ExpiredToken datetime  NULL,
    LastLoginIn datetime  NULL,
    LastUpdatePassword datetime  NOT NULL,
    CONSTRAINT User_pk PRIMARY KEY  (Id)
);

-- Table: UserProblem
CREATE TABLE UserProblem (
    Id uniqueidentifier  NOT NULL,
    DateTime datetime  NOT NULL,
    UserMessage ntext  NOT NULL,
    Response ntext  NULL,
    PreviousProblemId uniqueidentifier  NULL,
    UserId uniqueidentifier  NULL,
    Email nvarchar(100)  NULL,
    Status char(1)  NOT NULL,
    CONSTRAINT UserProblem_pk PRIMARY KEY  (Id)
);

-- foreign keys
-- Reference: Address_Division (table: Address)
ALTER TABLE Address ADD CONSTRAINT Address_Division
    FOREIGN KEY (DivisionId)
    REFERENCES AdministrativeDivision (Id);

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

-- Reference: Copy_of_Address_Street (table: Address)
ALTER TABLE Address ADD CONSTRAINT Copy_of_Address_Street
    FOREIGN KEY (StreetId)
    REFERENCES Street (Id);

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
    FOREIGN KEY (PersonId,BranchId,OfferId,Created)
    REFERENCES Recruitment (PersonId,BranchId,OfferId,Created);

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
    FOREIGN KEY (BranchId,OfferId,Created)
    REFERENCES BranchOffer (BranchId,OfferId,Created);

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

-- Reference: UserProblem_User (table: UserProblem)
ALTER TABLE UserProblem ADD CONSTRAINT UserProblem_User
    FOREIGN KEY (UserId)
    REFERENCES "User" (Id);

-- End of file.

