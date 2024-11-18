using System;
using System.Collections.Generic;

namespace VKR.Models;

public partial class Dish
{
    public int Dishid { get; set; }

    public string Dishname { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int? Calories { get; set; }

    public string? Imageurl { get; set; }

    public bool Isavailablefordelivery { get; set; }

    public virtual ICollection<Dishreview> Dishreviews { get; set; } = new List<Dishreview>();

    public virtual ICollection<Ingredientamount> Ingredientamounts { get; set; } = new List<Ingredientamount>();

    public virtual ICollection<Orderitem> Orderitems { get; set; } = new List<Orderitem>();
}
