-- Get the first entity where CategooryId is eual provided id

CREATE PROCEDURE [dbo].[spPayOutReportGetByUserIdAndDate]
	@userId nvarchar(250) = null,
	@startDate datetime = null,
	@endDate datetime = null

AS
BEGIN
	SELECT Top(1) * From [dbo].[PayOutReport] WHERE [UserId] = @userId AND [EndDate] = @endDate AND [StartDate] = @startDate
END
