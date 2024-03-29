USE [master]
GO
CREATE LOGIN [wishgrid] WITH PASSWORD=N'password', DEFAULT_DATABASE=[WishGrid], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO
ALTER SERVER ROLE [sysadmin] ADD MEMBER [wishgrid]
GO
USE [WishGrid]
GO
CREATE USER [wishgrid] FOR LOGIN [wishgrid]
GO
USE [WishGrid]
GO
ALTER ROLE [db_owner] ADD MEMBER [wishgrid]
GO
