namespace CafePOS.Models;

public class CreateOrderViewModel
{
    public int ServerID { get; set; }

    public List<CafePOS.Models.Database.Category> Categories { get; set; } = new();

    public List<CafePOS.Models.Database.Item> Items { get; set; } = new();

    public List<OrderItemViewModel> OrderItems { get; set; } = new();
}