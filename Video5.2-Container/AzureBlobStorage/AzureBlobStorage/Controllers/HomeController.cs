using AzureBlobStorage.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AzureBlobStorage.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ListBlobs()
        {
            return View();
        }

        [HttpGet]
        public IActionResult UploadBlob()
        {
            return View();
        }
    }
}
