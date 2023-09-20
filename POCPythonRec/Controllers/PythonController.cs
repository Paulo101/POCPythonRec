using Microsoft.AspNetCore.Mvc;

namespace POCPythonRec.Controllers
{
    public class PythonController : Controller
    {
        public IActionResult Index()
        {
            //logic to update ../../PythonHelper/Data/Accomodation.csv
            return View();
        }
    }
}
