CREATE PROCEDURE [dbo].[spRaffleSendArchieved]

@id bigint
	
AS

BEGIN
	
	UPDATE[dbo].[Raffle] set [Archived] = 1 Where Id = @id
	
End


