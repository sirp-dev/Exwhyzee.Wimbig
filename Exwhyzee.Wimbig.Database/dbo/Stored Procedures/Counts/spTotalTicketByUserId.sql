-- Get All Raffles

CREATE PROCEDURE [dbo].[spTotalTicketByUserId]
@userId nvarchar(200) = NULL

AS
BEGIN

	DECLARE @TotalCount INT = 0

	SELECT COUNT(*) FROM [Ticket] WHERE [UserId] = @userId

	RETURN @TotalCount

END
