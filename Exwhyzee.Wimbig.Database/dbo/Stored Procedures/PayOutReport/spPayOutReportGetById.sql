-- Get the first entity where CategooryId is eual provided id

CREATE PROCEDURE [dbo].[spPayOutReportGetById]
	@id bigint = 0 
AS
BEGIN
	SELECT Top(1) * From [dbo].[PayOutReport] WHERE [Id] = @id
END
