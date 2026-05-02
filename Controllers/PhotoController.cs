using DrDanielaVintu.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DrDanielaVintu.Controllers
{
    public class PhotoController : Controller
    {
        private readonly IPhotoService _photoService;

        public PhotoController(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ViewBag.Error = "Te rugăm să selectezi o imagine.";
                return View();
            }

            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null)
            {
                ViewBag.Error = result.Error.Message;
                return View();
            }

            ViewBag.ImageUrl = result.SecureUrl.AbsoluteUri;
            ViewBag.PublicId = result.PublicId;

            return View();
        }
    }
}
