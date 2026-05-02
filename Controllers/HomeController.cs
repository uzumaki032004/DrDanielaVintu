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

    public async Task<IActionResult> Index()
    {
        // Fetch all data for the "Flow"
        var settings = await _context.SiteSettings.ToDictionaryAsync(s => s.Key, s => s.Value);
        var programs = await _context.SubscriptionPlans.ToListAsync();
        var latestArticles = await _context.Articles.OrderByDescending(a => a.CreatedAt).Take(3).ToListAsync();

        ViewBag.Programs = programs;
        ViewBag.LatestArticles = latestArticles;

        return View(settings);
    }

    public async Task<IActionResult> About()
    {
        var settings = await _context.SiteSettings.Where(s => s.Key.StartsWith("About_")).ToDictionaryAsync(s => s.Key, s => s.Value);
        return View(settings);
    }

    public async Task<IActionResult> Subscriptions()
    {
        var plans = await _context.SubscriptionPlans.ToListAsync();
        return View(plans);
    }

    public async Task<IActionResult> News()
    {
        var news = await _context.NewsItems.OrderByDescending(n => n.EventDate).ToListAsync();
        return View(news);
    }

    public async Task<IActionResult> Articles()
    {
        var articles = await _context.Articles.OrderByDescending(a => a.CreatedAt).ToListAsync();
        return View(articles);
    }

    public async Task<IActionResult> ArticleDetails(int id)
    {
        var article = await _context.Articles.FindAsync(id);
        if (article == null) return NotFound();
        return View(article);
    }

    public async Task<IActionResult> Contact()
    {
        var settings = await _context.SiteSettings.Where(s => s.Key.StartsWith("Contact_")).ToDictionaryAsync(s => s.Key, s => s.Value);
        return View(settings);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
