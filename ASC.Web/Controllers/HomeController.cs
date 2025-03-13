using ASC.Web.Configuration;
using ASC.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using ASC.Utilities;
namespace ASC.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IOptions<ApplicationSettings> _settings;

        public HomeController(
             //ILogger<HomeController> logger, 
             IOptions<ApplicationSettings> settings)
        {
            //_logger = logger;
            _settings = settings;
        }

        public IActionResult Index()
        { // Thi?t l?p Session
            HttpContext.Session.SetSession("Test", _settings.Value);

            // L?y Session
            var settings = HttpContext.Session.GetSession<ApplicationSettings>("Test");

            // S? d?ng IOptions
            ViewBag.Title = settings.ApplicationTitle;

            // Tr??ng h?p ki?m th? th?t b?i (?ã comment)
            // ViewData.Model = "Test";
            // throw new Exception("??ng nh?p th?t b?i!!!");

            return View();
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
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
