﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VKR.Models;

public partial class User
{
    [Key]
    public int Userid { get; set; }

    public string Fullname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phonenumber { get; set; }

    public int? Loyaltypoints { get; set; }

    public string? Address { get; set; }

    public string Passwordhash { get; set; } = null!;

    public virtual ICollection<Dishreview> Dishreviews { get; set; } = new List<Dishreview>();
}
