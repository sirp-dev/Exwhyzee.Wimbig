--Delete an image

CREATE PROCEDURE [dbo].[spBarnerImageFileDelete]
	@imageId bigInt = 0
AS
BEGIN
	Delete From [dbo].[BarnerImage] Where [Id] = @imageId
END