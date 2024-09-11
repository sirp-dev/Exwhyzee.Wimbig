--Create Category Entity

CREATE PROCEDURE [dbo].[spCategoryInsert]
		@categoryName nvarchar(250) ,

		@decription nvarchar(Max),
		@dateCreated datetime,
		@sectionId bigint,
		@entityStatus int
AS
BEGIN
	INSERT INTO [dbo].[Category] ([Name],[DateCreated],[Description],[SectionId],[EntityStatus])
	output inserted.CategoryId

	VALUES (@categoryName,@dateCreated,@decription,@sectionId,@entityStatus)
END
