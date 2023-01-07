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
            
            var autorDTO = result.Value as AutorDTO;
            if(autorDTO == null)
            {
                var autorsDTO = result.Value as List<AutorDTO> ?? throw new ArgumentNullException("Need to be an instance of AutorDTO or List<AutorDTO>");

                autorsDTO.ForEach(async autorDTO => await linksGenerator.GenerateLinks(autorDTO));

                result.Value = autorsDTO;
            }
            else
            {
                await linksGenerator.GenerateLinks(autorDTO);
            }
            await next();
        }
    }
}
