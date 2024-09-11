------ INsert Mapping For Raffle Into A Category

CREATE PROCEDURE [dbo].[spMapRaffleToCategoryInsert]
	@raffleId bigint,
	@raffleName nvarchar(200),
	@categoryId int,
	@categoryName nvarchar(200),
	@dateCreated datetime

	AS
	BEGIN

	INSERT INTO [dbo].[MapRaffleToCategory] ([RaffleId],[RaffleName],[CategoryId],[CategoryName],[DateCreated])
	output inserted.Id
	VALUES (@raffleId, @raffleName,@categoryId,@categoryName,@dateCreated)
END
