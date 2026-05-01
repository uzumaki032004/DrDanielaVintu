using System;

namespace DrDanielaVintu.Labs.Lab3.Singletons
{
    /// <summary>
    /// Implementare Singleton Thread-safe.
    /// </summary>
    public sealed class ClinicSettings
    {
        private static ClinicSettings _instance = null;
        private static readonly object _lock = new object();

        public string ClinicName { get; set; }
        public string AdminEmail { get; set; }
        public DateTime InitializedAt { get; }

        private ClinicSettings()
        {
            // Constructor privat pentru a preveni instanțierea externă
            ClinicName = "Clinica Dr. Daniela Vintu";
            AdminEmail = "contact@daniela-vintu.ro";
            InitializedAt = DateTime.Now;
        }

        public static ClinicSettings Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new ClinicSettings();
                    }
                    return _instance;
                }
            }
        }
    }
}
