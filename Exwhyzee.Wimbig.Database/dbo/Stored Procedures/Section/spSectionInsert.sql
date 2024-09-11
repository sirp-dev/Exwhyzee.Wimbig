------ INsert Section

CREATE PROCEDURE [dbo].[spSectionInsert]
	@name nvarchar(200),
	@descripiton nvarchar(Max),
	@dateCreated datetime,
	@entityStatus int

	AS
	BEGIN

	INSERT INTO [dbo].[Section] ([Name],[Description],[DateCreated],[EntityStatus])
	output inserted.SectionId
	VALUES (@name, @descripiton,@dateCreated,@entityStatus)
END
