using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace InvestmentApp.Controllers
{
    public class IndicadoresController : Controller
    {
       
        public IActionResult Index()
        {
            return View();
        }
    }
}
