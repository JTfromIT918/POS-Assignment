using CafePOS.Models;
using CafePOS.Models.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CafePOS.Controllers;

public class OrdersController : Controller
{
    private readonly CafeDbContext _context;

    public OrdersController(CafeDbContext context)
    {
        _context = context;
    }

    public IActionResult NewOrder()
    {
        var servers = new List<CafePOS.Models.Database.Server>
        {
            new()
            {
                ServerId = 1,
                FirstName = "Emma",
                LastName = "Garcia"
            },
            new()
            {
                ServerId = 2,
                FirstName = "Jacob",
                LastName = "Chen"
            },
            new()
            {
                ServerId = 3,
                FirstName = "Sofia",
                LastName = "Sato"
            }
        };

        return View(servers);
    }

    public IActionResult SelectServer(int serverId)
    {
        return RedirectToAction("CreateOrder", new { serverId });
    }

    public IActionResult CreateOrder(int serverId)
    {
        var categories = new List<CafePOS.Models.Database.Category>
        {
            new()
            {
                CategoryId = 1,
                CategoryName = "Coffee"
            },
            new()
            {
                CategoryId = 2,
                CategoryName = "Food"
            }
        };

        var items = new List<CafePOS.Models.Database.Item>
        {
            new()
            {
                ItemId = 1,
                CategoryId = 1,
                ItemName = "Latte",
                ItemDescription = "Espresso with steamed milk"
            },
            new()
            {
                ItemId = 2,
                CategoryId = 1,
                ItemName = "Drip Coffee",
                ItemDescription = "Freshly brewed house coffee"
            },
            new()
            {
                ItemId = 3,
                CategoryId = 2,
                ItemName = "Turkey Sandwich",
                ItemDescription = "Turkey, Cheese, Lettuce, and tomato"
            },
            new()
            {
                ItemId = 4,
                CategoryId = 2,
                ItemName = "Chocolate Pastry",
                ItemDescription = "Fresh baked chocolate pastry"
            }
        };

        var viewModel = new CreateOrderViewModel
        {
            ServerID = serverId,
            Categories = categories,
            Items = items
        };

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult AddItem(int serverId, int itemId)
    {
        var itemNames = new Dictionary<int, string>
        {
            { 1, "Latte" },
            { 2, "Drip Coffee" },
            { 3, "Turkey Sandwich" },
            { 4, "Chocolate Pastry" }
        };

        var itemPrices = new Dictionary<int, decimal>
        {
            { 1, 4.00m },
            { 2, 2.50m },
            { 3, 7.50m },
            { 4, 3.50m }
        };

        var orderItemsJson = HttpContext.Session.GetString("OrderItems");

        var orderItems = string.IsNullOrEmpty(orderItemsJson)
            ? new List<OrderItemViewModel>()
            : JsonSerializer.Deserialize<List<OrderItemViewModel>>(orderItemsJson)
                ?? new List<OrderItemViewModel>();

        var existingItem = orderItems.FirstOrDefault(i => i.ItemID == itemId);

        if (existingItem != null)
        {
            existingItem.Quantity++;
        }
        else
        {
            orderItems.Add(new OrderItemViewModel
            {
                ItemID = itemId,
                ItemName = itemNames[itemId],
                Price = itemPrices[itemId],
                Quantity = 1
            });
        }

        HttpContext.Session.SetString(
            "OrderItems",
            JsonSerializer.Serialize(orderItems)
        );

        var viewModel = new CreateOrderViewModel
        {
            ServerID = serverId,
            OrderItems = orderItems
        };

        return View("CurrentOrder", viewModel);
    }

    [HttpPost]
    public IActionResult IncreaseQuantity(int itemId)
    {
        var orderItems = GetOrderItems();

        var item = orderItems.FirstOrDefault(i => i.ItemID == itemId);

        if (item != null)
        {
            item.Quantity++;
        }

        SaveOrderItems(orderItems);

        var viewModel = new CreateOrderViewModel
        {
            ServerID = 1,
            OrderItems = orderItems
        };

        return View("CurrentOrder", viewModel);
    }

    [HttpPost]
    public IActionResult DecreaseQuantity(int itemId)
    {
        var orderItems = GetOrderItems();

        var item = orderItems.FirstOrDefault(i => i.ItemID == itemId);

        if (item != null && item.Quantity > 1)
        {
            item.Quantity--;
        }

        SaveOrderItems(orderItems);

        var viewModel = new CreateOrderViewModel
        {
            ServerID = 1,
            OrderItems = orderItems
        };

        return View("CurrentOrder", viewModel);
    }

    [HttpPost]
    public IActionResult RemoveItem(int itemId)
    {
        var orderItems = GetOrderItems();

        var item = orderItems.FirstOrDefault(i => i.ItemID == itemId);

        if (item != null)
        {
            orderItems.Remove(item);
        }

        SaveOrderItems(orderItems);

        var viewModel = new CreateOrderViewModel
        {
            ServerID = 1,
            OrderItems = orderItems
        };

        return View("CurrentOrder", viewModel);
    }

    [HttpPost]
    public IActionResult PlaceOrder(int serverId)
    {
        var orderItems = GetOrderItems();

        if (orderItems.Count == 0)
        {
            return RedirectToAction("CreateOrder", new { serverId });
        }

        var order = new CafeOrder
        {
            ServerId = serverId,
            OrderDate = DateTime.Today,
            SubTotal = orderItems.Sum(i => i.LineTotal),
            Tax = orderItems.Sum(i => i.LineTotal) * 0.085m,
            Tip = 0,
            AmountDue = orderItems.Sum(i => i.LineTotal) * 1.085m
        };

        _context.CafeOrders.Add(order);
        _context.SaveChanges();

        foreach (var item in orderItems)
        {
            var itemPrice = _context.ItemPrices
                .FirstOrDefault(p => p.ItemId == item.ItemID);

            if (itemPrice == null)
            {
                continue;
            }

            var orderItem = new OrderItem
            {
                OrderId = order.OrderId,
                ItemPriceId = itemPrice.ItemPriceId,
                Quantity = (sbyte)item.Quantity,
                ExtendedPrice = item.LineTotal
            };

            _context.OrderItems.Add(orderItem);
        }

        _context.SaveChanges();

        HttpContext.Session.Remove("OrderItems");

        return RedirectToAction("OrderDetails", new { orderId = order.OrderId });
    }

    public IActionResult OrderDetails(int orderId)
    {
        var order = _context.CafeOrders
            .Include(o => o.Server)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.ItemPrice)
                    .ThenInclude(ip => ip.Item)
            .FirstOrDefault(o => o.OrderId == orderId);

        if (order == null)
        {
            return NotFound();
        }

        return View(order);
    }

    [HttpGet]
    public IActionResult Payment(int orderId)
    {
        var order = _context.CafeOrders
            .Include(o => o.PaymentType)
            .Include(o => o.Server)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.ItemPrice)
                    .ThenInclude(ip => ip.Item)
            .FirstOrDefault(o => o.OrderId == orderId);

        if (order == null)
        {
            return NotFound();
        }

        var paymentTypes = _context.PaymentTypes
            .OrderBy(p => p.PaymentTypeName)
            .ToList();

        ViewBag.PaymentTypes = paymentTypes;

        return View(order);
    }

    [HttpPost]
    public IActionResult ProcessPayment(int orderId, int paymentTypeId)
    {
        var order = _context.CafeOrders
            .FirstOrDefault(o => o.OrderId == orderId);

        if (order == null)
        {
            return NotFound();
        }

        order.PaymentTypeId = paymentTypeId;

        _context.SaveChanges();

        return RedirectToAction("Index", "Home");
    }

    private List<OrderItemViewModel> GetOrderItems()
    {
        var orderItemsJson = HttpContext.Session.GetString("OrderItems");

        if (string.IsNullOrEmpty(orderItemsJson))
        {
            return new List<OrderItemViewModel>();
        }

        return JsonSerializer.Deserialize<List<OrderItemViewModel>>(orderItemsJson)
            ?? new List<OrderItemViewModel>();
    }

    private void SaveOrderItems(List<OrderItemViewModel> orderItems)
    {
        HttpContext.Session.SetString(
            "OrderItems",
            JsonSerializer.Serialize(orderItems)
        );
    }
}

