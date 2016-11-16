CREATE TABLE [dbo].[Customers]
(
	[CustomerId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Surname] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](150) NULL,
	[Phone] [nvarchar](50) NULL,
	[Age] [int] NULL,
	[DateOfBirth] [datetime2](7) NULL,
	[CustomerStatusId] [int] NULL, 
    CONSTRAINT [PK_Customers] PRIMARY KEY ([CustomerId]),
)
