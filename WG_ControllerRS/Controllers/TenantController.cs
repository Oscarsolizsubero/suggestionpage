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
    public class TenantController : WGBaseController
    {
        private readonly IRTenant _repository;
        private readonly IConfiguration _config;

        public TenantController(IRTenant repository, IConfiguration config) : base()
        {
            _repository = repository;
            _config = config;
        }

        [HttpGet(LOAD + "/{tenant}/{userId}/{roleId}")]
        public IActionResult Load(string tenant, int userId, int roleId)
        {
            var test = User.Claims.FirstOrDefault(x => x.Value == tenant);
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) || (User.Claims.FirstOrDefault(x => x.Value == tenant) == null) || int.Parse(User.FindFirst(ClaimTypes.Role).Value) != roleId)
                return Unauthorized();

            VMTenant t = _repository.select(tenant);
            if (t == null)
            {
                return BadRequest("Could not find tenant.");
            }
            else
            {

                return Ok(t);
            }
        }

        [HttpPut(EDIT)]
        public IActionResult Edit([FromBody]VMTenantEdit request)
        {
            if (request.LoggedUserId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) || int.Parse(User.FindFirst(ClaimTypes.Role).Value) != request.LoggedUserRole
                || (User.Claims.FirstOrDefault(x => x.Value == request.TenantURL) == null))
                return Unauthorized();

            if (request.LoggedUserRole != 2)
            {
                return Unauthorized();
            }
            else
            {
                _repository.EditModeration(request.Id,request.Moderation);
                return Ok();
            }
        }

        [AllowAnonymous]
        [HttpGet("image/{URLOrigin}")]
        public IActionResult TenantImage(string URLOrigin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            VMTenantImage t = _repository.selectImage(URLOrigin);
            if (t == null)
            {
                return BadRequest("Could not find tenant.");
            }
            else
            {
                return Ok(t);
            }
        }

    }
}
