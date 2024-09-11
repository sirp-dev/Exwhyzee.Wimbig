-- Delete a category by its Id

CREATE PROCEDURE [dbo].[spPayOutDetailsDelete]
	@Id int = 0
AS
BEGIN
	DELETE [dbo].[PayOutDetails] WHERE 
	[Id] = @Id
END
