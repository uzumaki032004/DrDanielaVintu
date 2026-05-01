using System;

namespace DrDanielaVintu.Labs.Lab3.Models
{
    /// <summary>
    /// Interfața Prototype.
    /// </summary>
    public interface IMealPrototype
    {
        IMealPrototype Clone();
    }

    public class PatientRecord : IMealPrototype
    {
        public string PatientName { get; set; }
        public string Diagnosis { get; set; }
        public DateTime LastVisit { get; set; }

        public IMealPrototype Clone()
        {
            // Shallow copy - suficient pentru tipuri de bază
            return (IMealPrototype)this.MemberwiseClone();
        }

        public override string ToString()
        {
            return $"Pacient: {PatientName} | Diagnostic: {Diagnosis} | Vizită: {LastVisit.ToShortDateString()}";
        }
    }
}
