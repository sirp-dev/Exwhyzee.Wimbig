/*
The database must have a MEMORY_OPTIMIZED_DATA filegroup
before the memory optimized object can be created.

The bucket count should be set to about two times the 
maximum expected number of distinct values in the 
index key, rounded up to the nearest power of two.
*/

CREATE TABLE [dbo].[Category]
(
	[CategoryId] INT NOT NULL PRIMARY KEY IDENTITY(1,1) ,
	[Name] nvarchar(250) not null,
	[DateCreated] datetime not null,
	[Description] nvarchar(MAX) null,
	[SectionId] int not null, 
    [EntityStatus] INT NULL

)
