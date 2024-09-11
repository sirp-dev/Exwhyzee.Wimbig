CREATE PROCEDURE [dbo].[spTransactionInsert]
	@walletId bigint,
	@userId nvarchar(100),
	@amount decimal,
	@transactionType int,
	@dateOfTransaction datetime,
	@status int,
	@sender nvarchar(100),
	@description nvarchar(500)

AS
Begin
	  INSERT INTO[dbo].[Transactions] ([WalletdId],[UserId],[Amount],[TransactionType],[DateOfTransaction], [Status], [Sender], [Description])
      output inserted.Id 
      VALUES (@walletId,@userId,@amount,@transactionType,@dateOfTransaction, @status, @sender, @description) 
End