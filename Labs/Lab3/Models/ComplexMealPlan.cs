using System.Collections.Generic;

namespace DrDanielaVintu.Labs.Lab3.Models
{
    /// <summary>
    /// Produsul complex ce va fi construit.
    /// </summary>
    public class ComplexMealPlan
    {
        public string Name { get; set; }
        public int DailyCalories { get; set; }
        public List<string> Meals { get; set; } = new List<string>();
        public List<string> Exclusions { get; set; } = new List<string>();
        public bool IncludeSupplementPlan { get; set; }

        public override string ToString()
        {
            return $"Plan: {Name} ({DailyCalories} kcal) | Mese: {string.Join(", ", Meals)} | Excluderi: {string.Join(", ", Exclusions)} | Suplimente: {(IncludeSupplementPlan ? "DA" : "NU")}";
        }
    }
}
