SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF EXISTS ( SELECT * 
            FROM   sysobjects 
            WHERE  id = object_id(N'[dbo].[CheckUser]') 
                   and OBJECTPROPERTY(id, N'IsProcedure') = 1 )
BEGIN
	PRINT 'Dropping [dbo].[CheckUser]'

    DROP PROCEDURE [dbo].[CheckUser]
END
GO
PRINT 'Creating [dbo].[CheckUser]'
GO
CREATE PROCEDURE [dbo].[CheckUser]
@Username NVARCHAR(255)
AS
BEGIN

	SET NOCOUNT ON;

    IF EXISTS (
        SELECT * FROM [dbo].[UserAccountRecords] WHERE [Username] = @Username
    )
    BEGIN
        SELECT [Name], [IsAdmin] FROM [dbo].[UserAccountRecords] WHERE [Username] = @Username
    END
    
END
GO