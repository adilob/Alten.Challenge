using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApi.Application.Exceptions
{
    public class RoomNotFoundException : Exception
    {
        public RoomNotFoundException(string? message) : base(message)
        {
        }
    }
}
