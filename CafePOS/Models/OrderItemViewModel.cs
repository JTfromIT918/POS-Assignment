namespace CafePOS.Models;

public class OrderItemViewModel
{
    public int ItemID { get; set; }

    public string ItemName { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int Quantity { get; set; }
}