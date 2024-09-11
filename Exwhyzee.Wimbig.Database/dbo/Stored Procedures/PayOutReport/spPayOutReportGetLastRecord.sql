-- Get the first entity where CategooryId is eual provided id

CREATE PROCEDURE [dbo].[spPayOutReportGetLastRecord]
	
	@startDate datetime = null,
	@endDate datetime = null
AS
BEGIN
	SELECT Top(1) * From [dbo].[PayOutReport]
	ORDER BY [StartDate] DESC
END
