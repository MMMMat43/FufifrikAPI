using System;
using System.Collections.Generic;

namespace VKR.Models;

public partial class Orderitem
{
    public int Orderitemid { get; set; }

    public int Orderid { get; set; }

    public int Dishid { get; set; }

    public int Quantity { get; set; }

    public string Dishstatus { get; set; } = null!;

    public virtual Dish Dish { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
