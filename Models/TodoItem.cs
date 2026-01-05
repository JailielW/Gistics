using System;
using System.ComponentModel.DataAnnotations;

namespace Gistics.Models
{
    public class TodoItem
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(200, ErrorMessage = "Title must be at most 200 characters.")]
        public string? Title { get; set; }

        [StringLength(2000, ErrorMessage = "Description must be at most 2000 characters.")]
        public string? Description { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}