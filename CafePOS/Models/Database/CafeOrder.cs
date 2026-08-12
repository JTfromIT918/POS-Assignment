using System;
using System.Collections.Generic;

namespace CafePOS.Models.Database;

public partial class CafeOrder
{
    public int OrderId { get; set; }

    public int? ServerId { get; set; }

    public int? PaymentTypeId { get; set; }

    public DateTime OrderDate { get; set; }

    public decimal? SubTotal { get; set; }

    public decimal? Tax { get; set; }

    public decimal? Tip { get; set; }

    public decimal? AmountDue { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual PaymentType? PaymentType { get; set; }

    public virtual Server? Server { get; set; }
}
