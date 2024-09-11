CREATE PROCEDURE [dbo].[spTicketUpdateStatus]
	@id bigint
	
AS
BEGIN

	UPDATE[dbo].[Ticket] SET [TicketStatus] = 2
Where Id = @id
End