using System;

namespace DrDanielaVintu.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string Author { get; set; } = "Dr. Daniela Vîntu";
        public string? Tags { get; set; }
        public string Category { get; set; } = "General";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
