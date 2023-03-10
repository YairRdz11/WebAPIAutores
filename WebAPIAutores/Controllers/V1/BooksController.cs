using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.DTOs;
using WebAPIAutores.Entities;

namespace WebAPIAutores.Controllers.V1
{
    [Route("api/v1/[controller]")]
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

        [HttpGet("{id:int}", Name = "getBook")]
        public async Task<ActionResult<BookDTOWithAutors>> Get(int id)
        {
            var book = await context.Books
                .Include(x => x.AutorsBooks)
                .ThenInclude(x => x.Autor)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            book.AutorsBooks = book.AutorsBooks.OrderBy(x => x.Order).ToList();

            return mapper.Map<BookDTOWithAutors>(book);
        }

        [HttpPost(Name = "createBook")]
        public async Task<ActionResult> Post(BookCreationDTO bookCreationDto)
        {
            if (bookCreationDto.AutorIds == null)
            {
                return BadRequest("You can not create a book without autors");
            }

            var autorIds = await context.Autors
                .Where(x => bookCreationDto.AutorIds.Contains(x.Id))
                .Select(x => x.Id)
                .ToListAsync();
            if (autorIds.Count != bookCreationDto.AutorIds.Count)
            {
                return BadRequest("One of the autors sent doesn't exist");
            }

            var book = mapper.Map<Book>(bookCreationDto);

            AssignOrderAutors(book);

            context.Add(book);
            await context.SaveChangesAsync();

            var bookDto = mapper.Map<BookDTO>(book);

            return CreatedAtRoute("getBook", new { id = book.Id }, bookDto);
        }

        [HttpPut("{id:int}", Name = "updateBook")]
        public async Task<ActionResult> Put(int id, BookCreationDTO bookCreationDTO)
        {
            var bookDB = await context.Books
                .Include(x => x.AutorsBooks)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (bookDB == null)
            {
                return NotFound();
            }

            bookDB = mapper.Map(bookCreationDTO, bookDB);
            AssignOrderAutors(bookDB);
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id:int}", Name = "patchBook")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<BookPatchDTO> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var bookDB = await context.Books.FirstOrDefaultAsync(x => x.Id == id);
            if (bookDB == null)
            {
                return NotFound();
            }

            var bookDTO = mapper.Map<BookPatchDTO>(bookDB);

            patchDocument.ApplyTo(bookDTO, ModelState);
            var isValid = TryValidateModel(bookDTO);

            if (!isValid)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(bookDTO, bookDB);
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}", Name = "deleteBook")]
        public async Task<ActionResult> Delete(int id)
        {
            var bookExisted = await context.Books.AnyAsync(x => x.Id == id);
            if (!bookExisted)
            {
                return NotFound();
            }

            context.Remove(new Book { Id = id });

            await context.SaveChangesAsync();
            return NoContent();
        }

        private void AssignOrderAutors(Book book)
        {
            if (book.AutorsBooks != null)
            {
                for (int i = 0; i < book.AutorsBooks.Count; i++)
                {
                    book.AutorsBooks[i].Order = i;
                }
            }
        }
    }
}
