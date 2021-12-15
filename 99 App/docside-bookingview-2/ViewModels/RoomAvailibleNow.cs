using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace docside_bookingview_2.ViewModels
{
    public class RoomAvailibleNow
    {
        public int RoomId { get; set; }

        public string RoomName { get; set; }

        public string SquareMetres { get; set; }
        public string Floor { get; set; }
        public int MaxPeople { get; set; }

        public bool Available { get; set; }

        public bool Active { get; set; }
    }
}
