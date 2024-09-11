-- Get By Id

CREATE PROCEDURE [dbo].[spMapRaffleToCategoryGetById]
	@id bigint = 0
AS
BEGIN
	SELECT * FROm [dbo].[MapRaffleToCategory] WHERE Id = @id
END
