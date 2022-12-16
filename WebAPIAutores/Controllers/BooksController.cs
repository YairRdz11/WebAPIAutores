using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.Entities;

namespace WebAPIAutores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly ApplicationDBContext context;

        public BooksController(ApplicationDBContext context)
        {
            this.context = context;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Book>> Get(int id)
        {
            return await context.Books.Include(x => x.Autor).FirstOrDefaultAsync(x => x.Id == id);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Book book)
        {
            var autorExisted = await context.Autors.AnyAsync(x => x.Id == book.AutorId);
            if (!autorExisted)
            {
                return BadRequest($"Autor id not found of: {book.AutorId}");
            }

            context.Add(book);
            await context.SaveChangesAsync();

            return Ok();
        }
    }
}
