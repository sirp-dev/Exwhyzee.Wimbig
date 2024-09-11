-- Get the first entity where CategooryId is eual provided id

CREATE PROCEDURE [dbo].[spPayOutReportGetByDate]
	
	@startDate datetime = null,
	@endDate datetime = null

AS
BEGIN
	SELECT Top(1) * From [dbo].[PayOutReport] WHERE [EndDate] = @endDate AND [StartDate] = @startDate
END
