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

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Customers]') AND type in (N'U'))
	BEGIN

		INSERT [dbo].[Customers] ([CustomerId], [Name], [Surname], [Email], [Phone], [Age], [DateOfBirth], [CustomerStatusId]) VALUES (N'0ec1fd41-8acf-4cab-aeff-021f29209c97', N'Ivan', N'Ivanov', N'Email08fe7230-ddbc-40a6-8333-44f1fda616c2', N'1234567890', 25, CAST(N'2017-12-04 00:04:15.6178112' AS DateTime2), 2)

	END

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
	BEGIN

		INSERT [dbo].[Users] ([UserId], [Name], [Surname], [Email], [Phone], [Age], [DateOfBirth], [UserStatus]) VALUES (N'1a96f313-610a-442c-bdba-cfc3b3a23770', N'Stepan', N'Stepanov', N'Email11256e75-0b39-4864-8174-84810f336e00', N'0987654321', 30, CAST(N'2016-10-23 05:49:49.6770000' AS DateTime2), 2)

	END
GO