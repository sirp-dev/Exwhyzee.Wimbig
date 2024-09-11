CREATE PROCEDURE [dbo].[spRaffleGetByStatus]
@status int,
@count int
AS 
BEGIN

Select TOP (@count) * FROM [dbo].[Raffle]
WHERE ([Status] = @status)  AND ([NumberOfTickets] = [TotalSold])
END