using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DrDanielaVintu.Data;
using DrDanielaVintu.Models;
using DrDanielaVintu.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace DrDanielaVintu.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPhotoService _photoService;

        public AdminController(ApplicationDbContext context, IPhotoService photoService)
        {
            _context = context;
            _photoService = photoService;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Edit Home
        [HttpGet]
        public async Task<IActionResult> EditHome()
        {
            var settings = await _context.SiteSettings.Where(s => s.Key.StartsWith("Home_")).ToListAsync();
            return View(settings);
        }

        [HttpPost]
        public async Task<IActionResult> EditHome(Dictionary<string, string> settings)
        {
            foreach (var item in settings)
            {
                var setting = await _context.SiteSettings.FirstOrDefaultAsync(s => s.Key == item.Key);
                if (setting == null)
                {
                    _context.SiteSettings.Add(new SiteSettings { Key = item.Key, Value = item.Value });
                }
                else
                {
                    setting.Value = item.Value;
                }
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Edit About
        [HttpGet]
        public async Task<IActionResult> EditAbout()
        {
            var settings = await _context.SiteSettings.Where(s => s.Key.StartsWith("About_")).ToListAsync();
            return View(settings);
        }

        [HttpPost]
        public async Task<IActionResult> EditAbout(Dictionary<string, string> settings, IFormFile? photo)
        {
            foreach (var item in settings)
            {
                var setting = await _context.SiteSettings.FirstOrDefaultAsync(s => s.Key == item.Key);
                if (setting == null)
                {
                    _context.SiteSettings.Add(new SiteSettings { Key = item.Key, Value = item.Value });
                }
                else
                {
                    setting.Value = item.Value;
                }
            }

            if (photo != null && photo.Length > 0)
            {
                var uploadResult = await _photoService.AddPhotoAsync(photo);
                
                if (uploadResult.Error != null)
                {
                    ModelState.AddModelError("", "Eroare la încărcarea imaginii: " + uploadResult.Error.Message);
                    var settingsList = await _context.SiteSettings.Where(s => s.Key.StartsWith("About_")).ToListAsync();
                    return View(settingsList);
                }

                if (uploadResult.SecureUrl != null)
                {
                    var photoUrl = uploadResult.SecureUrl.AbsoluteUri;
                    var photoSetting = await _context.SiteSettings.FirstOrDefaultAsync(s => s.Key == "About_PhotoUrl");
                    
                    if (photoSetting == null)
                    {
                        _context.SiteSettings.Add(new SiteSettings { Key = "About_PhotoUrl", Value = photoUrl });
                    }
                    else
                    {
                        photoSetting.Value = photoUrl;
                    }
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Programs (formerly Subscriptions)
        public async Task<IActionResult> EditPrograms()
        {
            var plans = await _context.SubscriptionPlans.ToListAsync();
            return View(plans);
        }

        [HttpPost]
        public async Task<IActionResult> AddProgram(SubscriptionPlan plan)
        {
            _context.SubscriptionPlans.Add(plan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(EditPrograms));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProgram(SubscriptionPlan plan)
        {
            _context.SubscriptionPlans.Update(plan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(EditPrograms));
        }

        public async Task<IActionResult> DeleteProgram(int id)
        {
            var plan = await _context.SubscriptionPlans.FindAsync(id);
            if (plan != null)
            {
                _context.SubscriptionPlans.Remove(plan);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(EditPrograms));
        }

        // News
        public async Task<IActionResult> EditNews()
        {
            var news = await _context.NewsItems.ToListAsync();
            return View(news);
        }

        [HttpPost]
        public async Task<IActionResult> AddNews(NewsItem item)
        {
            _context.NewsItems.Add(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(EditNews));
        }

        public async Task<IActionResult> DeleteNews(int id)
        {
            var item = await _context.NewsItems.FindAsync(id);
            if (item != null)
            {
                _context.NewsItems.Remove(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(EditNews));
        }

        // Articles
        public async Task<IActionResult> EditArticles()
        {
            var articles = await _context.Articles.ToListAsync();
            return View(articles);
        }

        [HttpPost]
        public async Task<IActionResult> AddArticle(Article article, IFormFile? photo)
        {
            try
            {
                if (photo != null && photo.Length > 0)
                {
                    var result = await _photoService.AddPhotoAsync(photo);
                    if (result.Error != null)
                    {
                        ModelState.AddModelError("", "Eroare Cloudinary: " + result.Error.Message);
                        var articles = await _context.Articles.ToListAsync();
                        return View("EditArticles", articles);
                    }
                    article.ImageUrl = result.SecureUrl.AbsoluteUri;
                }
                
                article.CreatedAt = DateTime.UtcNow;
                article.Category ??= "General";
                
                _context.Articles.Add(article);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Articolul a fost publicat cu succes!";
                return RedirectToAction(nameof(EditArticles));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                var articles = await _context.Articles.OrderByDescending(a => a.Id).ToListAsync();
                return View("EditArticles", articles);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateArticle(Article article, IFormFile? photo)
        {
            try
            {
                var existing = await _context.Articles.FindAsync(article.Id);
                if (existing == null) return NotFound();

                existing.Title = article.Title;
                existing.Content = article.Content;
                existing.Author = article.Author;
                existing.Tags = article.Tags;

                if (photo != null && photo.Length > 0)
                {
                    var result = await _photoService.AddPhotoAsync(photo);
                    if (result.Error == null)
                    {
                        existing.ImageUrl = result.SecureUrl.AbsoluteUri;
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(EditArticles));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Eroare la actualizare: " + ex.Message);
                var articles = await _context.Articles.ToListAsync();
                return View("EditArticles", articles);
            }
        }

        public async Task<IActionResult> DeleteArticle(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article != null)
            {
                _context.Articles.Remove(article);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(EditArticles));
        }

        // Contact
        public async Task<IActionResult> EditContact()
        {
            var settings = await _context.SiteSettings.Where(s => s.Key.StartsWith("Contact_")).ToListAsync();
            return View(settings);
        }

        [HttpPost]
        public async Task<IActionResult> EditContact(Dictionary<string, string> settings)
        {
            foreach (var item in settings)
            {
                var setting = await _context.SiteSettings.FirstOrDefaultAsync(s => s.Key == item.Key);
                if (setting == null) _context.SiteSettings.Add(new SiteSettings { Key = item.Key, Value = item.Value });
                else setting.Value = item.Value;
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Messages
        public async Task<IActionResult> GetMessages()
        {
            var messages = await _context.ContactMessages
                .OrderByDescending(m => m.SentAt)
                .ToListAsync();
            return View(messages);
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var message = await _context.ContactMessages.FindAsync(id);
            if (message != null)
            {
                message.IsRead = true;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(GetMessages));
        }

        public async Task<IActionResult> DeleteMessage(int id)
        {
            var message = await _context.ContactMessages.FindAsync(id);
            if (message != null)
            {
                _context.ContactMessages.Remove(message);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(GetMessages));
        }

        // Bookings
        public async Task<IActionResult> EditBookings()
        {
            var bookings = await _context.Bookings
                .Include(b => b.Plan)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
            return View(bookings);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                booking.Status = "Confirmed";
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(EditBookings));
        }

        [HttpPost]
        public async Task<IActionResult> CancelBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                booking.Status = "Cancelled";
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(EditBookings));
        }

        // Testimonials
        public async Task<IActionResult> EditTestimonials()
        {
            var testimonials = await _context.Testimonials
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
            return View(testimonials);
        }

        [HttpPost]
        public async Task<IActionResult> AddTestimonial(Testimonial testimonial)
        {
            if (string.IsNullOrEmpty(testimonial.ClientInitials) && !string.IsNullOrEmpty(testimonial.ClientName))
            {
                var names = testimonial.ClientName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                testimonial.ClientInitials = names.Length > 1 
                    ? (names[0][0].ToString() + names[1][0].ToString()).ToUpper() 
                    : names[0][0].ToString().ToUpper();
            }
            
            testimonial.CreatedAt = DateTime.UtcNow;
            _context.Testimonials.Add(testimonial);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(EditTestimonials));
        }

        [HttpPost]
        public async Task<IActionResult> ToggleTestimonialVisibility(int id)
        {
            var t = await _context.Testimonials.FindAsync(id);
            if (t != null)
            {
                t.IsVisible = !t.IsVisible;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(EditTestimonials));
        }

        public async Task<IActionResult> DeleteTestimonial(int id)
        {
            var t = await _context.Testimonials.FindAsync(id);
            if (t != null)
            {
                _context.Testimonials.Remove(t);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(EditTestimonials));
        }

        // User Management
        public async Task<IActionResult> ManageUsers()
        {
            var users = await _context.Users.ToListAsync();
            var userRoles = new Dictionary<string, IList<string>>();
            
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRoles[user.Id] = roles;
            }

            ViewBag.UserRoles = userRoles;
            return View(users);
        }
    }
}
