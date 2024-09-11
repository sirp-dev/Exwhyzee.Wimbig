CREATE TABLE [dbo].[YoutubeLinks]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[Url] nvarchar(MAX) not null,
	[Title] nvarchar(MAX) not null,
	[DateCreated] datetime not null
	
)
