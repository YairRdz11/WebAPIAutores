using AutoMapper;
using WebAPIAutores.DTOs;
using WebAPIAutores.Entities;

namespace WebAPIAutores.Utilities
{
    public class AutomapperProfiles: Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<AutorCreationDTO, Autor>();
            CreateMap<Autor, AutorDTO>();

            CreateMap<BookCreationDTO, Book>();
            CreateMap<Book, BookDTO>();

            CreateMap<CommentCreationDTO, Comment>();
            CreateMap<Comment, CommentDTO>();
        }
    }
}
