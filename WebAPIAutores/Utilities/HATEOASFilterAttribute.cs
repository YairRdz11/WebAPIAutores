using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPIAutores.Utilities
{
    public class HATEOASFilterAttribute : ResultFilterAttribute
    {
        protected bool HasHATEOAS(ResultExecutingContext context)
        {
            var result = context.Result as ObjectResult;

            if (!IsSuccess(result)) { return false; }

            var header = context.HttpContext.Request.Headers["HATEOASIncluded"];

            if(header.Count == 0) { return false; }

            var valor = header[0];

            if(!valor.Equals("Y", StringComparison.OrdinalIgnoreCase)) { return false; }

            return true;
        }

        private bool IsSuccess(ObjectResult result)
        {
            if(result == null || result.Value == null)
            {
                return false;
            }
            if(result.StatusCode.HasValue && !result.StatusCode.Value.ToString().StartsWith("2"))
            {
                return false;
            }

            return true;
        }
    }
}
