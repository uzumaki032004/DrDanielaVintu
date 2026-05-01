using System;
using DrDanielaVintu.Labs.Lab1.Interfaces;

namespace DrDanielaVintu.Labs.Lab1.Services
{
    /// <summary>
    /// Implementare concretă pentru notificări prin Email.
    /// </summary>
    public class EmailNotificationService : INotificationService
    {
        public void SendNotification(string recipient, string message)
        {
            // Simulare trimitere email
            Console.WriteLine($"[EMAIL] Către: {recipient} | Mesaj: {message}");
        }
    }
}
