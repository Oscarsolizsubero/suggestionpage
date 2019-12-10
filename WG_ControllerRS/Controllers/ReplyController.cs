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
using WishGrid.ViewModels.Shared;

namespace WishGrid.Controllers
{
    [Authorize]
    [Produces(TYPE)]
    [Route(ROUTE)]
    public class ReplyController : WGBaseController
    {
        private readonly IRReply _repository;
        private readonly IConfiguration _config;

        public ReplyController(IRReply repository, IConfiguration config) : base()
        {
            _repository = repository;
            _config = config;
        }

        [HttpPost(CREATE)]
        public IActionResult Create([FromBody]VMReplyAdd request)
        {
            if (request.IdAuthor != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            if (!_repository.AdminorModerator(request.IdAuthor))
            {
                return Unauthorized();
            }
            if (_repository.isFullfilled(request.IdSuggestion))
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            VMReply vm = new VMReply()
            {
                Description = request.Description,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                Author = new VMUserSimple(request.IdAuthor),
                IdSuggestion = request.IdSuggestion
            };
            VMMessage msj = _repository.Insert(vm);
            if (msj.IsSuccessful())
            {
                return Ok(vm);
            }
            else
            {
                return BadRequest(msj.Text);
            }
        }
        [AllowAnonymous]
        [HttpGet(SEARCH+ "/{suggestionId}")]
        public IActionResult SearchAllBySuggestion(int suggestionId)
        {
            return Ok(_repository.SelectAllBySuggestion(suggestionId));
        }
    }
}
