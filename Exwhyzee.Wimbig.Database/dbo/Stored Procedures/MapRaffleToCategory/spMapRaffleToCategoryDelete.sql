-- Delete MapRaffleToCategory

CREATE PROCEDURE [dbo].[spMapRaffleToCategoryDelete]
	@id bigint = 0
AS
BEGIN

	DELETE [dbo].[MapImageToRaffle] WHERE [Id]=@id
END
