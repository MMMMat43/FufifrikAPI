using System;
using System.Collections.Generic;

namespace VKR.Models;

public partial class Payment
{
    public int Paymentid { get; set; }

    public int Orderid { get; set; }

    public int Paymenttypeid { get; set; }

    public DateTime Paymentdatetime { get; set; }

    public decimal Paymentamount { get; set; }

    public string Paymentstatus { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;

    public virtual Paymenttype Paymenttype { get; set; } = null!;
}
