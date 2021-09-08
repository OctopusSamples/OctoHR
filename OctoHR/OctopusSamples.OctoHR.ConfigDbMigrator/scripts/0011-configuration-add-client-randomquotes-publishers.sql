SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DECLARE @Name NVARCHAR(255)
DECLARE @Slug NVARCHAR(255)
DECLARE @Description NVARCHAR(255)
DECLARE @ImageUrl NVARCHAR(255)
DECLARE @ClientDatabase NVARCHAR(255)
DECLARE @Enabled BIT

SET @Name = N'RandomQuotes Publishers'
SET @Slug = N'randomquotes-publishers'
SET @Description = N'A book publisher full of random quotes!'
SET @ImageUrl = N''
SET @ClientDatabase='randomquotes'
SET @Enabled = 1

IF EXISTS ( SELECT * 
            FROM [dbo].[ClientConfiguration]    
            WHERE [Name] = @Name)
BEGIN
	PRINT 'Updating Client'

    UPDATE cc
		SET cc.[Slug]=@Slug,
		    cc.[Description]=@Description,
			cc.[ImageUrl]=@ImageUrl,
			cc.[ClientDatabase]=@ClientDatabase,
			cc.[Enabled]=@Enabled
	FROM [dbo].[ClientConfiguration] cc
    WHERE [Name] = @Name
END
ELSE
BEGIN
	INSERT INTO [dbo].[ClientConfiguration] ( [Name], [Slug], [Description], [ImageUrl], [ClientDatabase], [Enabled])
	SELECT @Name, @Slug, @Description, @ImageUrl, @ClientDatabase, @Enabled
END
