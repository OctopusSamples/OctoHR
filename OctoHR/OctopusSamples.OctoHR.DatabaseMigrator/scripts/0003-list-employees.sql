SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF EXISTS ( SELECT * 
            FROM   sysobjects 
            WHERE  id = object_id(N'[dbo].[List_Employees]') 
                   and OBJECTPROPERTY(id, N'IsProcedure') = 1 )
BEGIN
	PRINT 'Dropping [dbo].[List_Employees]'

    DROP PROCEDURE [dbo].[List_Employees]
END
GO
PRINT 'Creating [dbo].[List_Employees]'
GO
CREATE PROCEDURE [dbo].[List_Employees]
AS
BEGIN

	SET NOCOUNT ON;

    SELECT [Name], [Position], [Salary], [Manager], [CurrentStaff]
	FROM [dbo].[EmployeeRecords]
END
GO