using System;
namespace DrDanielaVintu.Models {
 public class NewsItem {
 public int Id { get; set; }
 public string Title { get; set; } = string.Empty;
 public string Description { get; set; } = string.Empty;
 public DateTime? EventDate { get; set; }
 public bool IsPremiumOnly { get; set; }
 }
}
