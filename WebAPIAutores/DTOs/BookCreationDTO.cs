using System.ComponentModel.DataAnnotations;
using WebAPIAutores.Validations;

namespace WebAPIAutores.DTOs
{
    public class BookCreationDTO
    {
        [CamelCaseValidation]
        [StringLength(maximumLength: 250)]
        public string Title { get; set; }
        public List<int> AutorIds { get; set; }
    }
}
