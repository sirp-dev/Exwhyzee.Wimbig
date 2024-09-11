CREATE PROCEDURE [dbo].[spMessageStoreInsert]
	@emailAddress nvarchar(50) = NULL,
	@phoneNumber nvarchar(15) = NULL,
	@channel int,
	@type int,
	@addressType int = 10,
	@message nvarchar(max),
	@imageUrl nvarchar(max)
AS
BEGIN
	BEGIN
	INSERT INTO [dbo].[MessageStore] ([EmailAddress], [PhoneNumber], [Message], [MessageType], [MessageChannel], [AddressType],[MessageStatus],[ImageUrl])
	OUTPUT inserted.Id
	VALUES (@emailAddress,@phoneNumber,@message,@type,@channel,@addressType,20,@imageUrl)
	END
END
