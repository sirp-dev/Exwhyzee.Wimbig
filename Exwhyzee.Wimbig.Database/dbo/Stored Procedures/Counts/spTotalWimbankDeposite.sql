-- Get All Raffles

CREATE PROCEDURE [dbo].[spTotalWimbankDeposite]
@status int = null

AS
BEGIN

	DECLARE @TotalSum decimal = 0

	SELECT SUM(Amount) FROM [Wimbank] WHERE [TransactionStatus] = @status
	
	RETURN @TotalSum

END
