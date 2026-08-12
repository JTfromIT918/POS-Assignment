using System.ComponentModel;
using CafePOS.Models;
using Microsoft.AspNetCore.Mvc;

namespace CafePOS.Controllers;

public class OrdersController : Controller
{
    public IActionResult NewOrder()
    {
        var servers = new List<Server>
        {
            new()
            {
                ServerID = 1,
                FirstName = "Emma",
                LastName = "Garcia",
            },
            new()
            {
                ServerID = 2,
                FirstName = "Jacob",
                LastName = "Chen",
            },
            new()
            {
                ServerID = 3,
                FirstName = "Sofia",
                LastName = "Sato",
            }

        };

        return View(servers);
    }

    public IActionResult SelectServer(int serverId)
    {
        return RedirectToAction("CreateOrder", new {serverId});
    }

    public IActionResult CreateOrder(int serverId)
    {
        var categories = new List<Category>
        {
            new()
            {
                CategoryID = 1,
                CategoryName = "Coffee"
            },
            new()
            {
                CategoryID = 2,
                CategoryName = "Food"
            }
        };

        var items = new List<Item>
        {
            new()
            {
                ItemID = 1,
                CategoryID = 1,
                ItemName = "Latte",
                ItemDescription = "Espresso with steamed milk"
            },
            new()
            {
                ItemID = 2,
                CategoryID = 1,
                ItemName = "Drip Coffee",
                ItemDescription = "Freshly brewed house coffee"
            },
            new()
            {
                ItemID = 3,
                CategoryID = 2,
                ItemName = "Turkey Sandwich",
                ItemDescription = "Turkey, Cheese, Lettuce, and tomato"
            },
            new()
            {
                ItemID = 4,
                CategoryID = 2,
                ItemName = "Chocolate Pastry",
                ItemDescription = "Fresh baked chocolate pastry"
            }
        };

        ViewBag.ServerId = serverId;
        ViewBag.Categories = categories;
        ViewBag.Items = items;

        return View();
    }

    [HttpPost]
    public IActionResult AddItem(int serverId, int itemId)
    {
        return Content($"Item {itemId} added to server {serverId}'s order.");
    }
}