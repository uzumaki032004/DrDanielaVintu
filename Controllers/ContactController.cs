using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.UI.Services;
using DrDanielaVintu.Data;
using DrDanielaVintu.Models;
using Microsoft.EntityFrameworkCore;

namespace DrDanielaVintu.Controllers
{
    public class ContactController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;

        public ContactController(ApplicationDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMessage(ContactMessage model)
        {
            if (ModelState.IsValid)
            {
                // 1. Save to DB
                model.SentAt = DateTime.UtcNow;
                model.IsRead = false;
                _context.ContactMessages.Add(model);
                await _context.SaveChangesAsync();

                // 2. Fetch Admin Email from SiteSettings
                var adminEmail = await _context.SiteSettings
                    .Where(s => s.Key == "Contact_Email")
                    .Select(s => s.Value)
                    .FirstOrDefaultAsync() ?? "micleusanudumitru@gmail.com";

                // 3. Send Email to Admin
                var adminSubject = $"Mesaj Nou Contact: {model.Subject}";
                var adminBody = $@"
                    <h3>Mesaj nou primit de la {model.Name}</h3>
                    <p><strong>Email:</strong> {model.Email}</p>
                    <p><strong>Telefon:</strong> {model.Phone ?? "Nespecificat"}</p>
                    <p><strong>Subiect:</strong> {model.Subject}</p>
                    <p><strong>Mesaj:</strong><br/>{model.Message}</p>
                ";
                await _emailSender.SendEmailAsync(adminEmail, adminSubject, adminBody);

                // 4. Send Confirmation Email to User
                var userSubject = "Confirmare primire mesaj - Dr. Daniela Vîntu";
                var userBody = $@"
                    <h3>Bună ziua, {model.Name},</h3>
                    <p>Vă mulțumim pentru mesajul transmis. Am primit solicitarea dumneavoastră cu subiectul '{model.Subject}' și vă vom contacta în cel mai scurt timp posibil.</p>
                    <p>O zi sănătoasă!<br/>Echipa Dr. Daniela Vîntu</p>
                ";
                await _emailSender.SendEmailAsync(model.Email, userSubject, userBody);

                TempData["Success"] = "Mesajul a fost trimis cu succes! Veți primi o confirmare pe email.";
                return RedirectToAction("Contact", "Home");
            }

            TempData["Error"] = "Vă rugăm să corectați erorile din formular.";
            return RedirectToAction("Contact", "Home");
        }
    }
}
