CREATE TABLE [dbo].[Products] (
    [Id]                UNIQUEIDENTIFIER DEFAULT (newsequentialid()) NOT NULL,
    [Description]       NVARCHAR (64)    NOT NULL,
    [QuantityAvailable] INT              DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);





