using RoomManager.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomManager.Models
{
    public class CrGroup
    {
        //public int Counter { get; set; } = 0;
        public string Name { get; set; }
        public List<Person> People = new List<Person>();
        public string Addr { get; set; }
        public string IP { get; set; }
        public int Port { get; set; }
    }
}
