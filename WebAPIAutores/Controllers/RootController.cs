using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPIAutores.DTOs;

namespace WebAPIAutores.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RootController : ControllerBase
    {
        private readonly IAuthorizationService authorizationService;

        public RootController(IAuthorizationService authorizationService)
        {
            this.authorizationService = authorizationService;
        }
        [HttpGet(Name = "GetRoot")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<DataHATEOAS>>> Get()
        {
            var dataHateoas = new List<DataHATEOAS>();
            var isAdmin = await authorizationService.AuthorizeAsync(User, "isAdmin");

            dataHateoas.Add(new DataHATEOAS(link: Url.Link("GetRoot", new { }), description: "self", method: "GET"));

            dataHateoas.Add(new DataHATEOAS(link: Url.Link("getAutors", new { }), description: "autors", method: "GET"));
            if (isAdmin.Succeeded)
            {
                dataHateoas.Add(new DataHATEOAS(link: Url.Link("createAutor", new { }), description: "create-autors", method: "POST"));
                dataHateoas.Add(new DataHATEOAS(link: Url.Link("createBook", new { }), description: "create-book", method: "POST"));
            }

            return dataHateoas;
        }
    }
}
