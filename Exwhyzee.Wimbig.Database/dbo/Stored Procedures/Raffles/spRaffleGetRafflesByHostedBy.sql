-- Get all Raffles by Hosted By

CREATE PROCEDURE [dbo].[spRaffleGetRafflesByHostedBy]
	@hostedBy nvarchar(200) = null,
	@status int = null,
	@startIndex int = 0,
	@count int= 2147483647
AS
BEGIN
	SELECT * FROM [dbo].[Raffle] wHERE [HostedBy] = @hostedBy AND (@status IS NULL OR [Status] = @status)
	
	ORDER BY [Id] DESC
	OFFSET @startIndex ROWS
	FETCH NEXT @count ROWS ONLY
END
