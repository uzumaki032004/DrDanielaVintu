using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.UI.Services;
using DrDanielaVintu.Data;
using DrDanielaVintu.Models;
using Microsoft.EntityFrameworkCore;

namespace DrDanielaVintu.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;

        public BookingController(ApplicationDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        [HttpGet]
        public async Task<IActionResult> Book(int planId)
        {
            var plan = await _context.SubscriptionPlans.FindAsync(planId);
            if (plan == null) return RedirectToAction("Subscriptions", "Home");

            var model = new Booking
            {
                PlanId = planId,
                Plan = plan,
                PreferredDate = DateTime.Now.AddDays(1)
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Book(Booking model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedAt = DateTime.UtcNow;
                model.Status = "Pending";
                _context.Bookings.Add(model);
                await _context.SaveChangesAsync();

                // Load plan details for email
                var plan = await _context.SubscriptionPlans.FindAsync(model.PlanId);

                // 1. Notify Admin
                var adminEmail = await _context.SiteSettings
                    .Where(s => s.Key == "Contact_Email")
                    .Select(s => s.Value)
                    .FirstOrDefaultAsync() ?? "micleusanudumitru@gmail.com";

                var adminSubject = $"Rezervare Nouă: {plan?.Name}";
                var adminBody = $@"
                    <h3>Rezervare nouă primită</h3>
                    <p><strong>Client:</strong> {model.ClientName}</p>
                    <p><strong>Email:</strong> {model.ClientEmail}</p>
                    <p><strong>Telefon:</strong> {model.ClientPhone}</p>
                    <p><strong>Program ales:</strong> {plan?.Name}</p>
                    <p><strong>Data preferată:</strong> {model.PreferredDate.ToString("dd.MM.yyyy")}</p>
                    <p><strong>Mesaj:</strong> {model.Message ?? "-"}</p>
                ";
                await _emailSender.SendEmailAsync(adminEmail, adminSubject, adminBody);

                // 2. Confirm to Client
                var userSubject = "Confirmare Rezervare - Dr. Daniela Vîntu";
                var userBody = $@"
                    <h3>Bună ziua, {model.ClientName},</h3>
                    <p>Rezervarea dumneavoastră pentru programul <strong>'{plan?.Name}'</strong> a fost recepționată cu succes.</p>
                    <p>Vă vom contacta telefonic sau prin email pentru a confirma data și ora finală pentru prima consultație.</p>
                    <p>Detalii rezervare:<br/>Data preferată: {model.PreferredDate.ToString("dd.MM.yyyy")}</p>
                    <p>Vă mulțumim!<br/>Echipa Dr. Daniela Vîntu</p>
                ";
                await _emailSender.SendEmailAsync(model.ClientEmail, userSubject, userBody);

                TempData["Success"] = "Rezervarea a fost trimisă cu succes! Vă vom contacta în curând.";
                return RedirectToAction("Subscriptions", "Home");
            }

            // If invalid, reload the plan
            model.Plan = await _context.SubscriptionPlans.FindAsync(model.PlanId);
            return View(model);
        }
    }
}
