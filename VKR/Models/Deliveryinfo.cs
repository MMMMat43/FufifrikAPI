using System;
using System.Collections.Generic;

namespace VKR.Models;

public partial class Deliveryinfo
{
    public int Deliveryinfoid { get; set; }

    public int Orderid { get; set; }

    public string Deliverystatus { get; set; } = null!;

    public int? Courierid { get; set; }

    public DateTime? Estimateddeliverytime { get; set; }

    public decimal? Deliverycost { get; set; }

    public virtual Order Order { get; set; } = null!;
}
