-- Update Category entity

CREATE PROCEDURE [dbo].[spWinnerReportUpdate]
	
	@id BIGINT,
	@winnerName nvarchar(MAX),
	@winnerPhoneNumber nvarchar(50),
	@winnerEmail NVARCHAR(50),
	@winnerLocation NVARCHAR(MAX),
	@amountPlayed DECIMAL,
	@raffleName NVARCHAR(50),
	@raffleId BIGINT,
	@ticketNumber INT,
	@itemCost DECIMAL,
	@dateCreated datetime, 
	@dateWon datetime, 
	@dateDelivered datetime, 
	@deliveredBy NVARCHAR(MAX),
	@deliveredPhone NVARCHAR(50),
	@deliveryAddress NVARCHAR(50),
	@totalAmountPlayed DECIMAL,
		@userId NVARCHAR(50),
		@status int

AS
BEGIN
	UPDATE [dbo].[WinnerReport] SET
	
	[WinnerName] = @winnerName,
	 [WinnerPhoneNumber] = @winnerPhoneNumber,
	 [WinnerEmail] = @winnerEmail,
	 [WinnerLocation] = @winnerLocation,
	 [AmountPlayed] = @amountPlayed,
	 [RaffleName] = @raffleName,
	 [RaffleId] = @raffleId,
	 [TicketNumber] = @ticketNumber,
	 [ItemCost] = @itemCost,
	 [DateCreated] = @dateCreated, 
	 [DateWon] = @dateWon, 
	 [DateDelivered] = @dateDelivered, 
	 [DeliveredBy] = @deliveredBy,
	 [DeliveredPhone] = @deliveredPhone,
	 [DeliveryAddress] = @deliveryAddress,
	 [TotalAmountPlayed] = @totalAmountPlayed,
	 [UserId] = @userId,
	 [Status] = @status

	WHERE [Id] = @Id
END