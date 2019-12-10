using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WishGrid.ViewModels;
using WishGrid.ViewModels.Shared;
using WishGrid.IRepositories;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using WishGrid.Models;
using WishGrid.Inputs;
using System.Security.Claims;

namespace WishGrid.Controllers
{
    [Authorize]
    [Produces(TYPE)]
    [Route(ROUTE)]
    public class SuggestionController : WGBaseController
    {
        private readonly IRSuggestion _repository;
        private readonly IConfiguration _config;

        public SuggestionController(IRSuggestion repository, IConfiguration config) : base()
        {
            _repository = repository;
            _config = config;
        }
        
        [HttpPost(SIZE + "private")]
        public int SizePrivate([FromBody]RequestPaginatorWithUser paginator)
        {
            if ((int.Parse(User.FindFirst(ClaimTypes.Role).Value) != 2) && (int.Parse(User.FindFirst(ClaimTypes.Role).Value) != 3))
            {
                return _repository.CountPublic(paginator.Tenant, paginator.StatusId, paginator.Filters);
            }
            return _repository.CountPrivate(paginator.Tenant, paginator.StatusId, paginator.Filters);
        }
        [AllowAnonymous]
        [HttpPost(SIZE)]
        public int SizePublic([FromBody]RequestPaginatorWithUser paginator)
        {
            return _repository.CountPublic(paginator.Tenant, paginator.StatusId, paginator.Filters);
        }
        [HttpPost(SEARCH+"private")]
        public IActionResult SearchPrivate([FromBody]RequestPaginatorWithUser request)
        {
            if ((int.Parse(User.FindFirst(ClaimTypes.Role).Value) != 2) && (int.Parse(User.FindFirst(ClaimTypes.Role).Value) != 3))
            {
                return Ok(_repository.SelectPublic(request.PageSize, request.PageNumber, request.Filters, request.IdUser, request.Tenant, request.StatusId));
            }
            return Ok(_repository.SelectPrivate(request.PageSize, request.PageNumber, request.Filters, request.IdUser, request.Tenant, request.StatusId));
        }
        [AllowAnonymous]
        [HttpPost(SEARCH)]
        public IActionResult SearchPublic([FromBody]RequestPaginatorWithUser request)
        {
            return Ok(_repository.SelectPublic(request.PageSize, request.PageNumber, request.Filters, request.IdUser, request.Tenant, request.StatusId));
        }
        [AllowAnonymous]
        [HttpGet(LOAD + "/{tenant}/{userId}/{suggestionId}")]
        public IActionResult Load(string tenant, int userId, int suggestionId)
        {
            var test = User.Claims.FirstOrDefault(x => x.Value == tenant);
            

            VMVote vmvote = new VMVote()
            {
                SuggestionId = suggestionId,
                UserId = userId
            };
            VMSuggestion vm = _repository.Select(vmvote, tenant);
            if (vm == null)
            {
                return BadRequest("Could not find suggestion.");
            }
            else
            {

                return Ok(vm);
            }
        }

        [HttpPost(CREATE)]
        public IActionResult Create([FromBody]VMSuggestionAdd request)
        {
            //var aud = User.Claims.First(x => x.Value == request.Tenant);
            //if (aud == null)
            //    return Unauthorized();
            int currentstatus;
            if (request.IdAuthor != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            if (_repository.Moderation(request.IdAuthor) && !_repository.AdminorModerator(request.IdAuthor))
            {
                currentstatus = 1;
            }
            else
            {
                currentstatus = 2;
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            VMSuggestion vm = new VMSuggestion()
            {
                Title = request.Title.ToUpper(),
                Description = request.Description,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                QuantityVote = 0,
                Author = new VMUserSimple(request.IdAuthor),
                StatusId = currentstatus
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

        [HttpPut(EDIT)]
        public IActionResult Edit([FromBody]VMSuggestion request)
        {
            if (request.IdAuthor != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_repository.isFullfilled(request.Id))
            {
                return BadRequest();
            }
            request.UpdatedDate = DateTime.Now;
            VMMessage msj = _repository.Update(request);
            if (msj.IsSuccessful())
            {
                return Ok("Edited Successfully.");
            }
            else
            {
                return BadRequest(msj.Text);
            }
        }
        //Devuelve la lista de status que puede buscar cualquier persona
        
        [HttpPut(EDIT + "status")]
        public IActionResult EditStatus([FromBody]VMSuggestionEditStatus request)
        {
            if (request.UserId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            if (!_repository.AdminorModerator(request.UserId))
            {
                return Unauthorized();
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                VMMessage msj = _repository.UpdateStatus(request);
                if (msj.IsSuccessful())
                {
                    return Ok("Edited Successfully.");
                }
                else
                {
                    return BadRequest(msj.Text);
                }
            }
        }

        [HttpDelete(REMOVE + "/{id}")]
        public IActionResult Remove(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            VMMessage msj = _repository.Delete(id);
            if (msj.IsSuccessful())
            {
                return Ok("Deleted Successfully.");
            }
            else
            {
                return BadRequest(msj.Text);
            }
        }

        //add vote of a user from a suggestion
        [HttpPost("vote")]
        public IActionResult Vote([FromBody]VMVote vmvote)
        {
            //search first if the user has already voted a suggestion
            if (_repository.IsVote(vmvote) || _repository.IsAuthor(vmvote))
            {
                return BadRequest();
            }
            if (_repository.isFullfilled(vmvote.SuggestionId))
            {
                return StatusCode(406);
            }
            if (_repository.Select(vmvote.SuggestionId) == null)
            {
                return NotFound();
            }
            _repository.Vote(vmvote);
            return Ok("Voted Successfully.");
        }

        [HttpDelete("vote/{userId}/{suggestionId}")]
        public IActionResult DeleteVote(int userId, int suggestionId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            //search first if the user has already voted a suggestion
            VMVote vote = new VMVote
            {
                SuggestionId = suggestionId,
                UserId = userId
            };
            if (_repository.isFullfilled(suggestionId))
            {
                return StatusCode(406);
            }
            if (_repository.IsAuthor(vote))
            {
                return BadRequest();
            }
            if (_repository.IsVote(vote))
            {
                _repository.VoteDelete(vote);
                return Ok("Delete Successfully");
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
