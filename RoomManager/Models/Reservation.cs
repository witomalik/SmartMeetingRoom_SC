using RoomManager.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomManager.Models
{
    public class Reservation
    {
        public CardReader CardReader { get; set; }
        public string StartTime_string { get; set; }
        public Person Person { get; set; }
        public long StartTime { get; set; }
        public string EndTime_string { get; set; }
        public long EndTime { get; set; }
    }
}
