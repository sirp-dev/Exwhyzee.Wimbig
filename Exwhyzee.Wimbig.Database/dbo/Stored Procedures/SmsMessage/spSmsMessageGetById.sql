﻿-- Get the first entity where CategooryId is eual provided id

CREATE PROCEDURE [dbo].[spSmsMessageGetById]
	@id bigint = 0 
AS
BEGIN
	SELECT Top(1) * From [dbo].[SmsMessage] WHERE [Id] = @id
END
