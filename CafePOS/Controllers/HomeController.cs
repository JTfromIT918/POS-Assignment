using System.Diagnostics;
using CafePOS.Models;
using CafePOS.Models.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CafePOS.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly CafeDbContext _context;

    public HomeController(
        ILogger<HomeController> logger,
        CafeDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        var openOrders = _context.CafeOrders
            .Include(order => order.Server)
            .Include(order => order.OrderItems)
            .Where(order => order.PaymentTypeId == null)
            .OrderBy(order => order.OrderDate)
            .ToList();

        return View(openOrders);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(
        Duration = 0,
        Location = ResponseCacheLocation.None,
        NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id
                ?? HttpContext.TraceIdentifier
        });
    }
}