using Microsoft.AspNetCore.Mvc;
using WebAPIAutores.DTOs;

namespace WebAPIAutores.Controllers
{
    [Route("api/")]
    [ApiController]
    public class RootController : ControllerBase
    {
        [HttpGet(Name = "GetRoot")]
        public ActionResult<IEnumerable<DataHATEOAS>> Get()
        {
            var dataHateoas = new List<DataHATEOAS>();

            dataHateoas.Add(new DataHATEOAS(link: Url.Link("GetRoot", new { }), description: "self", method: "GET"));

            return dataHateoas;
        }
    }
}
