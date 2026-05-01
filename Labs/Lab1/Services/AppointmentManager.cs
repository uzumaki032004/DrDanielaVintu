using System;
using DrDanielaVintu.Labs.Lab1.Interfaces;
using DrDanielaVintu.Labs.Lab1.Models;

namespace DrDanielaVintu.Labs.Lab1.Services
{
    /// <summary>
    /// Gestionează logica programărilor.
    /// Demonstrează:
    /// - SRP: Are o singură responsabilitate (managementul programărilor).
    /// - DIP: Depinde de interfața INotificationService, nu de implementarea concretă.
    /// - OCP & LSP: Poate lucra cu orice tip de MedicalService.
    /// </summary>
    public class AppointmentManager
    {
        private readonly INotificationService _notifier;

        // Dependency Injection prin Constructor
        public AppointmentManager(INotificationService notifier)
        {
            _notifier = notifier;
        }

        public void ProcessAppointment(Appointment appointment)
        {
            // Validare de bază
            if (appointment == null) throw new ArgumentNullException(nameof(appointment));

            // Confirmare logică
            appointment.IsConfirmed = true;

            // Calculare preț (Polimorfism - LSP)
            decimal price = appointment.Service.CalculateFinalPrice();

            // Notificare client
            string message = $"Bună ziua, {appointment.Client.FullName}! Programarea pentru '{appointment.Service.Name}' la data de {appointment.DateTime} a fost confirmată. Preț total: {price} RON.";
            
            _notifier.SendNotification(appointment.Client.Email, message);

            Console.WriteLine($"[LOG] Programare procesată pentru {appointment.Client.FullName}.");
        }
    }
}
