using System;
using System.Collections.Generic;

namespace CafePOS.Models.Database;

public partial class PaymentType
{
    public int PaymentTypeId { get; set; }

    public string PaymentTypeName { get; set; } = null!;

    public virtual ICollection<CafeOrder> CafeOrders { get; set; } = new List<CafeOrder>();
}
