namespace CafePOS.Models;

public class Item
{
    public int ItemID { get; set; }

    public int CategoryID { get; set; }

    public string ItemName { get; set; } = string.Empty;

    public string ItemDescription { get; set; } = string.Empty;
    
}