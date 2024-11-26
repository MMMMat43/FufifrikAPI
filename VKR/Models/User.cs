using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace VKR.Models;

[Table (name: "users", Schema = "restaurant")]
public partial class User
{
    [Column(name: "userid")]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [JsonPropertyName("Userid")]
    public int Userid { get; set; }
    [Column(name: "fullname")]
    [Required(ErrorMessage = "Поле Имени является обязательным параметром")]
    [StringLength(255, MinimumLength = 1, ErrorMessage = "Поле названия Имени должно содержать от 1 до 255 символов")]
    
    [JsonPropertyName("Fullname")]
    public string? Fullname { get; set; }
    [Column(name: "email")]
    [Required(ErrorMessage = "Поле Имени является обязательным параметром")]
    [StringLength(255, MinimumLength = 1, ErrorMessage = "Поле названия Имени должно содержать от 1 до 255 символов")]
    
    [JsonPropertyName("Email")]
    public string? Email { get; set; }
    [Column(name: "phonenumber")]
    [Required(ErrorMessage = "Поле телефонного номера отеля является обязательным параметром")]
    [StringLength(12, MinimumLength = 1, ErrorMessage = "Поле телефонного номера отеля должно содержать строго 12 символов")]
    
    [JsonPropertyName("Phonenumber")]
    public string? Phonenumber { get; set; }
    [JsonPropertyName("Loyaltypoints")]
    public int? Loyaltypoints { get; set; }
    [JsonPropertyName("Address")]
    public string? Address { get; set; }
    [JsonPropertyName("Passwordhash")]
    [Column(name: "passwordhash")]
    [Required(ErrorMessage = "Поле Имени является обязательным параметром")]
    [StringLength(255, MinimumLength = 1, ErrorMessage = "Поле названия Имени должно содержать от 1 до 255 символов")]
    public string Passwordhash { get; set; } = null!;
    public virtual ICollection<Dishreview> Dishreviews { get; set; } = new List<Dishreview>();
}
