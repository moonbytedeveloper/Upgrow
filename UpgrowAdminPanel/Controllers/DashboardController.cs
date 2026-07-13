using Microsoft.AspNetCore.Mvc;

namespace UpgrowAdminPanel.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
