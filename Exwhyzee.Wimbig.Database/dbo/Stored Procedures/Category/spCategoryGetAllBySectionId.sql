-- Get Categories by SectionId

CREATE PROCEDURE [dbo].[spCategoryGetAllBySectionId]
	@sectionId int

AS
BEGIN
	SELECT * FROM [dbo].[Category] WHERE  [SectionId]= @sectionId AND [EntityStatus] != 2
ENd
