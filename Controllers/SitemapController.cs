using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DrDanielaVintu.Data;
using System.Text;
using System.Xml;

namespace DrDanielaVintu.Controllers
{
    public class SitemapController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SitemapController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("sitemap.xml")]
        public async Task<IActionResult> Index()
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var sitemapContent = new StringBuilder();
            sitemapContent.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sitemapContent.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");

            // Static Pages
            string[] staticPages = { "", "/Home/About", "/Home/Subscriptions", "/Home/Articles", "/Home/Contact", "/Home/News" };
            foreach (var page in staticPages)
            {
                sitemapContent.AppendLine("<url>");
                sitemapContent.AppendLine($"<loc>{baseUrl}{page}</loc>");
                sitemapContent.AppendLine("<changefreq>weekly</changefreq>");
                sitemapContent.AppendLine("<priority>0.8</priority>");
                sitemapContent.AppendLine("</url>");
            }

            // Articles
            var articles = await _context.Articles.AsNoTracking().ToListAsync();
            foreach (var article in articles)
            {
                sitemapContent.AppendLine("<url>");
                sitemapContent.AppendLine($"<loc>{baseUrl}/Home/ArticleDetails/{article.Id}</loc>");
                sitemapContent.AppendLine("<changefreq>monthly</changefreq>");
                sitemapContent.AppendLine("<priority>0.6</priority>");
                sitemapContent.AppendLine("</url>");
            }

            sitemapContent.AppendLine("</urlset>");

            return Content(sitemapContent.ToString(), "application/xml", Encoding.UTF8);
        }
    }
}
