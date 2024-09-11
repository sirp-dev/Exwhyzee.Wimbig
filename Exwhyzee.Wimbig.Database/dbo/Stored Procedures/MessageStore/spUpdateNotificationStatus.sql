CREATE PROCEDURE [dbo].[spUpdateNotificationStatus]
	@messageStatus int,
	@dateSent datetime2 = NULL,
	@id bigint,
	@retries int,
	@response nvarchar(MAX)
AS
BEGIN
	UPDATE [dbo].[MessageStore] SET [MessageStatus] = @messageStatus, [DateSent] = @dateSent, [Retries] = @retries, [Response] = @response WHERE [Id] = @id
END
