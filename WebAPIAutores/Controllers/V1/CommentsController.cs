using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.DTOs;
using WebAPIAutores.Entities;
using WebAPIAutores.Utilities;

namespace WebAPIAutores.Controllers.V1
{
    [Route("api/v1/books/{bookId:int}/[controller]/")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ApplicationDBContext context;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;

        public CommentsController(ApplicationDBContext context, IMapper mapper, UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
        }
        [HttpGet(Name = "getCommentsBook")]
        public async Task<ActionResult<List<CommentDTO>>> Get(int bookId, [FromQuery] PaginationDTO paginationDTO)
        {
            var book = await context.Books.AnyAsync(x => x.Id == bookId);

            if (!book)
            {
                return NotFound();
            }

            var queryable = context.Comments.Where(x => x.BookId == bookId).AsQueryable();
            await HttpContext.InsertParametersPaginationHeader(queryable);

            var comments = await queryable
                .OrderBy(x => x.Id)
                .Paginate(paginationDTO)
                .ToListAsync();

            return mapper.Map<List<CommentDTO>>(comments);
        }

        [HttpGet("{id:int}", Name = "getComment")]
        public async Task<ActionResult<CommentDTO>> GetById(int id)
        {
            var comment = await context.Comments.FirstOrDefaultAsync(x => x.Id == id);

            if (comment == null)
            {
                return NotFound();
            }

            return mapper.Map<CommentDTO>(comment);
        }

        [HttpPost(Name = "createComment")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post(int bookId, CommentCreationDTO commentCreationDto)
        {
            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;
            var user = await userManager.FindByEmailAsync(email);
            var userId = user.Id;

            var book = await context.Books.AnyAsync(x => x.Id == bookId);

            if (!book)
            {
                return NotFound();
            }

            var comment = mapper.Map<Comment>(commentCreationDto);
            comment.BookId = bookId;
            comment.UserId = userId;
            context.Add(comment);
            await context.SaveChangesAsync();

            var commentDto = mapper.Map<CommentDTO>(comment);

            return CreatedAtRoute("getComment", new { id = comment.Id, bookId }, commentDto);
        }

        [HttpPut("{id:int}", Name = "updateComment")]
        public async Task<ActionResult> Put(int bookId, int id, CommentCreationDTO commentCreationDTO)
        {
            var book = await context.Books.AnyAsync(x => x.Id == bookId);

            if (!book)
            {
                return NotFound();
            }

            var commentExisted = await context.Comments.AnyAsync(x => x.Id == id);

            if (!commentExisted)
            {
                return NotFound();
            }

            var comment = mapper.Map<Comment>(commentCreationDTO);
            comment.Id = id;
            comment.BookId = bookId;
            context.Update(comment);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
