using Microsoft.AspNetCore.Mvc;

namespace VerifyIndiaAdminPanel.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
