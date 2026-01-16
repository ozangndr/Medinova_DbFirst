using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Medinova.DTOs
{
    public class AppointmentAvailablitiyDto
    {
        public string Time { get; set; }
        public bool IsBooked { get; set; }
    }
}