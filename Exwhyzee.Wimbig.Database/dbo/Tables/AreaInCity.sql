﻿CREATE TABLE [dbo].[AreaInCity]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Name] NVARCHAR(MAX) NOT NULL, 
	[CityId] bigint not null
   
   
)
