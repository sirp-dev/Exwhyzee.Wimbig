CREATE PROCEDURE [dbo].[spRaffleRemoveArchieved]
@id bigint
	
AS

BEGIN
	
	
	UPDATE[dbo].[Raffle] set [Archived] = 0 Where Id = @id
	
	
End