CREATE PROCEDURE [dbo].[spTransferWimbankInsert]
	@amount decimal,
	@userId nvarchar(100),
	@receiverId nvarchar(100),	
	@dateOfTransaction datetime,
	@transactionStatus int = 0,
	@note nvarchar(1000),
	@receiverPhone nvarchar(100)

AS
Begin
	  INSERT INTO[dbo].[WimbankTransfer] ([Amount],[UserId], [ReceiverId], [DateOfTransaction], [TransactionStatus], [Note], [ReceiverPhone])
      output inserted.Id 
      VALUES (@amount,@userId,@receiverId,@dateOfTransaction, @transactionStatus, @note, @receiverPhone) 
End