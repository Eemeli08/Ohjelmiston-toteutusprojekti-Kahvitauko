CREATE TABLE [dbo].[Laite] (
    [Id]       INT            NOT NULL IDENTITY(1,1),
    [Nimi]     NVARCHAR(255)  NOT NULL,
    [Max_teho] INT            NOT NULL,
    CONSTRAINT [PK_Laite] PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Heat] (
    [Id]     INT           NOT NULL IDENTITY(1,1),
    [Tyyppi] NVARCHAR(50)  NOT NULL,
    CONSTRAINT [PK_Heat] PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Lisäsähkö] (
    [Id]        INT           NOT NULL IDENTITY(1,1),
    [Tyyppi]    NVARCHAR(50)  NOT NULL,
    [Tehokkuus] FLOAT         NOT NULL,
    CONSTRAINT [PK_Lisäsähkö] PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Car] (
    [Id]             INT            NOT NULL IDENTITY(1,1),
    [GoForwardStuff] NVARCHAR(100)  NULL,
    CONSTRAINT [PK_Car] PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[ElectricityContract] (
    [Id]           INT            NOT NULL IDENTITY(1,1),
    [Nimi]         NVARCHAR(100)  NOT NULL,
    [Kaytto_hinta] FLOAT          NOT NULL,
    [Siirto_hinta] FLOAT          NOT NULL,
    CONSTRAINT [PK_ElectricityContract] PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Ihminen] (
    [Id]     INT            NOT NULL IDENTITY(1,1),
    [Talous] NVARCHAR(100)  NULL,
    [Kaytto] FLOAT          NULL,
    CONSTRAINT [PK_Ihminen] PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Residency] (
    [Id]             INT            NOT NULL IDENTITY(1,1),
    [Osoite]         NVARCHAR(255)  NOT NULL,
    [Heating_method] INT            NULL,
    [Lisasahko]      INT            NULL,
    [Car]            INT            NULL,
    [Residency_guy]  INT            NULL,
    [Sahkosopimus]   INT            NULL,
    CONSTRAINT [PK_Residency]                     PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Residency_Heat]                FOREIGN KEY ([Heating_method]) REFERENCES [dbo].[Heat]([Id]),
    CONSTRAINT [FK_Residency_Lisäsähkö]           FOREIGN KEY ([Lisasahko])      REFERENCES [dbo].[Lisäsähkö]([Id]),
    CONSTRAINT [FK_Residency_Car]                 FOREIGN KEY ([Car])            REFERENCES [dbo].[Car]([Id]),
    CONSTRAINT [FK_Residency_Ihminen]             FOREIGN KEY ([Residency_guy])  REFERENCES [dbo].[Ihminen]([Id]),
    CONSTRAINT [FK_Residency_ElectricityContract] FOREIGN KEY ([Sahkosopimus])   REFERENCES [dbo].[ElectricityContract]([Id])
);

CREATE TABLE [dbo].[Laitekaytto] (
    [LaiteId]  INT    NOT NULL,
    [DateFrom] DATE   NOT NULL,
    [DateTo]   DATE   NULL,
    [TimeFrom] TIME   NOT NULL,
    [TimeTo]   TIME   NULL,
    [Power]    FLOAT  NOT NULL,
    CONSTRAINT [PK_Laitekaytto]       PRIMARY KEY CLUSTERED ([LaiteId], [DateFrom], [TimeFrom]),
    CONSTRAINT [FK_Laitekaytto_Laite] FOREIGN KEY ([LaiteId]) REFERENCES [dbo].[Laite]([Id])
);

CREATE TABLE [dbo].[Residency_Laite] (
    [ResidencyId] INT NOT NULL,
    [LaiteId]     INT NOT NULL,
    CONSTRAINT [PK_Residency_Laite]          PRIMARY KEY CLUSTERED ([ResidencyId], [LaiteId]),
    CONSTRAINT [FK_ResidencyLaite_Residency] FOREIGN KEY ([ResidencyId]) REFERENCES [dbo].[Residency]([Id]),
    CONSTRAINT [FK_ResidencyLaite_Laite]     FOREIGN KEY ([LaiteId])     REFERENCES [dbo].[Laite]([Id])
);