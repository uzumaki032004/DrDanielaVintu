namespace DrDanielaVintu.Labs.Lab2.Models
{
    public interface IMedicalService
    {
        string GetDescription();
        decimal GetCost();
    }

    public class Consultation : IMedicalService
    {
        public string GetDescription() => "Consultație Generală de Nutriție";
        public decimal GetCost() => 200;
    }

    public class SpecializedLabTest : IMedicalService
    {
        public string GetDescription() => "Analize de laborator specializate (Metabolism)";
        public decimal GetCost() => 450;
    }
}
