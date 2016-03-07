CREATE TABLE [dbo].[Customers] (
    [Id]                  UNIQUEIDENTIFIER DEFAULT (newsequentialid()) NOT NULL,
    [Name]                NVARCHAR (64)    NOT NULL,
    [AverageAnnualVolume] MONEY            NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);





