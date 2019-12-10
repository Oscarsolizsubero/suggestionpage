using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using WishGrid.IRepositories;
using WishGrid.Models;
using WishGrid.RepositoriesEF;
using WishGrid.Security;
using WishGrid.ViewModels;
using WishGrid.Tests.Shared;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using WishGrid.Setup.Shared;

namespace WishGrid.Tests
{
    [TestClass]
    public class UTUser : TestBaseEF
    {
        [TestCategory(Categories.User)]
        [TestMethod]
        public void Create()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            AppSettings.BuildDBContext(optionsBuilder);
            using (var context = new DataContext(optionsBuilder.Options))
            {
                string value = "password";
                byte[] arrayHash, arraySalt;
                SecurityHelper.Encrypt(value, out arrayHash, out arraySalt);
                User user = new User()
                {
                    UserName = "wpadilaa@info-arch.com",
                    RoleId = 4,
                    TenantId = 2,
                    PasswordHash = arrayHash,
                    PasswordSalt = arraySalt
                };
                context.User.Add(user);
                Assert.IsTrue(context.SaveChanges() == 1, "The changes was not made.");
                Assert.IsNotNull(context.User.Find(user.Id), "El user was not added.");
            }
        }

        [TestCategory(Categories.User)]
        [TestMethod]
        public void Update()
        {
            string value = "password";
            byte[] arrayHash, arraySalt;
            SecurityHelper.Encrypt(value, out arrayHash, out arraySalt);
            User userTest = new User()
            {
                Id = 3,
                UserName = $"User-{GenerateTitle()}",
                RoleId = 2,
                TenantId = 1,
                PasswordHash = arrayHash,
                PasswordSalt = arraySalt
            };
            context.Update(userTest);
            context.SaveChanges();
            User userFound = context.User.Find(userTest.Id);
            Assert.IsNotNull(userFound,"The user was not found.");
            Assert.IsTrue(userFound.UserName == userTest.UserName,"The datas was not changed.");
        }
        [TestCategory(Categories.User)]
        [TestMethod]
        public void login()
        {
            //headerdel token encriptado id=2 name=oscar
            string tokenprueba = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIyIiwidW5pcXVl";
            var userEncritpted = context.User.Find(2);
            if (userEncritpted != null &&
                Security.SecurityHelper.VerifyEncryption("password", userEncritpted.PasswordHash, userEncritpted.PasswordSalt))
            {
                //generate token
                var tokenHandler = new JwtSecurityTokenHandler();
                //password encriptada del server
                var key = Encoding.ASCII.GetBytes("clave xxxxxxxxxxxxx");
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.NameIdentifier, userEncritpted.Id.ToString()),
                    new Claim(ClaimTypes.Name, userEncritpted.UserName)
                    }),
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                       SecurityAlgorithms.HmacSha512Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);
                Assert.IsTrue(tokenString.Contains(tokenprueba), "token generated succesfully");
            }
        }
    }
}
