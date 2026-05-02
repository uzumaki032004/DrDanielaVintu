using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DrDanielaVintu.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Numele este obligatoriu")]
        [Display(Name = "Nume Client")]
        public string ClientName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email-ul este obligatoriu")]
        [EmailAddress(ErrorMessage = "Email invalid")]
        public string ClientEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Telefonul este obligatoriu")]
        [Display(Name = "Telefon")]
        public string ClientPhone { get; set; } = string.Empty;

        [Required]
        public int PlanId { get; set; }
        
        [ForeignKey("PlanId")]
        public SubscriptionPlan? Plan { get; set; }

        [Required(ErrorMessage = "Vă rugăm selectați o dată preferată")]
        [Display(Name = "Data Preferată")]
        [DataType(DataType.Date)]
        public DateTime PreferredDate { get; set; }

        [Display(Name = "Mesaj Suplimentar")]
        public string? Message { get; set; }

        public string Status { get; set; } = "Pending"; // Pending, Confirmed, Cancelled
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
