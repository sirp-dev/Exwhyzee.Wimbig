-- Get All Raffles

CREATE PROCEDURE [dbo].[spTotalRaffle]
@status int = null

AS
BEGIN

	DECLARE @TotalCount INT = 0

	SELECT COUNT(*) FROM [Raffle] WHERE [Status] = @status

	RETURN @TotalCount

END
