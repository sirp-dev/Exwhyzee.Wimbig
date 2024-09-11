-- Get the first entity where CategooryId is eual provided id

CREATE PROCEDURE [dbo].[spTicketGetByRaffleIdTicketNumber]
	@id bigint = 0,
	@number nvarchar(100) = null
AS
BEGIN
	SELECT Top(1) 
	[Ticket].Id,
	[Ticket].DatePurchased, 
	[Ticket].UserId,
	[Ticket].RaffleId,
	[Ticket].TicketStatus AS TicketStatus,
	[Ticket].IsWinner,
	[Ticket].PlayerName,
	[Ticket].Email, 
	Ticket.[DatePurchased] AS Date, 
	Ticket.[TicketStatus] AS Status,
	Ticket.YourPhoneNumber,
	Ticket.IsSentToStat, 
	Ticket.CurrentLocation,
	Ticket.TicketNumber AS TicketNumber,
	Ticket.TransactionId,
	Ticket.Price,
	Ticket.DateWon AS DateWon, 
	Raffle.[Name] AS RaffleName
	
	From [dbo].[Ticket]
	LEFT JOIN [Raffle] ON [Ticket].RaffleId = [Raffle].Id
	WHERE [RaffleId] = @id AND [TicketNumber] = @number
	
END
