using Microsoft.AspNetCore.Mvc;
using DrDanielaVintu.Labs.Lab1.Models;
using DrDanielaVintu.Labs.Lab1.Services;
using System;

namespace DrDanielaVintu.Controllers
{
    public class Lab1Controller : Controller
    {
        public IActionResult Index()
        {
            // 1. Inițializare servicii (DIP)
            var emailService = new EmailNotificationService();
            var manager = new AppointmentManager(emailService);

            // 2. Creare Client (Encapsulare)
            var client = new Client
            {
                FirstName = "Ion",
                LastName = "Popescu",
                Email = "ion.popescu@gmail.com",
                PhoneNumber = "0722123456"
            };

            // 3. Creare Servicii (Moștenire & Polimorfism)
            var consultation = new Consultation
            {
                Name = "Consultație Nutriție Inițială",
                BasePrice = 200,
                IsOnline = true // Va aplica reducere de 10%
            };

            var plan = new NutritionalPlan
            {
                Name = "Plan Personalizat Slăbire",
                BasePrice = 500,
                DurationInWeeks = 4 // Va adăuga 4 * 50 RON
            };

            // 4. Creare Programări
            var app1 = new Appointment
            {
                Client = client,
                Service = consultation,
                DateTime = DateTime.Now.AddDays(2)
            };

            var app2 = new Appointment
            {
                Client = client,
                Service = plan,
                DateTime = DateTime.Now.AddDays(7)
            };

            // 5. Procesare (Demonstrare SOLID)
            manager.ProcessAppointment(app1);
            manager.ProcessAppointment(app2);

            ViewBag.Message = "Laborator 1 executat cu succes! Verifică consola (Output) pentru detalii despre procesare.";
            return View();
        }
    }
}
