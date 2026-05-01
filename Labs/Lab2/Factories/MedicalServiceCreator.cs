using DrDanielaVintu.Labs.Lab2.Models;

namespace DrDanielaVintu.Labs.Lab2.Factories
{
    /// <summary>
    /// Clasa Creator care declară metoda fabrică.
    /// </summary>
    public abstract class MedicalServiceCreator
    {
        // Factory Method
        public abstract IMedicalService CreateService();

        public string GetServiceInfo()
        {
            var service = CreateService();
            return $"Serviciu Creat: {service.GetDescription()} | Cost: {service.GetCost()} RON";
        }
    }

    /// <summary>
    /// Creator concret pentru Consultații.
    /// </summary>
    public class ConsultationCreator : MedicalServiceCreator
    {
        public override IMedicalService CreateService()
        {
            return new Consultation();
        }
    }

    /// <summary>
    /// Creator concret pentru Analize.
    /// </summary>
    public class LabTestCreator : MedicalServiceCreator
    {
        public override IMedicalService CreateService()
        {
            return new SpecializedLabTest();
        }
    }
}
