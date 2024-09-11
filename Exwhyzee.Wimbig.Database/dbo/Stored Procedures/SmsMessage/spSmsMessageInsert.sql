--Create Category Entity

CREATE PROCEDURE [dbo].[spSmsMessageInsert]
		
	@senderId nvarchar(250),	
	@dateCreated datetime,
	@recipient nvarchar(MAX),
	@message nvarchar(MAX),
		@status int,
		@response nvarchar(MAX)

		
AS
BEGIN
	INSERT INTO [dbo].[SmsMessage] ([SenderId],[DateCreated],[Recipient],[Message],[Status],[Response])
	output inserted.Id

	VALUES (@senderId,@dateCreated,@recipient,@message,@status,@response)
END
