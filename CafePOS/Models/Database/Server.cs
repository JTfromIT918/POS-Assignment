using System;
using System.Collections.Generic;

namespace CafePOS.Models.Database;

public partial class Server
{
    public int ServerId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateOnly HireDate { get; set; }

    public DateOnly? TermDate { get; set; }

    public DateOnly DoB { get; set; }

    public virtual ICollection<CafeOrder> CafeOrders { get; set; } = new List<CafeOrder>();
}
