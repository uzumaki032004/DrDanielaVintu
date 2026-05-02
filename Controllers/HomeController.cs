using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DrDanielaVintu.Models;
using DrDanielaVintu.Data;
using Microsoft.EntityFrameworkCore;

namespace DrDanielaVintu.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> Index()
    {
        ViewData["MetaDescription"] = "Dr. Daniela Vîntu - Expert în nutriție și stil de viață sănătos. Descoperă programe personalizate pentru energie, echilibru și stare de bine pe termen lung.";
        // Fetch all data for the "Flow"
        var settings = await _context.SiteSettings.AsNoTracking().ToDictionaryAsync(s => s.Key, s => s.Value);
        var programs = await _context.SubscriptionPlans.AsNoTracking().ToListAsync();
        var latestArticles = await _context.Articles.AsNoTracking()
            .OrderByDescending(a => a.Id) // Sort by ID to ensure strict new-to-old order
            .Take(3)
            .ToListAsync();
        
        var testimonials = await _context.Testimonials.AsNoTracking()
            .Where(t => t.IsVisible)
            .OrderByDescending(t => t.CreatedAt)
            .Take(6)
            .ToListAsync();

        ViewBag.Programs = programs;
        ViewBag.LatestArticles = latestArticles;
        ViewBag.Testimonials = testimonials;

        return View(settings);
    }

    public async Task<IActionResult> About()
    {
        ViewData["MetaDescription"] = "Află povestea Dr. Daniela Vîntu și abordarea ei bazată pe știință și empatie în nutriție.";
        var settings = await _context.SiteSettings.Where(s => s.Key.StartsWith("About_")).ToDictionaryAsync(s => s.Key, s => s.Value);
        return View(settings);
    }

    public async Task<IActionResult> Subscriptions()
    {
        ViewData["MetaDescription"] = "Alege programul de nutriție care ți se potrivește. Consultații unice, pachete Basic sau VIP pentru rezultate durabile.";
        var plans = await _context.SubscriptionPlans.ToListAsync();
        return View(plans);
    }

    public async Task<IActionResult> News()
    {
        ViewData["MetaDescription"] = "Noutăți și evenimente din lumea nutriției cu Dr. Daniela Vîntu.";
        var news = await _context.NewsItems.OrderByDescending(n => n.EventDate).ToListAsync();
        return View(news);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> Articles(string? q, string? category, int page = 1)
    {
        ViewData["MetaDescription"] = "Articole, sfaturi și resurse despre nutriție, rețete sănătoase și un stil de viață echilibrat.";
        
        int pageSize = 9;
        var query = _context.Articles.AsNoTracking().AsQueryable();

        // Search
        if (!string.IsNullOrEmpty(q))
        {
            query = query.Where(a => a.Title.Contains(q) || a.Content.Contains(q));
            ViewBag.SearchQuery = q;
        }

        // Category Filter
        if (!string.IsNullOrEmpty(category))
        {
            query = query.Where(a => a.Category == category);
            ViewBag.SelectedCategory = category;
        }

        var totalItems = await query.CountAsync();
        var articles = await query
            .OrderByDescending(a => a.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        ViewBag.CurrentPage = page;
        ViewBag.Categories = await _context.Articles.Select(a => a.Category).Distinct().ToListAsync();

        return View(articles);
    }

    public async Task<IActionResult> ArticleDetails(int id)
    {
        var article = await _context.Articles.FindAsync(id);
        if (article == null) return NotFound();
        ViewData["Title"] = article.Title;
        ViewData["MetaDescription"] = article.Content.Length > 150 ? article.Content.Substring(0, 150) : article.Content;
        return View(article);
    }

    public async Task<IActionResult> Contact()
    {
        ViewData["MetaDescription"] = "Contactează echipa Dr. Daniela Vîntu pentru întrebări sau programări la consultații de nutriție.";
        var settings = await _context.SiteSettings.Where(s => s.Key.StartsWith("Contact_")).ToDictionaryAsync(s => s.Key, s => s.Value);
        return View(settings);
    }

    public IActionResult Calculator()
    {
        ViewData["Title"] = "Calculator IMC";
        ViewData["MetaDescription"] = "Calculează-ți Indicele de Masă Corporală (IMC) rapid și gratuit. Află în ce categorie de greutate te încadrezi.";
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [Route("Home/Error/{statusCode}")]
    public async Task<IActionResult> Error(int statusCode)
    {
        ViewData["LatestArticles"] = await _context.Articles.AsNoTracking().OrderByDescending(a => a.Id).Take(3).ToListAsync();
        
        switch (statusCode)
        {
            case 404:
                return View("NotFound");
            case 403:
                return View("AccessDenied");
            default:
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
