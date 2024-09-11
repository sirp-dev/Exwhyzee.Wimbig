CREATE PROCEDURE [dbo].[spWalletUpdate]
	@Id bigint,
	@userId nvarchar(100),
	@balance decimal,
	@dateUpdated datetime

AS
Begin
	  UPDATE[dbo].[Wallets]  SET 
	  [UserId] = @userId,
	  [Balance] = @balance,
	  [DateUpdated] = @dateUpdated
WHERE Id = @Id
End