namespace DrDanielaVintu.Labs.Lab1.Models
{
    /// <summary>
    /// Reprezintă un plan nutrițional pe mai multe săptămâni.
    /// </summary>
    public class NutritionalPlan : MedicalService
    {
        public int DurationInWeeks { get; set; }

        public override decimal CalculateFinalPrice()
        {
            // Prețul crește în funcție de durata planului
            return BasePrice + (DurationInWeeks * 50);
        }
    }
}
