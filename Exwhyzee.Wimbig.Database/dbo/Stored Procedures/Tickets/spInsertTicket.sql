CREATE PROCEDURE [dbo].[spTicketInsert]
	@userId nvarchar(250),
	@raffleId bigint,
	@ticketNumber nvarchar(100),
	@datePurchased datetime,
	@transactionId bigint,
	@price decimal(8,2),
	@myPhoneNumber varchar(13),
	@ticketStatus int,
	@email nvarchar(200),
	@playerName nvarchar(200),
	@currentLocation nvarchar(500)

AS
Begin
	  INSERT INTO[dbo].[Ticket] WITH (TABLOCKX) (
	  [UserId],[RaffleId],[TicketNumber],[DatePurchased],[TransactionId],[Price],[YourPhoneNumber], [TicketStatus], [Email], [PlayerName], [CurrentLocation])
                                 
		output inserted.Id 
        VALUES 
	(@userId, @raffleId, @ticketNumber,
	@datePurchased, @transactionId,@price,@myPhoneNumber, @ticketStatus, @email,@playerName,@currentLocation) 
End