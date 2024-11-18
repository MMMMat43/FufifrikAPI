using System;
using System.Collections.Generic;

namespace VKR.Models;

public partial class Ingredient
{
    public int Ingredientid { get; set; }

    public string Ingredientname { get; set; } = null!;

    public int Quantity { get; set; }

    public virtual ICollection<Ingredientamount> Ingredientamounts { get; set; } = new List<Ingredientamount>();
}
