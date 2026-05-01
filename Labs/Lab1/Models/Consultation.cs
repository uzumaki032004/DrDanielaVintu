namespace DrDanielaVintu.Labs.Lab1.Models
{
    /// <summary>
    /// Reprezintă o consultație simplă.
    /// </summary>
    public class Consultation : MedicalService
    {
        public bool IsOnline { get; set; }

        public override decimal CalculateFinalPrice()
        {
            // Dacă e online, oferim o reducere de 10%
            return IsOnline ? BasePrice * 0.9m : BasePrice;
        }
    }
}
