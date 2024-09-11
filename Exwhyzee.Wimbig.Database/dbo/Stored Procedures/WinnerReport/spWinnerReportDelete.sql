-- Delete a category by its Id

CREATE PROCEDURE [dbo].[spWinnerReportDelete]
	@Id int = 0
AS
BEGIN
	DELETE [dbo].[WinnerReport] WHERE 
	[Id] = @Id
END
