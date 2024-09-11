CREATE PROCEDURE [dbo].[spTransferWimbankGetById]
	@transferWimbankId bigint = 0
AS
Begin
	SELECT * from [dbo].[WimbankTransfer] Where [Id] = @transferWimbankId
	
End