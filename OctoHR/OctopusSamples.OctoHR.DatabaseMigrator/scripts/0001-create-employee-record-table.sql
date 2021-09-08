IF NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'EmployeeRecords')
BEGIN
	PRINT 'Table [dbo].[EmployeeRecords] doesnt exist, creating.'

	CREATE TABLE [dbo].EmployeeRecords (
		[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
		[Name] NVARCHAR(255) NOT NULL,
		[Position] NVARCHAR(255) NULL,
		[Salary] DECIMAL(8,2) NULL,
		[Manager] NVARCHAR(255) NULL,
		[CurrentStaff] BIT NOT NULL
	);
END 
ELSE
BEGIN
	PRINT 'Table [dbo].[EmployeeRecords] exists'
END
