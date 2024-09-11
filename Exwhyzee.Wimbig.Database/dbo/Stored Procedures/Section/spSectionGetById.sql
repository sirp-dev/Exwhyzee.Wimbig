-- Read Section By Id

CREATE PROCEDURE [dbo].[spSectionGetById]
	@id bigint = 0
AS
BEGIN
	SELECT * FROM [dbo].[Section] WHERE [SectionId] = @id
END
