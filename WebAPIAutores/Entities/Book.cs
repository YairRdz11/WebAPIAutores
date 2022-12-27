using System.ComponentModel.DataAnnotations;
using WebAPIAutores.Validations;

namespace WebAPIAutores.Entities
{
    public class Book
    {
        public int Id { get; set; }
        [Required]
        [CamelCaseValidation]
        [StringLength(maximumLength: 250)]
        public string Title { get; set; }
        public DateTime? PublishDate { get; set; }
        public List<Comment> Comments { get; set; }
        public List<AutorBook> AutorsBooks { get; set; }
    }
}
