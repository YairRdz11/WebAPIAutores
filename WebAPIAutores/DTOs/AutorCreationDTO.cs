using System.ComponentModel.DataAnnotations;
using WebAPIAutores.Validations;

namespace WebAPIAutores.DTOs
{
    public class AutorCreationDTO
    {
        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(maximumLength: 120, ErrorMessage = "The field {0} must not have more than {1} charecters")]
        [CamelCaseValidation]
        public string Name { get; set; }
    }
}
