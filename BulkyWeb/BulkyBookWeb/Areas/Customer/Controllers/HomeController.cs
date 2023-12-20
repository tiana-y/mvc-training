using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Customer.Controllers;

[Area("Customer")]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
