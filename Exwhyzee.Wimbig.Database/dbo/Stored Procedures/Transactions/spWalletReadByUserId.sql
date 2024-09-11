CREATE PROCEDURE [dbo].[spWalletReadByUserId]
	@userId nvarchar(100)
AS
Begin
	SELECT * from [dbo].[Wallets] Where [UserId] = @userId
	
End