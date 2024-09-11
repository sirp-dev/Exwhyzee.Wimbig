CREATE PROCEDURE [dbo].[spFetchNotificationsToBeSent]
	@messageStatus int,
	@messageChannel int,
	@retries int
AS
BEGIN
	SELECT Top(10) * FROM [dbo].[MessageStore] WHERE [MessageStatus] = @messageStatus AND [MessageChannel] = @messageChannel AND [Retries] <= @retries  order by [Retries] asc
END
