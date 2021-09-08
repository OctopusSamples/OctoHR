IF NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'ClientConfiguration')
BEGIN
	PRINT 'Table [dbo].[ClientConfiguration] doesnt exist, creating.'

	CREATE TABLE [dbo].[ClientConfiguration] (
		[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
		[Name] NVARCHAR(255) NOT NULL,
		[Slug] NVARCHAR(255) NOT NULL,
		[Description] NVARCHAR(255) NULL,
		[ImageUrl] NVARCHAR(255) NOT NULL,
		[ClientDatabase] NVARCHAR(255) NOT NULL,
		[Enabled] BIT NOT NULL
	);
END 
ELSE
BEGIN
	PRINT 'Table [dbo].[ClientConfiguration] exists'
END
