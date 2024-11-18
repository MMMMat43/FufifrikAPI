using System;
using System.Collections.Generic;

namespace VKR.Models;

public partial class Ingredientamount
{
    public int Dishid { get; set; }

    public int Ingredientid { get; set; }

    public int Amount { get; set; }

    public virtual Dish Dish { get; set; } = null!;

    public virtual Ingredient Ingredient { get; set; } = null!;
}
