CREATE PROCEDURE [dbo].[spTransactionUpdate]
	@Id bigint,
	@walletId bigint,
	@userId nvarchar(100),
	@amount decimal,
	@transactionType int,
	@dateOfTransaction datetime,
	@transactionReference nvarchar(50),
	@sender nvarchar(100),
	@status int,
	@description nvarchar(Max)

AS
Begin
	  UPDATE[dbo].[Transactions]  SET 
	  [WalletdId]  = @walletId,
	  [UserId] = @userId,
	  [Amount] = @amount,
	  [TransactionType] = @transactionType,
	  [DateOfTransaction] = @dateOfTransaction,
	  [TransactionReference] = @transactionReference,
	  [Sender] = @sender,
	  [Status] = @status,
	  [Description] = @description
WHERE Id = @Id
End