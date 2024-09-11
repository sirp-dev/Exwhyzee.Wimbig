CREATE PROCEDURE [dbo].[spTicketsReadAllWinners]


AS

	DECLARE @FilteredCount INT = 0
	DECLARE @TotalCount INT = 0

	SELECT [Ticket].Id AS Id, [Ticket].UserId,[Ticket].RaffleId,AspNetUsers.PhoneNumber, [Ticket].DatePurchased, [Ticket].IsWinner,[Ticket].PlayerName, [Ticket].Email, 
	Ticket.YourPhoneNumber,Ticket.IsSentToStat, Ticket.TicketStatus, Ticket.CurrentLocation,
	CONCAT(AspNetUsers.FirstName, + ' ' + AspNetUsers.LastName, + ' ' + AspNetUsers.OtherNames) AS FullName,Ticket.TicketNumber AS TicketNumber,Ticket.TransactionId,Ticket.Price,Ticket.DateWon AS DateWon, Raffle.[Name] AS RaffleName,
	[AspNetUsers].UserName AS Username FROM [Ticket]  
	LEFT JOIN [AspNetUsers] ON [Ticket].UserId = [AspNetUsers].Id
	LEFT JOIN [Raffle] ON [Ticket].RaffleId = [Raffle].Id
	

	WHERE [IsWinner] = 1
	RETURN