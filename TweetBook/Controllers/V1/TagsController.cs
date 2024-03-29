﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TweetBook.Contract.V1.Responses;
using TweetBook.Services;

namespace TweetBook.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Poster,Admin")]
    public class TagsController : Controller
    {
        private readonly IPostService postService;

        public TagsController(IPostService postService)
        {
            this.postService = postService;
        }

        [HttpGet(Contract.V1.ApiRoutes.Tags.GetAll)]
        [Authorize(Policy = "TagViewer")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(this.postService.GetAllTags().Select(tag => new TagResponse {Name = tag.Name}));
        }

        [HttpDelete(Contract.V1.ApiRoutes.Tags.Delete)]
        [Authorize(Roles = "Admin", Policy = "MustWorkForNico")]
        public async Task<IActionResult> Delete([FromRoute] string tagName)
        {
            var deleted = this.postService.DeleteTag(tagName);

            if (deleted)
                return NoContent();

            return NotFound();
        }
    }
}