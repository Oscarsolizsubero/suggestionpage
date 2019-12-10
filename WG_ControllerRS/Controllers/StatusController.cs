using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WishGrid.IRepositories;
using WishGrid.ViewModels;

namespace WishGrid.Controllers
{
    [Authorize]
    [Produces(TYPE)]
    [Route(ROUTE)]
    public class StatusController : WGBaseController
    {
        private readonly IRStatus _repository;
        private readonly IConfiguration _config;

        public StatusController(IRStatus repository, IConfiguration config) : base()
        {
            _repository = repository;
            _config = config;
        }

        [AllowAnonymous]
        [HttpGet(SEARCH)]
        public IActionResult SearchStatusPublic()
        {
            return Ok(_repository.SelectStatusPublic());
        }

        //Devuelve la lista de status que puede buscar el Admin y Moderator
       [HttpGet(SEARCH + "private/{IdRole}")]
        public IActionResult SearchStatusPrivate(int IdRole)
        {
            int role = int.Parse(User.FindFirst(ClaimTypes.Role).Value);
            if (IdRole != role)
            {
                return Unauthorized();
            }
            if (role != 2 && role != 3)
            {
                return Ok(_repository.SelectStatusPublic());
            }
            return Ok(_repository.SelectStatusPrivate());
        }
        [HttpGet(SEARCH + "statusforedit/{IdRole}/{IdSuggestion}")]
        public IActionResult SearchStatusForEdit(int IdRole,int IdSuggestion)
        {
            int role = int.Parse(User.FindFirst(ClaimTypes.Role).Value);
            if (IdRole != role)
            {
                return Unauthorized();
            }
            if (role != 2 && role != 3)
            {
                return Unauthorized();
            }
            return Ok(_repository.SelectStatusforEdit(IdSuggestion));
        }
    }
}