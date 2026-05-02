using System;
using System.ComponentModel.DataAnnotations;

namespace DrDanielaVintu.Models
{
    public class ContactMessage
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Numele este obligatoriu")]
        [Display(Name = "Nume Complet")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email-ul este obligatoriu")]
        [EmailAddress(ErrorMessage = "Adresa de email nu este validă")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Telefon (opțional)")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Subiectul este obligatoriu")]
        public string Subject { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mesajul este obligatoriu")]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "Mesajul trebuie să aibă între 10 și 2000 de caractere")]
        public string Message { get; set; } = string.Empty;

        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;
    }
}
