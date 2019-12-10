using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WishGrid.Inputs;
using WishGrid.IRepositories;
using WishGrid.Models;
using WishGrid.Security;
using WishGrid.ViewModels;
using WishGrid.ViewModels.Shared;

namespace WishGrid.Controllers
{
    [Authorize]
    [Produces(TYPE)]
    [Route(ROUTE)]
    public class UserController : WGBaseController
    {
        private readonly IRUser _repository;
        private readonly IConfiguration _config;

        public UserController(IRUser repository, IConfiguration config)
        {
            _repository = repository;
            _config = config;
        }

        [HttpPost(SEARCH)]
        public IActionResult Search([FromBody]RequestPaginatorWithUser request)
        {
            if (request.IdUser != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) || int.Parse(User.FindFirst(ClaimTypes.Role).Value) != 2)
                return Unauthorized();

            return Ok(_repository.Select(request.PageSize, request.PageNumber, request.Filters, request.Tenant));
        }

        [HttpPost(SIZE)]
        public int Size([FromBody]RequestPaginatorWithUser paginator)
        {
            return _repository.Count(paginator.Tenant, paginator.Filters);
        }

        [HttpPut(EDIT)]
        public IActionResult Edit([FromBody]VMUserforListEdit request)
        {
            if (request.LoggedUserId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) || int.Parse(User.FindFirst(ClaimTypes.Role).Value) != request.LoggedUserRole
                || (User.Claims.FirstOrDefault(x => x.Value == request.LoggedUserTenant) == null))
                return Unauthorized();
            else
            {
                VMUserforList user = new VMUserforList
                {
                    Id = request.Id,
                    Username = request.Username,
                    Email = request.Email,
                    Moderator = request.Moderator
                };
                _repository.EditUser(user);
                return Ok();
            }
        }
        //CREA USUARIOS QUE QUIERAN LOGUEARSE DENTRO DE UN DOMINIO WISHGRID QUE EXISTA Y QUE EL USER NAME NO SE REPITA
        [AllowAnonymous]
        [HttpPost(CREATE)]
        public IActionResult CreateUser([FromBody] VMUserAdd request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_repository.VerifyUserName(request.Email, request.Tenant))
            {
                byte[] hash, salt;
                SecurityHelper.Encrypt(request.Password, out hash, out salt);
                _repository.CreateUser(request, hash, salt);
                return Ok("successfully created user");
            }
            else
            {
                return BadRequest("Error created user");
            }
        }
        [AllowAnonymous]
        [HttpPost("verificationEmail")]
        public IActionResult verificationEmail([FromBody] VMUserAdd request)
        {
            if (_repository.VerifyUserName(request.Email, request.Tenant))
            {
                return Ok("successfully created user");
            }
             return BadRequest("Error created user");
        }
        //CAMBIAR LA CONTRASEÑA DE UN USUARIO
        [HttpPut(EDIT + "Password")]
        public IActionResult EditPassword([FromBody]VMUserPassword request)
        {
            //VERFIFICA EL ID DEL USUARIO PARA HACER LA MODIFICACION DE SU PASSWORD
            if (request.Id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (VerifyPassword(request.oldPassword, request.Id))
            {
                //ENCRYPT NEW PASSWORD
                byte[] hash, salt;
                SecurityHelper.Encrypt(request.password, out hash, out salt);
                //REPLACE NEW PASSWORD
                _repository.UpdatePassword(request.Id, hash, salt);
                return Ok("Password has been changed");
            }
            else
            {
                return StatusCode(406);
            }
        }
        private bool VerifyPassword(string OldPassword, int IdUser)
        {
            VMUserEncritpted userEncritpted = _repository.FindByIdUser(IdUser);
            if (userEncritpted != null &&
                Security.SecurityHelper.VerifyEncryption(OldPassword, userEncritpted.PasswordHash, userEncritpted.PasswordSalt))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //RESET PASSWORD USER 
        [AllowAnonymous]
        [HttpPost("resetPassword")]
        public IActionResult ResetPassword([FromBody]VMUserAdd request)
        {
            //VERFIFICA EL ID DEL USUARIO PARA HACER LA MODIFICACION DE SU PASSWORD          
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_repository.VerifyUserNameEmail(request.Email, request.Tenant))
            {
                string character = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
                //GUARDA EN LA VARIABLE LA NUEVA CONTRASEÑA CREADA RANDONICAMENTE
                StringBuilder res = new StringBuilder();
                Random rnd = new Random();
                int c = 0;
                while (c < 8)
                {
                    res.Append(character[rnd.Next(character.Length)]);
                    c += 1;
                }
                //ENCRYPT NEW PASSWORD
                byte[] hash, salt;
                SecurityHelper.Encrypt(res.ToString(), out hash, out salt);
                //REPLACE NEW PASSWORD
                _repository.ResetPassword(request.Tenant, request.Email, hash, salt, res.ToString());
                return Ok("Reset Password has been changed");
            }
            else
            {
                return BadRequest("Error created user");
            }
        }
        /*Validacion de la Cuenta Creada, si es OK redirigirlo hacia el Home, si no mostrar mensaje "usuario o contraseña invalida"*/
        [AllowAnonymous]
        [HttpGet("validation/{tenant}/{token}")]
        public IActionResult Validation(string tenant, string token)
        {
            string validation = "";
            string decodeToken = Decode(token);
            char[] character = { '.' };
            string[] tokenDecode = decodeToken.Split(character);
            string userId = Convert.ToString(tokenDecode[0]);
            string password = Convert.ToString(tokenDecode[1]);
            if (tokenDecode.Length == 3)
            {
                 validation = Convert.ToString(tokenDecode[2]);
            }
            if (validation == "true")
            {
                if(VerifyPassword(password,Convert.ToInt32(userId)))
                {
                    var userEncritpted = _repository.ValidationCreateAccount(tenant, Convert.ToInt32(userId));
                    if (userEncritpted == true)
                    {
                        _repository.ValidationSendEmail(Convert.ToInt32(userId), password);
                        return Ok("Validation Account");
                    }
                }  
                return BadRequest("Error confirmation account");
            }
            else
            {
                var userEncritpted = _repository.ValidationCreateAccount(tenant, Convert.ToInt32(userId));
                if (userEncritpted == true)
                {
                    return Ok("Validation Account");
                }
            }           
            return BadRequest("Error confirmation account");
        }
        public static string Decode(string textDecode)
        {
            var EncodedBytes = System.Convert.FromBase64String(textDecode);
            return System.Text.Encoding.UTF8.GetString(EncodedBytes);
        }


    }
}
