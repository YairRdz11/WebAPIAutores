using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using WebAPIAutores.DTOs;

namespace WebAPIAutores.Services
{
    public class LinksGenerator
    {
        private readonly IAuthorizationService authorizationService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IActionContextAccessor actionContextAccesor;

        public LinksGenerator(IAuthorizationService authorizationService, IHttpContextAccessor httpContextAccessor,
            IActionContextAccessor actionContextAccesor)
        {
            this.authorizationService = authorizationService;
            this.httpContextAccessor = httpContextAccessor;
            this.actionContextAccesor = actionContextAccesor;
        }
        private IUrlHelper BuildURLHelper()
        {
            var factory = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IUrlHelperFactory>();

            return factory.GetUrlHelper(actionContextAccesor.ActionContext);
        }
        private async Task<bool> IsAdmin()
        {
            var httpContext = httpContextAccessor.HttpContext;
            var result =  await authorizationService.AuthorizeAsync(httpContext.User, "isAdmin");

            return result.Succeeded;
        }
        public async Task GenerateLinks(AutorDTO autorDTO)
        {
            var isAdmin = await IsAdmin();
            var Url = BuildURLHelper();
            autorDTO.Links.Add(new DataHATEOAS(link: Url.Link("getAutor", new { id = autorDTO.Id }), description: "self", method: "GET"));

            if (isAdmin)
            {
                autorDTO.Links.Add(new DataHATEOAS(link: Url.Link("updateAutor", new { id = autorDTO.Id }), description: "create-autor", method: "PUT"));
                autorDTO.Links.Add(new DataHATEOAS(link: Url.Link("deleteAutor", new { id = autorDTO.Id }), description: "delet-autor", method: "DELETE"));
            }
        }
    }
}
