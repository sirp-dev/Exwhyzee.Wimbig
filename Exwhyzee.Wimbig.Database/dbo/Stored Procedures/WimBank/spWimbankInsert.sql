CREATE PROCEDURE [dbo].[spWimbankInsert]
	@amount decimal,
	@userId nvarchar(100),
	@balance decimal,
	@dateOfTransaction datetime,
	
	@note nvarchar(1000),
	@transactionStatus int = 0,
	@receiverId nvarchar(100)

AS
Begin
	  INSERT INTO[dbo].[Wimbank] ([Amount],[UserId], [Balance],[DateOfTransaction], [Note], [TransactionStatus], [ReceiverId])
      output inserted.Id 
      VALUES (@amount,@userId,@balance,@dateOfTransaction, @note, @transactionStatus, @receiverId) 
End