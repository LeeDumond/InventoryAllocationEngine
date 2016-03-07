CREATE TABLE [dbo].[OrderItems] (
    [Id]              UNIQUEIDENTIFIER DEFAULT (newsequentialid()) NOT NULL,
    [OrderId]         INT              NOT NULL,
    [ProductId]       UNIQUEIDENTIFIER NOT NULL,
    [QuantityOrdered] INT              NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);



