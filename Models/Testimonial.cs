using System;
using System.ComponentModel.DataAnnotations;

namespace DrDanielaVintu.Models
{
    public class Testimonial
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Numele clientului este obligatoriu")]
        public string ClientName { get; set; } = string.Empty;

        public string? ClientInitials { get; set; }

        [Required(ErrorMessage = "Textul testimonialului este obligatoriu")]
        [StringLength(1000, MinimumLength = 10)]
        public string Text { get; set; } = string.Empty;

        [Range(1, 5)]
        public int Rating { get; set; } = 5;

        public bool IsVisible { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
