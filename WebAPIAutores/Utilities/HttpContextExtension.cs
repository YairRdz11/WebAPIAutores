using Microsoft.EntityFrameworkCore;

namespace WebAPIAutores.Utilities
{
    public static class HttpContextExtension
    {
        public async static Task InsertParametersPaginationHeader<T>(this HttpContext httpContext, IQueryable<T> queryParameters)
        {
            if (httpContext == null) throw new ArgumentNullException(nameof(httpContext));

            double amount = await queryParameters.CountAsync();
            httpContext.Request.Headers.Add("amountTotalRecord", amount.ToString());
        }
    }
}
