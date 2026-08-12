using Microsoft.AspNetCore.Mvc;

namespace CafePOS.Controllers;

public class OrdersController : Controller
{
    public IActionResult NewOrder()
    {
        return View();
    }
}