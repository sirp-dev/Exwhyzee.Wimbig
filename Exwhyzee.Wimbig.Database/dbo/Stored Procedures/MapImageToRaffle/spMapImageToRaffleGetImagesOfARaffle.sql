--Get all images mapped to a raffle

CREATE PROCEDURE [dbo].[spMapImageToRaffleGetImagesOfARaffle]
	@raffleId bigint,
	@count int=1

AS
BEGIN

	if(@count = 1 or @count is null)
	Begin
	set @count = 1
	SELECT  Top(@count) map.[Id], map.[RaffleId], map.[ImageId], img.[Url], img.[ImageExtension] From   [dbo].[MapImageToRaffle] as map  inner join [dbo].[ImageFile] as img on map.[ImageId] = img.[Id]
			
			AND (map.[ImageId]=img.[Id])
			AND(map.[RaffleId] = @raffleId)
			And(img.[IsDefault]= 1)
	End

	Else
		Begin
			SELECT map.[Id], map.[RaffleId], map.[ImageId], img.[Url], img.[ImageExtension] From   [dbo].[MapImageToRaffle] as map, [dbo].[ImageFile] as img where 1 = 1
			
			AND (map.[ImageId]=img.[Id])
			AND(map.[RaffleId] = @raffleId)

			Order By map.[Id] ASC
			OffSet 0 Rows
			Fetch Next 5 Rows Only
		End

END