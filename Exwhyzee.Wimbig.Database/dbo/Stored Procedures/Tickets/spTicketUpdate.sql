CREATE PROCEDURE [dbo].[spTicketUpdate]
	@id bigint
	
AS
BEGIN

	UPDATE[dbo].[Ticket] SET [IsWinner] = 1, [DateWon] = getDate(), [TicketStatus] = 2
Where Id = @id
End