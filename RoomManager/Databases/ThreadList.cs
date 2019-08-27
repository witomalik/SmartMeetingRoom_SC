using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomManager.Databases
{
    public class ThreadList
    {
        private static List<int> TIMER_DB = new List<int>();

        

        public static void addTimer(int id)
        {
            TIMER_DB.Add(id);
        }
    }
}
