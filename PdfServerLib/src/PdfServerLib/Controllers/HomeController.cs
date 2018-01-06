using Microsoft.AspNetCore.Mvc;

namespace PdfServerLib.Controllers
{
    public class HomeController : Controller
    {        
        public IActionResult Index()
        {
            return View();
        }
    }
}
