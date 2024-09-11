-- Delete a category by its Id

CREATE PROCEDURE [dbo].[spAgentDelete]
	@Id int = 0
AS
BEGIN
	DELETE [dbo].[Agent] WHERE 
	[Id] = @Id
END
