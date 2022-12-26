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
            CreateMap<Autor, AutorDTO>()
                .ForMember(x => x.Books, options => options.MapFrom(MapAutorDTOBooks));

            CreateMap<BookCreationDTO, Book>()
                .ForMember(x => x.AutorsBooks, options => options.MapFrom(MapAutorsBook));
            CreateMap<Book, BookDTO>()
                .ForMember(x=>x.Autors, options => options.MapFrom(MapBookDTOAutors));

            CreateMap<CommentCreationDTO, Comment>();
            CreateMap<Comment, CommentDTO>();
        }

        private List<BookDTO> MapAutorDTOBooks(Autor autor, AutorDTO autorDTO)
        {
            var result = new List<BookDTO>();

            if (autor.AutorsBooks == null)
            {
                return result;
            }

            foreach (var autorBook in autor.AutorsBooks)
            {
                result.Add(new BookDTO() { Id = autorBook.BookId, Title = autorBook.Book.Title });
            }

            return result;
        }

        private List<AutorDTO> MapBookDTOAutors(Book book, BookDTO bookDTO)
        {
            var result = new List<AutorDTO>();

            if(book.AutorsBooks == null)
            {
                return result;
            }

            foreach(var autorBook in book.AutorsBooks)
            {
                result.Add(new AutorDTO() { Id = autorBook.AutorId, Name = autorBook.Autor.Name });
            }

            return result;
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
