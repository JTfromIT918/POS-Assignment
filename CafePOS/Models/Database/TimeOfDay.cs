using System;
using System.Collections.Generic;

namespace CafePOS.Models.Database;

public partial class TimeOfDay
{
    public int TimeOfDayId { get; set; }

    public string TimeOfDayName { get; set; } = null!;

    public virtual ICollection<ItemPrice> ItemPrices { get; set; } = new List<ItemPrice>();
}
