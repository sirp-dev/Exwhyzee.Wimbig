CREATE PROCEDURE [dbo].[spWalletInsert]
	@userId nvarchar(100),
	@balance decimal,
	@dateUpdated datetime

AS
Begin
	  INSERT INTO[dbo].[Wallets] ([UserId],[Balance],[DateUpdated]) 
      output inserted.Id 
      VALUES (@userId, @balance,@dateUpdated) 
End
