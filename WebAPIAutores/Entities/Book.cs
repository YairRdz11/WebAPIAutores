﻿using System.ComponentModel.DataAnnotations;
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
        public List<Comment> Comments { get; set; }
    }
}
