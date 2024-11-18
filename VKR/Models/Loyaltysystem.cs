using System;
using System.Collections.Generic;

namespace VKR.Models;

public partial class Loyaltysystem
{
    public int Loyaltyid { get; set; }

    public int Userid { get; set; }

    public int Points { get; set; }

    public string? Description { get; set; }
}
