-- Update Map

CREATE PROCEDURE [dbo].[spMapImageToRaffleUpdate]
	@id bigint ,
	@imageId bigint ,
	@raffleId bigint,
	@dateCreated datetime
AS
BEGIN
	Update [dbo].[MapImageToRaffle] Set
	[ImageId]=@imageId,
	[RaffleId]= @raffleId,
	[DateCreated]= @dateCreated

	Where [Id]= @id
END
