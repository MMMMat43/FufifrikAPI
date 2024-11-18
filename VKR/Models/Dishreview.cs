using System;
using System.Collections.Generic;

namespace VKR.Models;

public partial class Dishreview
{
    public int Reviewid { get; set; }

    public int Userid { get; set; }

    public int Dishid { get; set; }

    public int Rating { get; set; }

    public string? Comment { get; set; }

    public virtual Dish Dish { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
