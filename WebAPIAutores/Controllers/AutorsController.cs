using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.DTOs;
using WebAPIAutores.Entities;

namespace WebAPIAutores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutorsController : ControllerBase
    {
        private readonly ApplicationDBContext context;
        private readonly IMapper mapper;

        public AutorsController(ApplicationDBContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<AutorDTO>>> Get()
        {
            var autors = await context.Autors.ToListAsync();

            return mapper.Map<List<AutorDTO>>(autors);
        }

        [HttpGet("{id:int}", Name = "getAutor")]
        public async Task<ActionResult<AutorDTOWithBooks>> Get(int id)
        {
            var autor = await context.Autors
                .Include(x => x.AutorsBooks)
                .ThenInclude(x => x.Book)
                .FirstOrDefaultAsync(x => x.Id == id);

            if(autor is null)
            {
                return NotFound();
            }

            return mapper.Map<AutorDTOWithBooks>(autor);
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<List<AutorDTO>>> Get(string name)
        {
            var autors = await context.Autors.Where(x => x.Name.Contains(name)).ToListAsync();

            return mapper.Map<List<AutorDTO>>(autors);
        }

        [HttpPost]
        public async Task<ActionResult> Post(AutorCreationDTO autorCreationDTO)
        {
            var autorExisted = await context.Autors.AnyAsync(x => x.Name.ToLower() == autorCreationDTO.Name.ToLower());
            if(autorExisted)
            {
                return BadRequest($"The autor {autorCreationDTO.Name} already exists");
            }

            var autor = mapper.Map<Autor>(autorCreationDTO);
            context.Add(autor);

            await context.SaveChangesAsync();

            var autorDto = mapper.Map<AutorDTO>(autor);

            return CreatedAtRoute("getAutor", new { id = autor.Id }, autorDto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, AutorCreationDTO autorCreationDTO)
        {
            var autorExisted = await context.Autors.AnyAsync(x => x.Id == id);
            if (!autorExisted)
            {
                return NotFound();
            }

            var autor = mapper.Map<Autor>(autorCreationDTO);
            autor.Id = id;

            context.Update(autor);
            await context.SaveChangesAsync();

            return NoContent();
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
            return NoContent();
        }
    }
}
