-- Get the first entity where CategooryId is eual provided id

CREATE PROCEDURE [dbo].[spTicketGetWinnerById]
	@id bigint = 0,
	@raffleId bigint = 0
AS
BEGIN
	SELECT Top(1) * From [dbo].[Ticket] WHERE [Id] = @id AND [RaffleId] = @raffleId
END

