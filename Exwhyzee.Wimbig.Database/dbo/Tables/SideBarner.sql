CREATE TABLE [dbo].[SideBarner]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[Url] nvarchar(300) not null,
	[DateCreated] datetime not null,
	[ImageExtension] nvarchar(10) null,
	[Status] int null,
	[IsDefault] bit null, 
    [TargetLocation] NVARCHAR(50) NULL
)
