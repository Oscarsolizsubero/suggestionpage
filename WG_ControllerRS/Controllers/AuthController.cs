using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WishGrid.IRepositories;
using WishGrid.ViewModels;

namespace WishGrid.Controllers
{
    [Route(ROUTE)]
    public class AuthController : WGBaseController
    {
        private readonly IRUser _repository;
        private readonly IConfiguration _config;

        public AuthController(IRUser repository, IConfiguration config) : base()
        {
            _repository = repository;
            _config = config;
        }        

        [HttpPost("login")]
        public IActionResult Login([FromBody]VMUserAuth viewModel)
        {
            var userEncritpted = _repository.FindByLogin(viewModel.Name, viewModel.Tenant);
            if (userEncritpted != null &&
                Security.SecurityHelper.VerifyEncryption(viewModel.Password, userEncritpted.PasswordHash, userEncritpted.PasswordSalt))
            {
                //generate token
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_config.GetSection("AppSettings:Token").Value);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.NameIdentifier, userEncritpted.User.Id.ToString()),
                    new Claim(ClaimTypes.Name, userEncritpted.User.Name),
                    new Claim(ClaimTypes.Role, userEncritpted.User.RoleId.ToString())
                    }),
                    Expires = DateTime.Now.AddDays(30),
                    Audience = viewModel.Tenant,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                       SecurityAlgorithms.HmacSha512Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);
                return Ok(new { tokenString });
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
