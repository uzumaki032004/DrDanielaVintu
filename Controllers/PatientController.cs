using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DrDanielaVintu.Data;
using DrDanielaVintu.Models;
using System.Security.Claims;

namespace DrDanielaVintu.Controllers
{
    [Authorize] // Any logged in user
    public class PatientController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PatientController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Dashboard()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            
            var bookings = await _context.Bookings
                .Include(b => b.Plan)
                .Where(b => b.ClientEmail == userEmail)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            ViewBag.LatestArticles = await _context.Articles.AsNoTracking().OrderByDescending(a => a.Id).Take(3).ToListAsync();
            
            return View(bookings);
        }

        public async Task<IActionResult> MyBookings()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var bookings = await _context.Bookings
                .Include(b => b.Plan)
                .Where(b => b.ClientEmail == userEmail)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
            return View(bookings);
        }
    }
}
