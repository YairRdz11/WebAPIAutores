using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.DTOs;
using WebAPIAutores.Entities;

namespace WebAPIAutores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly ApplicationDBContext context;
        private readonly IMapper mapper;

        public BooksController(ApplicationDBContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BookDTO>> Get(int id)
        {
            var book = await context.Books.FirstOrDefaultAsync(x => x.Id == id);

            return mapper.Map<BookDTO>(book);
        }

        [HttpPost]
        public async Task<ActionResult> Post(BookCreationDTO bookCreationDto)
        {
            //var autorExisted = await context.Autors.AnyAsync(x => x.Id == book.AutorId);
            //if (!autorExisted)
            //{
            //    return BadRequest($"Autor id not found of: {book.AutorId}");
            //}
            var book = mapper.Map<Book>(bookCreationDto);
            context.Add(book);
            await context.SaveChangesAsync();

            return Ok();
        }
    }
}
