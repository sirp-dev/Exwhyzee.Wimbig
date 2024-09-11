-- Delete Maped Image To a Raffle

CREATE PROCEDURE [dbo].[spMapImageToRaffleDelete]
	@Id bigint 
AS
BEGIN
	Delete From [dbo].[MapImageToRaffle] Where [Id] = @Id;
	
END
