IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'Sähkötiedot')
BEGIN
    CREATE DATABASE [Sähkötiedot];
END
GO

USE [Sähkötiedot];
GO
-- 1. Database

-- 2. Taulut ilman ulkoisia avaimia
IF OBJECT_ID('[dbo].[Laite]', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Laite] (
        [Id]       INT            NOT NULL IDENTITY(1,1),
        [Nimi]     NVARCHAR(255)  NOT NULL,
        [Max_teho] INT            NOT NULL,
        CONSTRAINT [PK_Laite] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END

IF OBJECT_ID('[dbo].[Heat]', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Heat] (
        [Id]     INT           NOT NULL IDENTITY(1,1),
        [Tyyppi] NVARCHAR(50)  NOT NULL,
        CONSTRAINT [PK_Heat] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END

IF OBJECT_ID('[dbo].[Lisäsähkö]', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Lisäsähkö] (
        [Id]        INT           NOT NULL IDENTITY(1,1),
        [Tyyppi]    NVARCHAR(50)  NOT NULL,
        [Tehokkuus] FLOAT         NOT NULL,
        CONSTRAINT [PK_Lisäsähkö] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END

IF OBJECT_ID('[dbo].[Car]', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Car] (
        [Id]             INT            NOT NULL IDENTITY(1,1),
        [GoForwardStuff] NVARCHAR(100)  NULL,
        CONSTRAINT [PK_Car] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END

IF OBJECT_ID('[dbo].[ElectricityContract]', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[ElectricityContract] (
        [Id]           INT            NOT NULL IDENTITY(1,1),
        [Nimi]         NVARCHAR(100)  NOT NULL,
        [Kaytto_hinta] FLOAT          NOT NULL,
        [Siirto_hinta] FLOAT          NOT NULL,
        CONSTRAINT [PK_ElectricityContract] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END

IF OBJECT_ID('[dbo].[Ihminen]', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Ihminen] (
        [Id]     INT            NOT NULL IDENTITY(1,1),
        [Talous] NVARCHAR(100)  NULL,
        [Kaytto] FLOAT          NULL,
        CONSTRAINT [PK_Ihminen] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END

-- 3. Taulut, joilla on ulkoisia avaimia
IF OBJECT_ID('[dbo].[Residency]', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Residency] (
        [Id]             INT            NOT NULL IDENTITY(1,1),
        [Osoite]         NVARCHAR(255)  NOT NULL,
        [Heating_method] INT            NULL,
        [Lisasahko]      INT            NULL,
        [Car]            INT            NULL,
        [Residency_guy]  INT            NULL,
        [Sahkosopimus]   INT            NULL,
        CONSTRAINT [PK_Residency] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_Residency_Heat] FOREIGN KEY ([Heating_method]) REFERENCES [dbo].[Heat]([Id]),
        CONSTRAINT [FK_Residency_Lisäsähkö] FOREIGN KEY ([Lisasahko]) REFERENCES [dbo].[Lisäsähkö]([Id]),
        CONSTRAINT [FK_Residency_Car] FOREIGN KEY ([Car]) REFERENCES [dbo].[Car]([Id]),
        CONSTRAINT [FK_Residency_Ihminen] FOREIGN KEY ([Residency_guy]) REFERENCES [dbo].[Ihminen]([Id]),
        CONSTRAINT [FK_Residency_ElectricityContract] FOREIGN KEY ([Sahkosopimus]) REFERENCES [dbo].[ElectricityContract]([Id])
    );
END

IF OBJECT_ID('[dbo].[Laitekaytto]', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Laitekaytto] (
        [LaiteId]  INT    NOT NULL,
        [DateFrom] DATE   NOT NULL,
        [DateTo]   DATE   NULL,
        [TimeFrom] TIME   NOT NULL,
        [TimeTo]   TIME   NULL,
        [Power]    FLOAT  NOT NULL,
        CONSTRAINT [PK_Laitekaytto] PRIMARY KEY CLUSTERED ([LaiteId], [DateFrom], [TimeFrom]),
        CONSTRAINT [FK_Laitekaytto_Laite] FOREIGN KEY ([LaiteId]) REFERENCES [dbo].[Laite]([Id])
    );
END

IF OBJECT_ID('[dbo].[Residency_Laite]', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Residency_Laite] (
        [ResidencyId] INT NOT NULL,
        [LaiteId]     INT NOT NULL,
        CONSTRAINT [PK_Residency_Laite] PRIMARY KEY CLUSTERED ([ResidencyId], [LaiteId]),
        CONSTRAINT [FK_ResidencyLaite_Residency] FOREIGN KEY ([ResidencyId]) REFERENCES [dbo].[Residency]([Id]),
        CONSTRAINT [FK_ResidencyLaite_Laite] FOREIGN KEY ([LaiteId]) REFERENCES [dbo].[Laite]([Id])
    );
END

-- 4. Data- ja lokitaulut
IF OBJECT_ID('[dbo].[Sähkö_Data]', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Sähkö_Data] (
        [Id]         INT             NOT NULL IDENTITY(1,1),
        [Päivä_Aika] DATETIME        NOT NULL,
        [Hinta_kwh]  DECIMAL(10, 4)  NOT NULL,
        CONSTRAINT [PK_Sähkö_Data] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END

IF OBJECT_ID('[dbo].[Sää_data]', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Sää_data] (
        [Id]             INT IDENTITY(1,1) PRIMARY KEY,
        [Päivä_aika]     DATETIME NOT NULL,
        [Lämpö]          DECIMAL(5,2),
        [Aurinko]        DECIMAL(5,2),
        [Tuuli]          DECIMAL(5,2),
        [Aurinkopaneeli] DECIMAL(10, 2) NULL
    );
END
IF OBJECT_ID('[dbo].[Auto_Tyyppi]', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Auto_Tyyppi] (
        [Id]         INT             NOT NULL IDENTITY(1,1),
        [Tyyppi] NVARCHAR(255)        NOT NULL,
        [Akun_koko]  INT  NOT NULL,
        CONSTRAINT [PK_Auto_Tyyppi] PRIMARY KEY CLUSTERED ([Id] ASC)
        );
END
IF OBJECT_ID('[dbo].[Lämmitys]', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Lämmitys] (
        [Id]         INT             NOT NULL IDENTITY(1,1),
        [Lämmitystapa] NVARCHAR(255)        NOT NULL,
        [Eristystapa]  NVARCHAR(255)  NOT NULL,
        [Henkilömäärä] INT  NOT NULL,
        [Aurinkopaneelin_maxteho] INT  NOT NULL,
        [Aurinkopaneelin_as_kulma] INT  NOT NULL,
        [Akun_kapasiteetti] INT  NOT NULL,
        CONSTRAINT [PK_Lämmitys] PRIMARY KEY CLUSTERED ([Id] ASC)
        );
END
IF OBJECT_ID('[dbo].[Sähkösopimus]', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Sähkösopimus] (
        [Id]         INT             NOT NULL IDENTITY(1,1),
        [Siirtomaksu] DECIMAL(18,2)        NOT NULL,
        [Siirto]  DECIMAL(18,2)  NOT NULL,
        [Käyttömaksu] DECIMAL(18,2) NOT NULL,
        [Käyttö] DECIMAL(18,2) NOT NULL,
        CONSTRAINT [PK_Sähkösopimus] PRIMARY KEY CLUSTERED ([Id] ASC)
        );
END

--Tämä versio on tarkoitettu vain tietokannan luontiin sekä recoveryyn sen "If doesnt exist" -osioiden avulla. 
--Dataa ei ole vielä lisätty, mutta se on tarkoitus tehdä erikseen.