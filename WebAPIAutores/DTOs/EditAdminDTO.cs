using System.ComponentModel.DataAnnotations;

namespace WebAPIAutores.DTOs
{
    public class EditAdminDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
