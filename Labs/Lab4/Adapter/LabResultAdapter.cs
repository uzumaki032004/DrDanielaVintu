namespace DrDanielaVintu.Labs.Lab4.Adapter
{
    /// <summary>
    /// Interfața pe care o folosește clinica noastră.
    /// </summary>
    public interface IClinicReport
    {
        string GetHeader();
        string GetResults();
    }

    /// <summary>
    /// Sistem extern (Adaptee) cu o interfață diferită.
    /// </summary>
    public class ExternalLabSystem
    {
        public string GetRawData()
        {
            return "ID_EXT:123; VAL:Glucose=110,Iron=75; STATUS:OK";
        }
    }

    /// <summary>
    /// Adaptorul care face conversia.
    /// </summary>
    public class LabResultAdapter : IClinicReport
    {
        private readonly ExternalLabSystem _externalSystem;

        public LabResultAdapter(ExternalLabSystem externalSystem)
        {
            _externalSystem = externalSystem;
        }

        public string GetHeader()
        {
            var data = _externalSystem.GetRawData();
            var id = data.Split(';')[0].Split(':')[1];
            return $"Raport Clinic - Sursa Externă (ID: {id})";
        }

        public string GetResults()
        {
            var data = _externalSystem.GetRawData();
            var val = data.Split(';')[1].Split(':')[1];
            return $"Rezultate prelucrate: {val.Replace(",", " | ")}";
        }
    }
}
