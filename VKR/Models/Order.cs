using System;
using System.Collections.Generic;

namespace VKR.Models;

public partial class Order
{
    public int Orderid { get; set; }

    public int Userid { get; set; }

    public DateTime Orderdatetime { get; set; }

    public decimal Totalamount { get; set; }

    public string Orderstatus { get; set; } = null!;

    public virtual ICollection<Deliveryinfo> Deliveryinfos { get; set; } = new List<Deliveryinfo>();

    public virtual ICollection<Orderitem> Orderitems { get; set; } = new List<Orderitem>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
