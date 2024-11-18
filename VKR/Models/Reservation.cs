using System;
using System.Collections.Generic;

namespace VKR.Models;

public partial class Reservation
{
    public int Reservationid { get; set; }

    public int Userid { get; set; }

    public DateTime Reservationdatetime { get; set; }

    public int Tablenumber { get; set; }

    public int Guestcount { get; set; }
}
