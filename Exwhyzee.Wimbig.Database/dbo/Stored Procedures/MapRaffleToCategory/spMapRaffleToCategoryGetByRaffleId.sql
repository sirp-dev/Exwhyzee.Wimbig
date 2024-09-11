-- Get By Id

CREATE PROCEDURE [dbo].[spMapRaffleToCategoryGetByRaffleId]
	@id bigint = 0
AS
BEGIN
	SELECT * FROm [dbo].[MapRaffleToCategory] WHERE RaffleId = @id
END
