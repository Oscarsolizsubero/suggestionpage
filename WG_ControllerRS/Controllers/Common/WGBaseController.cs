using System;
using System.Collections.Generic;

using System.Linq;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WishGrid.Controllers
{
    public class WGBaseController : Controller
    {
        protected const string TYPE = "application/json";
        protected const string ROUTE = "api/[controller]";
        protected const string CREATE = "create";
        protected const string EDIT = "edit";
        protected const string REMOVE = "remove";
        protected const string LOAD = "load";
        protected const string SEARCH = "search";
        protected const string SIZE = "size";

        public WGBaseController() : base() { }

        //public IActionResult VerifyRequest()
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //}

    }
}
