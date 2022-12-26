namespace WebAPIAutores.DTOs
{
    public class BookDTOWithAutors : BookDTO
    {
        public List<AutorDTO> Autors { get; set; }
    }
}
