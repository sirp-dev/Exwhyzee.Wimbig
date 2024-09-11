--DELETE

CREATE PROCEDURE [dbo].[spRaffleDelete]
	@raffleId bigint = 0

AS
BEGIN
	Delete [dbo].[Raffle] Where [Id] = @raffleId
End
