CREATE TABLE [dbo].[Orders] (
    [Id]           INT              IDENTITY (1001, 1) NOT NULL,
    [CustomerId]   UNIQUEIDENTIFIER NOT NULL,
    [DateReceived] DATETIME         NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

