-- Delete a category by its Id

CREATE PROCEDURE [dbo].[spSmsMessageDelete]
	@Id int = 0
AS
BEGIN
	DELETE [dbo].[SmsMessage] WHERE 
	[Id] = @Id
END
