namespace DrDanielaVintu.Labs.Lab1.Models
{
    /// <summary>
    /// Clasă de bază abstractă pentru servicii.
    /// Demonstrează MOȘTENIREA și POLIMORFISMUL (prin metoda abstractă).
    /// </summary>
    public abstract class MedicalService
    {
        public string Name { get; set; }
        public decimal BasePrice { get; set; }

        // Metodă polimorfică
        public abstract decimal CalculateFinalPrice();

        public override string ToString()
        {
            return $"{Name} - Preț: {CalculateFinalPrice()} RON";
        }
    }
}
