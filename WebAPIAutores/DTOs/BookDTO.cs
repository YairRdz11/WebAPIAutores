using System.ComponentModel.DataAnnotations;
using WebAPIAutores.Validations;

namespace WebAPIAutores.DTOs
{
    public class BookDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime PublishDate { get; set; }
    }
}
