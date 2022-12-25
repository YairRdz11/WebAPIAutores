namespace WebAPIAutores.Entities
{
    public class AutorBook
    {
        public int BookId { get; set; }
        public int AutorId { get; set; }
        public int Order { get; set; }
        public Book Book { get; set; }
        public Autor Autor { get; set; }
    }
}
