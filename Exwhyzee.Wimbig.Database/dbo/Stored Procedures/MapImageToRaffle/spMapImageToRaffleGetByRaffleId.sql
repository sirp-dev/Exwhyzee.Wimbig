--Get By Identifier

CREATE PROCEDURE [dbo].[spMapImageToRaffleGetByRaffleId]
	@Id bigint
AS
BEGIN
	Select Top(1) * From [dbo].[MapImageToRaffle] Where [RaffleId]=@Id

END
