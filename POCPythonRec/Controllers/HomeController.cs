using Microsoft.AspNetCore.Mvc;
using Python.Runtime;
namespace POCPythonRec.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment environment;
        public HomeController(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }
        public IActionResult Index()
        {
            var top10 = PythonRecommendationService.GetRecommendations("Champlin, Thiel and Schulist", environment);
            return View(top10);
        }
    }
}
