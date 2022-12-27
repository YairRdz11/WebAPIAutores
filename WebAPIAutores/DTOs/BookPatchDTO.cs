using System.ComponentModel.DataAnnotations;
using WebAPIAutores.Validations;

namespace WebAPIAutores.DTOs
{
    public class BookPatchDTO
    {
        [Required]
        [CamelCaseValidation]
        [StringLength(maximumLength: 250)]
        public string Title { get; set; }
        public DateTime PublishDate { get; set; }
    }
}
