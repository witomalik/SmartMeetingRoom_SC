using RoomManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomManager.Databases
{
    public class CrName
    {
        private static List<CrGroup> RESERVATION_DB = new List<CrGroup>();
        //private static int LAST_RESERVATION_ID = 0;

        public static List<CrGroup> getCrGroupDb()
        {
            return RESERVATION_DB;
        }

        /*public static int getLastCrGroupId()
        {
            return LAST_RESERVATION_ID;
        }*/

        public static CrGroup addCrGroup(CrGroup reserve)
        {
            RESERVATION_DB.Add(reserve);
            return reserve;
        }

        /*public static CrGroup getCrGroup(int id)
        {
            foreach (CrGroup reserve in RESERVATION_DB)
            {
                if (reserve.Counter == id)
                {
                    return reserve;
                }
            }
            return null;
        }*/

        public static CrGroup getCrGroupName(string id)
        {
            return RESERVATION_DB.Find(x => x.Name == id);
        }

        public static bool removeCrGroup(string id)
        {
            RESERVATION_DB.Remove(RESERVATION_DB.Find(x => x.Name == id));
            return false;
        }

        public static bool clearCrGroup()
        {
            RESERVATION_DB.Clear();
            return true;
        }

        public static bool findCrGroup(string name)
        {
            return RESERVATION_DB.Exists(x => x.Name == name);
        }

        public static int findIndexPerson(string name, string cardNumber)
        {
            return RESERVATION_DB.Find(x => x.Name == name).People.FindIndex(x => x.CardNumber == cardNumber);
        }

    }
}
