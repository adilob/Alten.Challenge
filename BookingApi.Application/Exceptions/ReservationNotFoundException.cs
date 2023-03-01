using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApi.Application.Exceptions
{
    public class ReservationNotFoundException : Exception
    {
        public ReservationNotFoundException(string? message) : base(message)
        {
        }
    }
}
