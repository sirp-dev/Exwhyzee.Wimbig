-- Update Category entity

CREATE PROCEDURE [dbo].[spSmsMessageUpdate]
	@id bigint,
	@senderId nvarchar(250),	
	@dateCreated datetime,
	@recipient nvarchar(MAX),
	@message nvarchar(MAX),
		@status int,
		@response nvarchar(MAX)
AS
BEGIN
	UPDATE [dbo].[SmsMessage] SET
	
	[SenderId] = @senderId,
	[DateCreated] = @dateCreated,
	[Recipient] = @recipient,
	[Message] = @message,
	[Status] = @status,
	[Response] = @response

	WHERE [Id] = @id
END