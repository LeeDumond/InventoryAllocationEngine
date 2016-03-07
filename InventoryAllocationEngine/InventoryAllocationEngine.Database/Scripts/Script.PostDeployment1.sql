/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

INSERT INTO [dbo].[Customers] ([Id], [Name], [AverageAnnualVolume]) VALUES (N'6da2468d-f1df-e511-9bf5-606dc7c69a50', N'Wal-Mart', CAST(2350000.0000 AS Money))
INSERT INTO [dbo].[Customers] ([Id], [Name], [AverageAnnualVolume]) VALUES (N'6ea2468d-f1df-e511-9bf5-606dc7c69a50', N'Kroger', CAST(1725000.0000 AS Money))
INSERT INTO [dbo].[Customers] ([Id], [Name], [AverageAnnualVolume]) VALUES (N'6fa2468d-f1df-e511-9bf5-606dc7c69a50', N'Shaw''s', CAST(1010000.0000 AS Money))
INSERT INTO [dbo].[Customers] ([Id], [Name], [AverageAnnualVolume]) VALUES (N'761f9194-f1df-e511-9bf5-606dc7c69a50', N'Target', CAST(825000.0000 AS Money))
INSERT INTO [dbo].[Customers] ([Id], [Name], [AverageAnnualVolume]) VALUES (N'771f9194-f1df-e511-9bf5-606dc7c69a50', N'Shop & Save', CAST(532000.0000 AS Money))
INSERT INTO [dbo].[Customers] ([Id], [Name], [AverageAnnualVolume]) VALUES (N'f23bfea2-f1df-e511-9bf5-606dc7c69a50', N'HyVee', CAST(289000.0000 AS Money))


INSERT INTO [dbo].[OrderItems] ([Id], [OrderId], [ProductId], [QuantityOrdered]) VALUES (N'14e94141-0ee0-e511-9bf5-606dc7c69a50', 2001, N'cd233a97-f9df-e511-9bf5-606dc7c69a50', 250)
INSERT INTO [dbo].[OrderItems] ([Id], [OrderId], [ProductId], [QuantityOrdered]) VALUES (N'cd934408-f3e0-e511-9bf5-606dc7c69a50', 2002, N'cd233a97-f9df-e511-9bf5-606dc7c69a50', 425)
INSERT INTO [dbo].[OrderItems] ([Id], [OrderId], [ProductId], [QuantityOrdered]) VALUES (N'b3246317-f3e0-e511-9bf5-606dc7c69a50', 2003, N'cd233a97-f9df-e511-9bf5-606dc7c69a50', 375)
INSERT INTO [dbo].[OrderItems] ([Id], [OrderId], [ProductId], [QuantityOrdered]) VALUES (N'd7ce3023-f3e0-e511-9bf5-606dc7c69a50', 2004, N'cd233a97-f9df-e511-9bf5-606dc7c69a50', 300)


INSERT INTO [dbo].[Products] ([Id], [Description], [QuantityAvailable]) VALUES (N'cd233a97-f9df-e511-9bf5-606dc7c69a50', N'Chicken, broiler/fryer', 900)


SET IDENTITY_INSERT [dbo].[Orders] ON
INSERT INTO [dbo].[Orders] ([Id], [CustomerId], [DateReceived]) VALUES (2001, N'6da2468d-f1df-e511-9bf5-606dc7c69a50', N'2016-03-01 08:13:00')
INSERT INTO [dbo].[Orders] ([Id], [CustomerId], [DateReceived]) VALUES (2002, N'6ea2468d-f1df-e511-9bf5-606dc7c69a50', N'2016-03-02 11:48:00')
INSERT INTO [dbo].[Orders] ([Id], [CustomerId], [DateReceived]) VALUES (2003, N'6fa2468d-f1df-e511-9bf5-606dc7c69a50', N'2016-03-01 14:25:00')
INSERT INTO [dbo].[Orders] ([Id], [CustomerId], [DateReceived]) VALUES (2004, N'771f9194-f1df-e511-9bf5-606dc7c69a50', N'2016-02-29 15:32:00')
SET IDENTITY_INSERT [dbo].[Orders] OFF
