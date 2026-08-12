namespace CafePOS.Models;

public class ItemPrice
{
    public int ItemPriceID { get; set; }

    public int ItemID { get; set; }

    public int TimeofDayID { get; set; }

    public decimal Price { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }
    
}