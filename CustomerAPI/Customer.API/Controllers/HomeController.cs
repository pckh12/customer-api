using Microsoft.AspNetCore.Mvc;


namespace CustomerAPI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return new RedirectResult("~/swagger");
        }
    }
}
