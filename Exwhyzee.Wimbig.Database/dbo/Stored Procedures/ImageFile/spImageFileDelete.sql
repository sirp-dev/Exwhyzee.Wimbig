--Delete an image

CREATE PROCEDURE [dbo].[spImageFileDelete]
	@imageId bigInt = 0
AS
BEGIN
	Delete From [dbo].[ImageFile] Where [Id] = @imageId
END