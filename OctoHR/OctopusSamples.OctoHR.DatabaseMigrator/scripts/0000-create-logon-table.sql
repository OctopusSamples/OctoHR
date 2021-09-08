IF NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'UserAccountRecords')
BEGIN
	PRINT 'Table [dbo].[UserAccountRecords] doesnt exist, creating.'

	CREATE TABLE [dbo].[UserAccountRecords] (
		[Username] NVARCHAR(255) NOT NULL PRIMARY KEY,
		[Name] NVARCHAR(255) NOT NULL,
		[IsAdmin] BIT NOT NULL,
		[Enabled] BIT NOT NULL
	);
END 
ELSE
BEGIN
	PRINT 'Table [dbo].[UserAccountRecords] exists'
END
