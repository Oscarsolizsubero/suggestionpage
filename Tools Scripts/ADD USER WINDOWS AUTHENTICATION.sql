USE [master]
GO  
CREATE PROCEDURE ADD_USER_WINDOWS   
    @user_name nvarchar(255)
AS   
BEGIN
	DECLARE @not_exist AS INT;

	SET @not_exist=(SELECT count(u.name) FROM sys.server_principals u WHERE u.name='DBI\Wilber.Padilla');

    SET NOCOUNT ON;  
	IF(@not_exist=0)BEGIN
		CREATE LOGIN [@user_name] FROM WINDOWS WITH DEFAULT_DATABASE=[WishGrid];
		ALTER SERVER ROLE [sysadmin] ADD MEMBER [@user_name];			
		CREATE USER [@user_name] FOR LOGIN [@user_name]	
		EXEC sp_addrolemember N'db_owner', [@user_name]
		ALTER ROLE [WishGrid.db_owner] ADD MEMBER 
	END
END    	

GO
CREATE LOGIN [DBI\Miguel.Castedo] FROM WINDOWS WITH DEFAULT_DATABASE=[WishGrid]
GO
ALTER SERVER ROLE [sysadmin] ADD MEMBER [DBI\Miguel.Castedo]
GO
USE [WishGrid]
GO
CREATE USER [DBI\Miguel.Castedo] FOR LOGIN [DBI\Miguel.Castedo]
GO
USE [WishGrid]
GO
ALTER ROLE [db_owner] ADD MEMBER [DBI\Miguel.Castedo]
GO
