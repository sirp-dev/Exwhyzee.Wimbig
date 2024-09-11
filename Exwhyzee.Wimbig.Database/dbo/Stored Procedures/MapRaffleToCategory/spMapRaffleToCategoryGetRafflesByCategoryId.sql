-- Mapped Raffles To Category

CREATE PROCEDURE [dbo].[spMapRaffleToCategoryGetRafflesByCategoryId]

	@categoryId bigint = 0
AS
BEGIN 

 SELECT map.[Id],map.[RaffleId], map.[CategoryId], map.[CategoryName], map.[DateCreated], raffle.[Id] as RaffleId, raffle.[Name] as RaffleName, raffle.[Description], raffle.[NumberOfTickets], raffle.[PricePerTicket],
 raffle.[HostedBy], raffle.[DeliveryType], raffle.[StartDate], raffle.[EndDate],raffle.[Status], raffle.[DateCreated] as RaffleDateCreated

 FROM [dbo].[MapRaffleToCategory] As map, [dbo].[Raffle] AS raffle WHERE 1=1 
	
	AND(map.RaffleId = raffle.Id AND map.CategoryId = @categoryId)
	AND ( GETUTCDATE() <= raffle.EndDate )

 END

