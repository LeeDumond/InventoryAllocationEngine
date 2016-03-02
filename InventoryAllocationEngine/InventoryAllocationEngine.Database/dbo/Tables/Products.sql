CREATE TABLE [dbo].[Products] (
    [Id]   UNIQUEIDENTIFIER DEFAULT (newsequentialid()) NOT NULL,
    [Name] NCHAR (64)       NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

