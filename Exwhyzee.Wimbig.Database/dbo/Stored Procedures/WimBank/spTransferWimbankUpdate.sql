CREATE PROCEDURE [dbo].[spTransferWimbankUpdate]
	@Id bigint,
	@amount decimal,
	@userId nvarchar(100),
	@receiverId nvarchar(100),
	@dateOfTransaction datetime,
	@transactionStatus int = 0,
	@note nvarchar(1000),
	@receiverPhone nvarchar(100)

AS
Begin
	  UPDATE[dbo].[WimbankTransfer]  SET 
	  [Amount] = @amount,
	  [ReceiverId] = @receiverId,
	  [UserId] = @userId,
	  [DateOfTransaction] = @dateOfTransaction,
	  [TransactionStatus] = @transactionStatus,
	  [ReceiverPhone] = @receiverPhone
WHERE Id = @Id
End