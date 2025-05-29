using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace InvestmentApp.Controllers
{
    public class HomeController : Controller
    {
       
        public IActionResult Index()
        {
            return View();
        }
    }
}
