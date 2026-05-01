using System;

namespace DrDanielaVintu.Labs.Lab1.Models
{
    /// <summary>
    /// Reprezintă o programare.
    /// </summary>
    public class Appointment
    {
        public Client Client { get; set; }
        public MedicalService Service { get; set; }
        public DateTime DateTime { get; set; }
        public bool IsConfirmed { get; set; }
    }
}
