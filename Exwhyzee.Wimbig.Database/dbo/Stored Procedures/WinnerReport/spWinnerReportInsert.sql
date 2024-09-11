--Create Category Entity

CREATE PROCEDURE [dbo].[spWinnerReportInsert]
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
	INSERT INTO [dbo].[WinnerReport] (
	[WinnerName],
	  [WinnerPhoneNumber],
	  [WinnerEmail],
	  [WinnerLocation],
	  [AmountPlayed],
	  [RaffleName],
	  [RaffleId],
	  [TicketNumber],
	  [ItemCost],
	  [DateCreated], 
	  [DateWon], 
	  [DateDelivered], 
	  [DeliveredBy],
	  [DeliveredPhone],
	  [DeliveryAddress],
	  [TotalAmountPlayed],
	  [UserId],
	  [Status]
	  )
	output inserted.Id

	VALUES (
	  @winnerName,
	  @winnerPhoneNumber,
	  @winnerEmail,
	  @winnerLocation,
	  @amountPlayed,
	  @raffleName,
	  @raffleId,
	  @ticketNumber,
	  @itemCost,
	  @dateCreated, 
	  @dateWon, 
	  @dateDelivered, 
	  @deliveredBy,
	  @deliveredPhone,
	  @deliveryAddress,
	  @totalAmountPlayed,
	  @userId,
	  @status
	  )
END
