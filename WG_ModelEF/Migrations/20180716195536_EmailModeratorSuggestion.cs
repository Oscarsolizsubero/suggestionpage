using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WG_ModelEF.Migrations
{
    public partial class EmailModeratorSuggestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var spEmailModeratorSuggestion =
                @"CREATE PROCEDURE EmailModeratorSuggestion 
                    @SuggestioId int,
					@statusId int
                    AS
                    DECLARE @count INT,
		                    @contador INT, 
		                    @email VARCHAR(MAX), @emailModerator VARCHAR(MAX) = '',
		                    @direccion VARCHAR(MAX) = 'http://' + (select t.URLOrigin from Suggestion s inner join [User] u on s.AuthorId = u.Id
																	                                    inner join Tenant t on u.TenantId = t.Id
													                                    where s.Id = @SuggestioId)
		                                                +'/suggestion-detail/'+ convert(varchar(20), @SuggestioId,103),
		                    @tableHTML  VARCHAR(MAX),
		                    @subject VARCHAR(80),
			                @title VARCHAR(200)

	                    SET @count = (select COUNT(*) from [User] u inner join [Role] r on u.RoleId = r.id 
                            where u.TenantId = (select TenantId from [User] where Id = (select s.AuthorId from Suggestion s where s.Id = @SuggestioId)) and r.Id in (2,3))
	                    IF @count > 0
	                    BEGIN
			                SET @contador = 1			
		                    WHILE @count >= @contador
			                BEGIN
			                    SET @email =(SELECT email FROM 
							                    (SELECT U.email,ROW_NUMBER()OVER (ORDER BY u.id ASC)AS ROW_NUMBER 
								                from [User] u inner join [Role] r on u.RoleId = r.id 
                                                where u.TenantId = (select TenantId from [User] u where u.Id = (select s.AuthorId from Suggestion s where s.Id = @SuggestioId)) and r.Id in (2,3)) as R
						                    WHERE ROW_NUMBER=@contador )
			   
			                    SET @emailModerator = @emailModerator + ';' + @email
			                    SET @contador = @contador + 1
			                END
		                SET @title = (select Title from Suggestion where Id = @SuggestioId)
	                    SET @email = (select SUBSTRING (@emailModerator,2,10000))
					    IF @statusId = 1
						BEGIN
						SET @tableHTML =  
					            N'<h4> NOTIFICACIÓN </h4>' +  
                                N' Hola, <br/>' +
                                N' Una nueva sugerencia  <b>  ' + @title + ' </b> fue creada, ahora podras aprovarla o rechazarla.  <br/>' +
                                N' Puedes acceder a la sugerencia haciendo clic <a href='+ @direccion + '> Aqui </a><br/>' +
                                N'<p>Atentamente,' + '</p>' +
                                N'Equipo de WISHGRID.';

						SET @subject = 'Nueva sugerencia fue creada'
						END
						IF @statusId = 2
						BEGIN
						SET @tableHTML =  
								N'<h4> NOTIFICACIÓN </h4>' +  
                                N' Hola, <br/>' +
								N' Una nueva sugerencia  <b>  ' + @title + ' </b>  fue creada, puedes acceder a la sugerencia haciendo clic <a href='+ @direccion + '> Aquí </a><br/>' +
								N'<p>Atentamente,' + '</p>' +
                                N'Equipo de WISHGRID.';

					    SET @subject = 'Nueva sugerencia fue creada'

						END
                            EXEC msdb.dbo.sp_send_dbmail
                            @profile_name = 'perfil1',
                            @recipients = @email,
                            @body = @tableHTML,
                            @body_format = 'HTML',
                            @subject = @subject;
                         END
                       GO"; migrationBuilder.Sql(spEmailModeratorSuggestion);

            var spEmailAceptedSuggestion =
                @"CREATE PROCEDURE EmailAceptedSuggestion
                    @SuggestioId int 
                    AS
                        DECLARE @email nvarchar(max),
			                    @tableHTML varchar(max),
				                    @subject varchar(80),
				                    @title nvarchar(max),
			                    @description nvarchar(max),
			                    @direccion VARCHAR(MAX) = 'http://' + (select t.URLOrigin from Suggestion s inner join [User] u on s.AuthorId = u.Id
																	                    inner join Tenant t on u.TenantId = t.Id
													                    where s.Id = @SuggestioId) +
									                    '/suggestion-detail/'+ convert(varchar(20), @SuggestioId,103),
			                    @userName nvarchar(max)
					                    SET @email = (SELECT u.Email FROM Suggestion s inner join [User] u ON s.AuthorId = u.Id 
											                    WHERE s.Id = @SuggestioId and s.StatusId in (2,3))
					                    SET @title = (SELECT s.Title FROM Suggestion s WHERE s.Id = @SuggestioId and s.StatusId in (2,3))			                           
					                    SET @userName = (SELECT u.Name + ' ' + u.LastName FROM Suggestion s inner join [User] u ON s.AuthorId = u.Id 
											                    WHERE s.Id = @SuggestioId and s.StatusId in (2,3))									  
						                    SET @tableHTML =  
							                    N'<H4>NOTIFICACIÓN </H4>' +  
							                    N'<p>Hola ' + @userName + ' , </p>' + 
							                    N' <p>Tu sugerencia : <b> ' + @title + ' </b> ha sido aprobada. </p>' +
												N' <p>Ahora la sugerencia se incluirá en nuestra lista de sugerencias para todos los usuarios. </p>' +
                                                N' <p>Puedes acceder a la sugerencia haciendo clic <a href=' + @direccion + '> Aqui </a></p>' +
                                                N'<p>Atentamente,' + '</p>' +
                                                N'Equipo de WISHGRID.';
                                SET @subject = 'Tu sugerencia fue aprobada'

                                    EXEC msdb.dbo.sp_send_dbmail
                                    @profile_name = 'perfil1',
                                    @recipients = @email,
                                    @body = @tableHTML,
                                    @body_format = 'HTML',
                                    @subject = @subject;
                                GO"; migrationBuilder.Sql(spEmailAceptedSuggestion);

            var spEmailDesignedModerator =
                @"CREATE PROCEDURE EmailDesignedModerator
	                @AuthorId int 
                    AS
                    DECLARE 
	                        @email VARCHAR(MAX), 
	                        @tableHTML VARCHAR(MAX),
	                        @direccion VARCHAR(MAX)
	
	                        SET @direccion =('http://' + (select t.URLOrigin 
					                                        from  [User] u inner join Tenant t on u.TenantId = t.Id
								                           where u.Id = @AuthorId))

	                        SET @email = (select u.Email from [User] u where u.Id = @AuthorId)
	                        SET @tableHTML =  
				                        N'<H4>NOTIFICACIÓN </H4>' +  
				                        N' Felicitaciones!!!!<br/>' + 
				                        N' Usted está asignado como moderador de <a href='+ @direccion + '> WishGrid </a><br/>' + 
				                        N'<p> Atentamente,' + '</p>' +
                                        N'Equipo de WISHGRID.';
			
                            EXEC msdb.dbo.sp_send_dbmail
                            @profile_name = 'perfil1',
                            @recipients = @email,
                            @body = @tableHTML,
                            @body_format = 'HTML',
                            @subject = 'Nueva Asignación'; 
                        GO"; migrationBuilder.Sql(spEmailDesignedModerator);

            var spEmailResetPassword =
                @"CREATE PROCEDURE EmailResetPassword
                @AuthorId INT,
                @Email VARCHAR(100),
                @Token VARCHAR(max)
                AS
                DECLARE 
	                @direccion VARCHAR(MAX)=('http://' + (select t.URLOrigin from  [User] u inner join Tenant t on u.TenantId = t.Id
								                                                where u.Id = @AuthorId)+ '/account-validation'),
	                @tableHTML  NVARCHAR(MAX),
	                @Name varchar (50)
	                SET @Name = (SELECT u.Name + ' '+ u.LastName  FROM [User] u WHERE u.Id = @AuthorId)
                    SET @tableHTML =  
			                N'<H4> NOTIFICACIÓN </H4>' +  
			                N'<p> Hola, ' + @Name + ': </p> '+ 
			                N'Se uso la direccion de correo ' + @Email + ' para restablecer tu contraseña <br/>' +
			                N' <p>Para terminar de restablecer tu contraseña debes confirmarla con este código</p>'+ 
			                N' <p><H3>'+ @Token + '</H3></p>' +
			                N' <p>haciendo clic <a href=' + @direccion + '> Aquí </a></p>' +
                            N'Atentamente,' + '<br/>' +
                            N'Equipo de WISHGRID.';
			                EXEC msdb.dbo.sp_send_dbmail
			                @profile_name = 'perfil1',
			                @recipients= @Email, 
			                @body = @tableHTML, 
			                @body_format = 'HTML',
			                @subject = 'Restablecer Contraseña de WISHGRID ';
                GO"; migrationBuilder.Sql(spEmailResetPassword);

            var spEmailCreateUserAndConfirmation =
                @"CREATE PROCEDURE EmailCreateUserAndConfirmation
                 @idUsuario int,@idTenant int, @token varchar(max)
                 AS
                 DECLARE @email nvarchar(max)= (SELECT u.Email FROM [User] u WHERE u.Id = @idUsuario),
			            @tableHTML varchar(max),
				        @subject varchar(80),				              			                   
			            @direccion VARCHAR(MAX) = 'http://' + (select t.URLOrigin from Tenant t where t.Id = @idTenant)+ '/account-validation',
			            @nametenant VARCHAR(MAX) = (select t.NameTenants from Tenant t where t.Id = @idTenant),
						@Name varchar(50) = (SELECT u.Name FROM [User] u WHERE u.Id = @idUsuario)
						SET @tableHTML =  
							N'<H4>BIENVENIDO...!!!!!!! </H4>' +  
							N'<p> Hola ' + @Name + ' ,</p>' + 
							N' Somos del equipo de WISHGRID.<br/>' +
							N' Mil gracias por haberte suscrito. <br/>'+
							N' <p>Somos una aplicación de sugerencias para la empresa <b>'+ @nametenant +'</b></p>' +
							N' <p>Para que empieces a utilizar WISHGRID debes confirmar tu suscripción con este código'+ 
							N' <p><H3>'+ @token + '</H3></p>' +
							N' <p>haciendo clic <a href=' + @direccion + '> Aquí </a></p>' +
                            N'Atentamente,' + '<br/>' +
                            N'Equipo de WISHGRID.';
                        SET @subject = 'Creación de Cuenta WISHGRID'

                        EXEC msdb.dbo.sp_send_dbmail
                        @profile_name = 'perfil1',
                        @recipients = @email,
                        @body = @tableHTML,
                        @body_format = 'HTML',
                        @subject = @subject;
            GO"; migrationBuilder.Sql(spEmailCreateUserAndConfirmation);

            var spEmailPasswordReset =
                @"CREATE PROCEDURE EmailPasswordReset
                @AuthorId INT,
                @ResetPassword VARCHAR(max)
                AS
                DECLARE 
	                @direccion VARCHAR(MAX)=('http://' + (select t.URLOrigin from  [User] u inner join Tenant t on u.TenantId = t.Id
								                                             where u.Id = @AuthorId)+ '/home'),
	                @tableHTML  NVARCHAR(MAX),
	                @email VARCHAR(max) = (select Email from [User] where Id = @AuthorId),
	                @Name varchar (50)
	                SET @Name = (SELECT u.Name + ' '+ u.LastName  FROM [User] u WHERE u.Id = @AuthorId)
                    SET @tableHTML =  
			                N'<H4> NOTIFICACIÓN </H4>' +  
			                N'<p> Hola, ' + @Name + ': </p> '+ 
			                N' <p>Tu nueva contraseña es: <b>'+ @ResetPassword + '</b></p>'+ 
			                N' <p>Puedes ingresar haciendo clic <a href=' + @direccion + '> Aquí </a></p>' +
			                N' <p>Al ingresar puede dirigirse al menu para cambiar su contraseña</p>' +
                            N'Atentamente,' + '<br/>' +
                            N'Equipo de WISHGRID.';
			                EXEC msdb.dbo.sp_send_dbmail
			                @profile_name = 'perfil1',
			                @recipients= @Email, 
			                @body = @tableHTML, 
			                @body_format = 'HTML',
			                @subject = 'Restablecer Contraseña de WISHGRID ';
                GO"; migrationBuilder.Sql(spEmailPasswordReset);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
