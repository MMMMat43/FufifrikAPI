using System;
using System.Collections.Generic;

namespace VKR.Models;

public partial class Paymenttype
{
    public int Paymenttypeid { get; set; }

    public string Paymenttypename { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
