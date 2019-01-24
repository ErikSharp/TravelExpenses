
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelExpenses.Application.Features.Categories;
using TravelExpenses.Domain.Entities;
using TravelExpenses.WebAPI.Extensions;

namespace TravelExpenses.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator mediator;

        public CategoriesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromHeader(Name = "Authorization")]string token)
        {
            var userId = User.Claims.GetUserId();
            var categories = await mediator.Send(new GetCategories.Query(userId)).ConfigureAwait(false);

            return Ok(categories);
        }

        [HttpPost]
        public async Task<IActionResult> Post(
            [FromHeader(Name = "Authorization")]string token,
            [FromBody]string[] categoryNames)
        {
            var userId = User.Claims.GetUserId();
            var categories = categoryNames.Select(n => new Category() { CategoryName = n, UserId = userId }).ToArray();
            await mediator.Send(new CreateCategory.Command(categories)).ConfigureAwait(false);

            return Created(
                new Uri(Request.Path, UriKind.Relative),
                null);
        }

        [HttpPut]
        public async Task<IActionResult> Put(
            [FromHeader(Name = "Authorization")]string token,
            [FromBody]Category category)
        {
            var userId = User.Claims.GetUserId();
            category.UserId = userId;
            await mediator.Send(new UpdateCategory.Command(category)).ConfigureAwait(false);

            return Ok();
        }
    }
}