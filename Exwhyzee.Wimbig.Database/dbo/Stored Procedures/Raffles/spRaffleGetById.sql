-- Select a Raffle by Id

CREATE PROCEDURE [dbo].[spRaffleGetById]
	@raffleId bigint = 0
AS
Begin
	SELECT * from [dbo].[Raffle] Where [Id] = @raffleId
	
End
