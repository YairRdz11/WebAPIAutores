﻿using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.DTOs;
using WebAPIAutores.Entities;

namespace WebAPIAutores.Controllers
{
    [Route("api/[controller]")]
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

        [HttpGet(Name = "getAutors")]
        [AllowAnonymous]
        public async Task<IActionResult> Get([FromQuery] bool HATEOASIncluded = true)
        {
            var autors = await context.Autors.ToListAsync();
            var dtos = mapper.Map<List<AutorDTO>>(autors);


            if (HATEOASIncluded)
            {
                var isAdmin = await authorizationService.AuthorizeAsync(User, "isAdmin");
                dtos.ForEach(x => GenerateLinks(x, isAdmin.Succeeded));
                var result = new CollectionResources<AutorDTO> { Values = dtos };
                result.Links.Add(new DataHATEOAS(link: Url.Link("getAutors", new { }), description: "self", method: "GET"));
                if (isAdmin.Succeeded)
                {
                    result.Links.Add(new DataHATEOAS(link: Url.Link("createAutor", new { }), description: "create-autor", method: "POST"));
                }

                return Ok(result);
            }


            return Ok(dtos);
        }

        [HttpGet("{id:int}", Name = "getAutor")]
        [AllowAnonymous]
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

            var dto = mapper.Map<AutorDTOWithBooks>(autor);
            var isAdmin = await authorizationService.AuthorizeAsync(User, "isAdmin");

            GenerateLinks(dto, isAdmin.Succeeded);

            return dto;
        }

        [HttpGet("{name}", Name = "getAutorByName")]
        public async Task<ActionResult<List<AutorDTO>>> Get(string name)
        {
            var autors = await context.Autors.Where(x => x.Name.Contains(name)).ToListAsync();

            return mapper.Map<List<AutorDTO>>(autors);
        }

        [HttpPost(Name = "createAutor")]
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

        [HttpPut("{id:int}", Name = "updateAutor")]
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

        [HttpDelete("{id:int}", Name = "deleteAutor")]
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

        private void GenerateLinks(AutorDTO autorDTO, bool isAdmin)
        {
            autorDTO.Links.Add(new DataHATEOAS(link: Url.Link("getAutor", new { id = autorDTO.Id }), description: "self", method: "GET"));

            if (isAdmin)
            {
                autorDTO.Links.Add(new DataHATEOAS(link: Url.Link("updateAutor", new { id = autorDTO.Id }), description: "create-autor", method: "PUT"));
                autorDTO.Links.Add(new DataHATEOAS(link: Url.Link("deleteAutor", new { id = autorDTO.Id }), description: "delet-autor", method: "DELETE"));
            }
        }
    }
}
