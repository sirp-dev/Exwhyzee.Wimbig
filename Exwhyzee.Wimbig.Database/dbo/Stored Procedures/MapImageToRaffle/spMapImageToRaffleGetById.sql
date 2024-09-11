--Get By Identifier

CREATE PROCEDURE [dbo].[spMapImageToRaffleGetById]
	@Id bigint
AS
BEGIN
	Select * From [dbo].[MapImageToRaffle] Where [Id]=@Id
END
