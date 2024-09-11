CREATE PROCEDURE [dbo].[spWimbankLastBalance]
@wimbankId bigint = 0
AS
Begin
	SELECT top 1 * from [dbo].[Wimbank]
	ORDER BY [Wimbank].Id desc
	
End
