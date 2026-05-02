using System.Collections.Generic;
namespace DrDanielaVintu.Models {
 public class SubscriptionPlan {
 public int Id { get; set; }
 public string Name { get; set; } = string.Empty;
 public decimal Price { get; set; }
 public string Benefits { get; set; } = string.Empty; // Semicolon separated
 public bool IsPopular { get; set; }
 }
}
