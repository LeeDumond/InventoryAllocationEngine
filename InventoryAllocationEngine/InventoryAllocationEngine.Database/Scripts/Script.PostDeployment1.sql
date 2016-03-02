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

INSERT INTO [dbo].[Customers] ([Id], [Name]) VALUES (N'6da2468d-f1df-e511-9bf5-606dc7c69a50', N'Wal-Mart')
INSERT INTO [dbo].[Customers] ([Id], [Name]) VALUES (N'6ea2468d-f1df-e511-9bf5-606dc7c69a50', N'Kroger')
INSERT INTO [dbo].[Customers] ([Id], [Name]) VALUES (N'6fa2468d-f1df-e511-9bf5-606dc7c69a50', N'Shaw''s')
INSERT INTO [dbo].[Customers] ([Id], [Name]) VALUES (N'761f9194-f1df-e511-9bf5-606dc7c69a50', N'Target')
INSERT INTO [dbo].[Customers] ([Id], [Name]) VALUES (N'771f9194-f1df-e511-9bf5-606dc7c69a50', N'Shop & Save')
INSERT INTO [dbo].[Customers] ([Id], [Name]) VALUES (N'f23bfea2-f1df-e511-9bf5-606dc7c69a50', N'HyVee')


INSERT INTO [dbo].[OrderItems] ([Id], [OrderId], [ProductId], [Quantity]) VALUES (N'14e94141-0ee0-e511-9bf5-606dc7c69a50', 1001, N'cd233a97-f9df-e511-9bf5-606dc7c69a50', 250)


INSERT INTO [dbo].[Products] ([Id], [Name]) VALUES (N'cd233a97-f9df-e511-9bf5-606dc7c69a50', N'Chicken, broiler/fryer')


SET IDENTITY_INSERT [dbo].[Orders] ON
INSERT INTO [dbo].[Orders] ([Id], [CustomerId], [DateReceived]) VALUES (1001, N'6da2468d-f1df-e511-9bf5-606dc7c69a50', N'2016-03-01 00:00:00')
SET IDENTITY_INSERT [dbo].[Orders] OFF