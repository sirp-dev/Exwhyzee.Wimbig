--Update Section Entity

CREATE PROCEDURE [dbo].[spSectionUpdate]
	@sectionId int = 0,
	@name nvarchar(200),
	@description nvarchar(MAX),
	@entitySatus int 
AS
BEGIN 
 UPDATE [dbo].[Section] SET 
 [Name] = @name,
 [Description] = @description,
 [EntityStatus] = @entitySatus

 WHERE [SectionId] = @sectionId	

END
