﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.DTOs;
using WebAPIAutores.Entities;

namespace WebAPIAutores.Controllers
{
    [Route("api/books/{bookId:int}/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ApplicationDBContext context;
        private readonly IMapper mapper;

        public CommentsController(ApplicationDBContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<List<CommentDTO>>> Get(int bookId)
        {
            var book = await context.Books.AnyAsync(x => x.Id == bookId);

            if (!book)
            {
                return NotFound();
            }

            var comments = await context.Comments
                .Where(x=> x.BookId== bookId)
                .ToListAsync();

            return mapper.Map<List<CommentDTO>>(comments);
        }

        [HttpGet("{id:int}", Name = "getComment")]
        public async Task<ActionResult<CommentDTO>> GetById(int id)
        {
            var comment = await context.Comments.FirstOrDefaultAsync(x => x.Id == id);

            if(comment == null)
            {
                return NotFound();
            }

            return mapper.Map<CommentDTO>(comment);
        }

        [HttpPost]
        public async Task<ActionResult> Post(int bookId, CommentCreationDTO commentCreationDto)
        {
            var book = await context.Books.AnyAsync(x =>x.Id == bookId);

            if (!book)
            {
                return NotFound();
            }

            var comment = mapper.Map<Comment>(commentCreationDto);
            comment.BookId= bookId;

            context.Add(comment);
            await context.SaveChangesAsync();

            var commentDto = mapper.Map<CommentDTO>(comment);

            return CreatedAtRoute("getComment", new { id = comment.Id, bookId = bookId }, commentDto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int bookId, int id, CommentCreationDTO commentCreationDTO)
        {
            var book = await context.Books.AnyAsync(x => x.Id == bookId);

            if (!book)
            {
                return NotFound();
            }

            var commentExisted = await context.Comments.AnyAsync(x => x.Id == id);

            if(!commentExisted)
            {
                return NotFound();
            }

            var comment = mapper.Map<Comment>(commentCreationDTO);
            comment.Id = id;
            comment.BookId= bookId;
            context.Update(comment);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
