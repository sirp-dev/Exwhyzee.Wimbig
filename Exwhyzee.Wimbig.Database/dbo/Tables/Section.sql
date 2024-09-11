CREATE TABLE [dbo].[Section]
(
	[SectionId] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[Name] nvarchar(250) not null,
	[Description] nvarchar(250) not null,
	[DateCreated] datetime not null, 
    [EntityStatus] INT NULL
)
