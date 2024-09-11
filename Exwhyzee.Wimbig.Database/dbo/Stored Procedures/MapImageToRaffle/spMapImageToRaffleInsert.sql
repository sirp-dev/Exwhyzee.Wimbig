-- Insert Map of Image to a raffle

CREATE PROCEDURE [dbo].[spMapImageToRaffleInsert]
	
	@imageId bigint,
	@raffleId bigint,
	@dateCreated datetime

AS
BEGIN

	Insert into [dbo].[MapImageToRaffle] ([RaffleId],[ImageId],[DateCreated]) output inserted.Id
	Values (@raffleId,@imageId,@dateCreated)

End
