CREATE PROCEDURE [dbo].[spTransactionGetById]
	@transactionId bigint = 0
AS
Begin
	SELECT * from [dbo].[Transactions] Where [Id] = @transactionId
	
End