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

            CreateMap<BookCreationDTO, Book>()
                .ForMember(x => x.AutorsBooks, options => options.MapFrom(MapAutorsBook));
            CreateMap<Book, BookDTO>();

            CreateMap<CommentCreationDTO, Comment>();
            CreateMap<Comment, CommentDTO>();
        }

        private List<AutorBook> MapAutorsBook(BookCreationDTO bookCreationDTO, Book book)
        {
            var result = new List<AutorBook>();

            if(bookCreationDTO.AutorIds == null)
            {
                return result;
            }

            foreach(var autorId in bookCreationDTO.AutorIds)
            {
                result.Add(new AutorBook() { AutorId = autorId });
            }
            return result;
        }
    }
}
