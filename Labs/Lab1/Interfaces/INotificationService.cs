namespace DrDanielaVintu.Labs.Lab1.Interfaces
{
    /// <summary>
    /// Interfață pentru servicii de notificare.
    /// Demonstrează ISP și DIP (clasele vor depinde de interfață).
    /// </summary>
    public interface INotificationService
    {
        void SendNotification(string recipient, string message);
    }

    /// <summary>
    /// Exemplu de ISP: Dacă avem nevoie de SMS separat, putem defini o altă interfață.
    /// </summary>
    public interface ISmsService
    {
        void SendSms(string phoneNumber, string text);
    }
}
