SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF EXISTS ( SELECT * 
            FROM   sysobjects 
            WHERE  id = object_id(N'[dbo].[List_Enabled_Clients]') 
                   and OBJECTPROPERTY(id, N'IsProcedure') = 1 )
BEGIN
	PRINT 'Dropping [dbo].[List_Enabled_Clients]'

    DROP PROCEDURE [dbo].[List_Enabled_Clients]
END
GO
PRINT 'Creating [dbo].[List_Enabled_Clients]'
GO
CREATE PROCEDURE [dbo].[List_Enabled_Clients]
AS
BEGIN

	SET NOCOUNT ON;

    SELECT [Name], [Description], [ImageUrl], [ClientConnection]
	FROM [dbo].[ClientConfiguration]
	WHERE [Enabled] = 1
END
GO


