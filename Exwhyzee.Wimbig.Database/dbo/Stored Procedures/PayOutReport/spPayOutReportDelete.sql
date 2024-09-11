-- Delete a category by its Id

CREATE PROCEDURE [dbo].[spPayOutReportDelete]
	@Id int = 0
AS
BEGIN
	DELETE [dbo].[PayOutReport] WHERE 
	[Id] = @Id
END
