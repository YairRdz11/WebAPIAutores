using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.DTOs;
using WebAPIAutores.Entities;
using WebAPIAutores.Utilities;

namespace WebAPIAutores.Controllers.V1
{
    [Route("api/[controller]")]
    [HeaderIsPresent("x-version", "1")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "isAdmin")]
    public class AutorsController : ControllerBase
    {
        private readonly ApplicationDBContext context;
        private readonly IMapper mapper;
        private readonly IAuthorizationService authorizationService;

        public AutorsController(ApplicationDBContext context, IMapper mapper, IAuthorizationService authorizationService)
        {
            this.context = context;
            this.mapper = mapper;
            this.authorizationService = authorizationService;
        }

        [HttpGet(Name = "getAutorsv1")]
        [AllowAnonymous]
        [ServiceFilter(typeof(HATEOASAutorFilterAttribute))]
        public async Task<ActionResult<List<AutorDTO>>> Get([FromQuery] PaginationDTO paginationDTO)
        {
            var queryble = context.Autors.AsQueryable();
            await HttpContext.InsertParametersPaginationHeader(queryble);
            var autors = await queryble.OrderBy(x=>x.Name).Paginate(paginationDTO).ToListAsync();

            return mapper.Map<List<AutorDTO>>(autors);
        }

        [HttpGet("{id:int}", Name = "getAutorv1")]
        [AllowAnonymous]
        [ServiceFilter(typeof(HATEOASAutorFilterAttribute))]
        public async Task<ActionResult<AutorDTOWithBooks>> Get(int id)
        {
            var autor = await context.Autors
                .Include(x => x.AutorsBooks)
                .ThenInclude(x => x.Book)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (autor is null)
            {
                return NotFound();
            }

            var dto = mapper.Map<AutorDTOWithBooks>(autor);
            var isAdmin = await authorizationService.AuthorizeAsync(User, "isAdmin");

            //GenerateLinks(dto, isAdmin.Succeeded);

            return dto;
        }

        [HttpGet("{name}", Name = "getAutorByNamev1")]
        public async Task<ActionResult<List<AutorDTO>>> GetByName(string name)
        {
            var autors = await context.Autors.Where(x => x.Name.Contains(name)).ToListAsync();

            return mapper.Map<List<AutorDTO>>(autors);
        }

        [HttpPost(Name = "createAutorv1")]
        public async Task<ActionResult> Post(AutorCreationDTO autorCreationDTO)
        {
            var autorExisted = await context.Autors.AnyAsync(x => x.Name.ToLower() == autorCreationDTO.Name.ToLower());
            if (autorExisted)
            {
                return BadRequest($"The autor {autorCreationDTO.Name} already exists");
            }

            var autor = mapper.Map<Autor>(autorCreationDTO);
            context.Add(autor);

            await context.SaveChangesAsync();

            var autorDto = mapper.Map<AutorDTO>(autor);

            return CreatedAtRoute("getAutorv1", new { id = autor.Id }, autorDto);
        }

        [HttpPut("{id:int}", Name = "updateAutorv1")]
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
        /// <summary>
        /// Delete an autor
        /// </summary>
        /// <param name="id">autor id to delete</param>
        /// <returns></returns>
        [HttpDelete("{id:int}", Name = "deleteAutorv1")]
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
