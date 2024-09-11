CREATE PROCEDURE [dbo].[spWimbankUpdate]
	@Id bigint,
	@amount decimal,
	@userId nvarchar(100),
	@balance decimal,
	@dateOfTransaction datetime,
	@transactionStatus int = 0,
	@note nvarchar(1000)

AS
Begin
	  UPDATE[dbo].[Wimbank]  SET 
	  [Amount] = @amount,
	  [Balance] = @balance,
	  [UserId] = @userId,
	  [DateOfTransaction] = @dateOfTransaction,
	  [TransactionStatus] = @transactionStatus
WHERE Id = @Id
End