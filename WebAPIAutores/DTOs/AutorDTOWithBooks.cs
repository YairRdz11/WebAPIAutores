namespace WebAPIAutores.DTOs
{
    public class AutorDTOWithBooks : AutorDTO
    {
        public List<BookDTO> Books { get; set; }
    }
}
