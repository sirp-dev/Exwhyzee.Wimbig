-- Delete SEction Entity

CREATE PROCEDURE [dbo].[spSectionDelete]
	@id int = 0

AS 
BEGIN

DELETE FROM [Section] WHERE [SectionId]= @id
 
END
