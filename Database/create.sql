-- Created by Vertabelo (http://vertabelo.com)
-- Last modification date: 2024-09-12 09:08:59.937

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
    CountryId int  NOT NULL,
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
    Name nvarchar(100)  NOT NULL,
    Description ntext  NULL,
    AddressId uniqueidentifier  NOT NULL,
    CONSTRAINT Branch_pk PRIMARY KEY  (Id)
);

-- Table: BranchCharacteristicsList
CREATE TABLE BranchCharacteristicsList (
    BranchId uniqueidentifier  NOT NULL,
    QualityId int  NOT NULL,
    CharacteristicId int  NOT NULL,
    CONSTRAINT BranchCharacteristicsList_pk PRIMARY KEY  (QualityId,CharacteristicId,BranchId)
);

-- Table: BranchOffer
CREATE TABLE BranchOffer (
    BranchId uniqueidentifier  NOT NULL,
    OfferId uniqueidentifier  NOT NULL,
    CONSTRAINT BranchOffer_pk PRIMARY KEY  (BranchId,OfferId)
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
    ReleaseDate datetime  NOT NULL,
    Description ntext  NOT NULL,
    CONSTRAINT Comment_pk PRIMARY KEY  (CommentTypeId,InternshipId)
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
    Name nvarchar(100)  NOT NULL,
    CONSTRAINT Company_pk PRIMARY KEY  (UserId)
);

-- Table: Country
CREATE TABLE Country (
    Id int  NOT NULL,
    Name nvarchar(100)  NOT NULL,
    CONSTRAINT Country_pk PRIMARY KEY  (Id)
);

-- Table: DivisionStreet
CREATE TABLE DivisionStreet (
    DivisionId int  NOT NULL,
    StreetId int  NOT NULL,
    CONSTRAINT DivisionStreet_pk PRIMARY KEY  (DivisionId,StreetId)
);

-- Table: Internship
CREATE TABLE Internship (
    Id uniqueidentifier  NOT NULL,
    OfferId uniqueidentifier  NOT NULL,
    PersonId uniqueidentifier  NOT NULL,
    ContractNumber nvarchar(100)  NOT NULL,
    CONSTRAINT Internship_pk PRIMARY KEY  (Id)
);

-- Table: Offer
CREATE TABLE Offer (
    Id uniqueidentifier  NOT NULL,
    Name varchar(100)  NOT NULL,
    Description ntext  NOT NULL,
    WorkStart  date  NULL,
    WorkEnd  date  NULL,
    PublishStart  datetime  NOT NULL,
    PublishEnd  datetime  NULL,
    Paid  char(1)  NOT NULL,
    NegotiatedSalary char(1)  NULL,
    MinSalary money  NULL,
    MaxSalary money  NULL,
    RemoteWork char(1)  NOT NULL,
    LastUpdate  datetime  NOT NULL,
    PrivateStatus char(1)  NOT NULL,
    CONSTRAINT Offer_pk PRIMARY KEY  (Id)
);

-- Table: OfferCharacteristicsList
CREATE TABLE OfferCharacteristicsList (
    OfferId uniqueidentifier  NOT NULL,
    QualityId int  NOT NULL,
    CharacteristicId int  NOT NULL,
    CONSTRAINT OfferCharacteristicsList_pk PRIMARY KEY  (CharacteristicId,QualityId,OfferId)
);

-- Table: Person
CREATE TABLE Person (
    UserId uniqueidentifier  NOT NULL,
    Name nvarchar(100)  NOT NULL,
    Surname nvarchar(100)  NOT NULL,
    CONSTRAINT Person_pk PRIMARY KEY  (UserId)
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
    OfferId uniqueidentifier  NOT NULL,
    PersonId uniqueidentifier  NOT NULL,
    ApplicationDate datetime  NOT NULL,
    AcceptedRejected char(1)  NOT NULL,
    Comment ntext  NULL,
    CONSTRAINT Recruitment_pk PRIMARY KEY  (OfferId,PersonId)
);

-- Table: Street
CREATE TABLE Street (
    Id int  NOT NULL,
    Name nvarchar(100)  NOT NULL,
    AdministrativeTypeId int  NULL,
    CONSTRAINT Street_pk PRIMARY KEY  (Id)
);

-- Table: TypeURL
CREATE TABLE TypeURL (
    Id int  NOT NULL,
    Name nvarchar(100)  NOT NULL,
    Description ntext  NOT NULL,
    CONSTRAINT TypeURL_pk PRIMARY KEY  (Id)
);

-- Table: URLs
CREATE TABLE URLs (
    UserId uniqueidentifier  NOT NULL,
    TypeURLId int  NOT NULL,
    PublishDate datetime  NOT NULL,
    Description ntext  NOT NULL,
    CONSTRAINT URLs_pk PRIMARY KEY  (TypeURLId,PublishDate,UserId)
);

-- Table: User
CREATE TABLE "User" (
    Id uniqueidentifier  NOT NULL,
    Email nvarchar(100)  NOT NULL,
    Logo image  NULL,
    CeateDate datetime  NOT NULL,
    Description ntext  NULL,
    Password nvarchar(max)  NOT NULL,
    Salt nvarchar(max)  NOT NULL,
    RefreshToken nvarchar(max)  NOT NULL,
    ExpiredToken datetime  NOT NULL,
    CONSTRAINT User_pk PRIMARY KEY  (Id)
);

-- Table: UserCharacteristicsList
CREATE TABLE UserCharacteristicsList (
    UserId uniqueidentifier  NOT NULL,
    QualityId int  NOT NULL,
    CharacteristicId int  NOT NULL,
    CONSTRAINT UserCharacteristicsList_pk PRIMARY KEY  (QualityId,CharacteristicId,UserId)
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

-- Reference: AdministrativeDivision_Country (table: AdministrativeDivision)
ALTER TABLE AdministrativeDivision ADD CONSTRAINT AdministrativeDivision_Country
    FOREIGN KEY (CountryId)
    REFERENCES Country (Id);

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
    FOREIGN KEY (OfferId,PersonId)
    REFERENCES Recruitment (OfferId,PersonId);

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

-- Reference: Person_User (table: Person)
ALTER TABLE Person ADD CONSTRAINT Person_User
    FOREIGN KEY (UserId)
    REFERENCES "User" (Id);

-- Reference: Quality_CharacteristicType (table: Quality)
ALTER TABLE Quality ADD CONSTRAINT Quality_CharacteristicType
    FOREIGN KEY (CharacteristicTypeId)
    REFERENCES CharacteristicType (Id);

-- Reference: Recruitment_Offer (table: Recruitment)
ALTER TABLE Recruitment ADD CONSTRAINT Recruitment_Offer
    FOREIGN KEY (OfferId)
    REFERENCES Offer (Id);

-- Reference: Recruitment_Person (table: Recruitment)
ALTER TABLE Recruitment ADD CONSTRAINT Recruitment_Person
    FOREIGN KEY (PersonId)
    REFERENCES Person (UserId);

-- Reference: Street_AdministrativeType (table: Street)
ALTER TABLE Street ADD CONSTRAINT Street_AdministrativeType
    FOREIGN KEY (AdministrativeTypeId)
    REFERENCES AdministrativeType (Id);

-- Reference: URLs_TypeURL (table: URLs)
ALTER TABLE URLs ADD CONSTRAINT URLs_TypeURL
    FOREIGN KEY (TypeURLId)
    REFERENCES TypeURL (Id);

-- Reference: URLs_User (table: URLs)
ALTER TABLE URLs ADD CONSTRAINT URLs_User
    FOREIGN KEY (UserId)
    REFERENCES "User" (Id);

-- Reference: UserCharacteristicsList_Characteristic (table: UserCharacteristicsList)
ALTER TABLE UserCharacteristicsList ADD CONSTRAINT UserCharacteristicsList_Characteristic
    FOREIGN KEY (CharacteristicId)
    REFERENCES Characteristic (Id);

-- Reference: UserCharacteristicsList_Quality (table: UserCharacteristicsList)
ALTER TABLE UserCharacteristicsList ADD CONSTRAINT UserCharacteristicsList_Quality
    FOREIGN KEY (QualityId)
    REFERENCES Quality (Id);

-- Reference: UserCharacteristicsList_User (table: UserCharacteristicsList)
ALTER TABLE UserCharacteristicsList ADD CONSTRAINT UserCharacteristicsList_User
    FOREIGN KEY (UserId)
    REFERENCES "User" (Id);

-- End of file.

