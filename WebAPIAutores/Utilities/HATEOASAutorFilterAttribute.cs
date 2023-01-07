using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAPIAutores.DTOs;
using WebAPIAutores.Services;

namespace WebAPIAutores.Utilities
{
    public class HATEOASAutorFilterAttribute : HATEOASFilterAttribute
    {
        private readonly LinksGenerator linksGenerator;

        public HATEOASAutorFilterAttribute(LinksGenerator linksGenerator)
        {
            this.linksGenerator = linksGenerator;
        }
        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var hasHATEOAS = HasHATEOAS(context);

            if (!hasHATEOAS)
            {
                await next();
                return;
            }

            var result = context.Result as ObjectResult;
            var model = result.Value as AutorDTO 
                ?? throw new ArgumentNullException("We need an instance of AutorDTO");
            await linksGenerator.GenerateLinks(model);
            await next();
        }
    }
}
