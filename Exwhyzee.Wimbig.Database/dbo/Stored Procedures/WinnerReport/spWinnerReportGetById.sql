-- Get the first entity where CategooryId is eual provided id

CREATE PROCEDURE [dbo].[spWinnerReportGetById]
	@id bigint = 0 
AS
BEGIN
	SELECT Top(1) * From [dbo].[WinnerReport] WHERE [Id] = @id
END
