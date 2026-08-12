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
        var today = DateOnly.FromDateTime(DateTime.Today);

        var servers = _context.Servers
            .Where(s => s.HireDate <= today &&
                        (s.TermDate == null || s.TermDate >= today))
            .ToList();

        return View(servers);
    }

    public IActionResult SelectServer(int serverId)
    {
        var server = _context.Servers
        .FirstOrDefault(s => s.ServerId == serverId);

        if (server == null)
        {
            return NotFound();
        }

        var today = DateOnly.FromDateTime(DateTime.Today);

        if (server.HireDate > today ||
        (server.TermDate != null && server.TermDate < today))

        {
            return BadRequest("That server is not active.");
        }

        var order = new CafeOrder

        {

        ServerId = serverId,
        OrderDate = DateTime.Now,
        SubTotal = 0,
        Tax = 0,
        Tip = 0,
        AmountDue = 0
    };

    _context.CafeOrders.Add(order);
    _context.SaveChanges();

    return RedirectToAction(
        "OrderDetails",
        new { orderId = order.OrderId }

    );

}

    public IActionResult CreateOrder(int serverId)
    {
        var categories = _context.Categories
            .ToList();

        var items = _context.Items
            .ToList();

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
        var item = _context.Items
            .FirstOrDefault(i => i.ItemId == itemId);

        if (item == null)
        {
            return NotFound();
        }

        var itemPrice = _context.ItemPrices
            .FirstOrDefault(p => p.ItemId == itemId);

        if (itemPrice == null)
        {
            return NotFound();
        }

        var orderItems = GetOrderItems();

        var existingItem = orderItems
            .FirstOrDefault(i => i.ItemID == itemId);

        if (existingItem != null)
        {
            existingItem.Quantity++;
        }
        else
        {
            orderItems.Add(new OrderItemViewModel
            {
                ItemID = item.ItemId,
                ItemName = item.ItemName,
                Price = itemPrice.Price,
                Quantity = 1
            });
        }

        SaveOrderItems(orderItems);

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

        var subTotal = orderItems.Sum(i => i.LineTotal);
        var tax = subTotal * 0.085m;
        var amountDue = subTotal + tax;

        var order = new CafeOrder
        {
            ServerId = serverId,
            OrderDate = DateTime.Now,
            SubTotal = subTotal,
            Tax = tax,
            Tip = 0,
            AmountDue = amountDue
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

        return RedirectToAction(
            "OrderDetails",
            new { orderId = order.OrderId }
        );
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

    public IActionResult AddItem(int orderId)
    {
        var order = _context.CafeOrders
            .FirstOrDefault(o => o.OrderId == orderId);

        if (order == null)
        {
            return NotFound();
        }

        var items = _context.Items
            .OrderBy(i => i.ItemName)
            .ToList();

        ViewBag.OrderId = orderId;

        return View(items);
    }

    [HttpPost]
    public IActionResult AddItemToOrder(int orderId, int itemId)
    {
        var order = _context.CafeOrders
        .FirstOrDefault(o => o.OrderId == orderId);

        if (order == null)
        {
                return NotFound();

        }

        var itemPrice = _context.ItemPrices
        .FirstOrDefault(p => p.ItemId == itemId);

        if (itemPrice == null)
        {
            return NotFound();
        }

        var existingOrderItem = _context.OrderItems
            .FirstOrDefault(oi =>
                oi.OrderId == orderId &&
                oi.ItemPriceId == itemPrice.ItemPriceId);

                if (existingOrderItem != null)
        {
            existingOrderItem.Quantity++;
            existingOrderItem.ExtendedPrice += itemPrice.Price;
        }
        else
        {
            var orderItem = new OrderItem
            {
                OrderId = orderId,
                ItemPriceId = itemPrice.ItemPriceId,
                Quantity = 1,
                ExtendedPrice = itemPrice.Price

            };

            _context.OrderItems.Add(orderItem);
        }

        _context.SaveChanges();

      

        var newSubtotal = _context.OrderItems
    .Where(oi => oi.OrderId == orderId)
    .Select(oi => oi.ExtendedPrice)
    .Sum();

var newTax = newSubtotal * 0.085m;
var newTip = 0m;
var newAmountDue = newSubtotal + newTax + newTip;

order.SubTotal = newSubtotal;
order.Tax = newTax;
order.Tip = newTip;
order.AmountDue = newAmountDue;



_context.SaveChanges();



return RedirectToAction("OrderDetails", new { orderId });
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
        var orderItemsJson =
            HttpContext.Session.GetString("OrderItems");

        if (string.IsNullOrEmpty(orderItemsJson))
        {
            return new List<OrderItemViewModel>();
        }

        return JsonSerializer.Deserialize<List<OrderItemViewModel>>(
            orderItemsJson
        ) ?? new List<OrderItemViewModel>();
    }

    private void SaveOrderItems(List<OrderItemViewModel> orderItems)
    {
        HttpContext.Session.SetString(
            "OrderItems",
            JsonSerializer.Serialize(orderItems)
        );
    }
}