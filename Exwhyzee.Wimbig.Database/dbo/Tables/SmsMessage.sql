/*
The database must have a MEMORY_OPTIMIZED_DATA filegroup
before the memory optimized object can be created.

The bucket count should be set to about two times the 
maximum expected number of distinct values in the 
index key, rounded up to the nearest power of two.
*/

CREATE TABLE [dbo].[SmsMessage]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1) ,
	[SenderId] nvarchar(250) null,
	[DateCreated] datetime not null,
	[Recipient] nvarchar(MAX) null,
	[Message] nvarchar(MAX) null,
    [Status] INT NULL, 
    [Response] NVARCHAR(MAX) NULL

)
