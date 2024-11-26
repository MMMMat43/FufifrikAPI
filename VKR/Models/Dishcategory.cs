using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VKR.Models;

[Table("dishcategories", Schema = "restaurant")]
public class Dishcategory
{
    [Key]
    public int Dishcategoryid { get; set; }

    [Required]
    [MaxLength(255)]
    public string Categoryname { get; set; } = null!;

    public virtual ICollection<Dish> Dishes { get; set; } = new List<Dish>();
}
