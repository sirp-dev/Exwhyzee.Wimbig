-- Get the first entity where CategooryId is eual provided id

CREATE PROCEDURE [dbo].[spPayOutReportGetLastRecordByUserId]
	@userId nvarchar(250),
	@startDate datetime = null,
	@endDate datetime = null
AS
BEGIN
	SELECT Top(1) * From [dbo].[PayOutReport] WHERE [UserId] = @userId
	ORDER BY [StartDate] DESC
END
