CREATE PROCEDURE [dbo].[spTicketUpdateSentGameStat]
	@id bigint
	
AS
BEGIN

	UPDATE[dbo].[Ticket] SET [IsSentToStat] = 1 
	
Where Id = @id
End