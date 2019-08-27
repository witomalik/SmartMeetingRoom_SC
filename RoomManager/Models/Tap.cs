using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomManager.Models
{
    public class Tap
    {
        public string Person { get; set; }
        public long StartTime { get; set; }
        public long EndTime { get; set; }
        public long TapCount { get; set; }
    }
}
