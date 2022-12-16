using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.Entities;

namespace WebAPIAutores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutorsController : ControllerBase
    {
        private readonly ApplicationDBContext context;
        public AutorsController(ApplicationDBContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Autor>>> Get()
        {
            return await context.Autors.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> Post(Autor autor)
        {
            var autorExisted = await context.Autors.AnyAsync(x => x.Name.ToLower() == autor.Name.ToLower());
            if(autorExisted)
            {
                return BadRequest($"The autor {autor.Name} already exists");
            }
            context.Add(autor);

            await context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, Autor autor)
        {
            if (autor.Id != id)
            {
                return BadRequest("The Id doesn't match with id url");
            }

            var autorExisted = await context.Autors.AnyAsync(x => x.Id == id);
            if (!autorExisted)
            {
                return NotFound();
            }

            context.Update(autor);
            await context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var autorExisted = await context.Autors.AnyAsync(x => x.Id == id);
            if (!autorExisted)
            {
                return NotFound();
            }

            context.Remove(new Autor { Id = id });
            
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
