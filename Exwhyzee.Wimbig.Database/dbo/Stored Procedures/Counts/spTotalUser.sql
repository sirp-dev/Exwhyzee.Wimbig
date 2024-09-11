-- Get All Raffles

CREATE PROCEDURE [dbo].[spTotalUser]

AS
BEGIN

	DECLARE @TotalCount INT = 0

	SELECT COUNT(*) FROM [AspNetUsers] 
	

	RETURN @TotalCount

END
