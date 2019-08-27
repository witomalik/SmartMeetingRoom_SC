using RoomManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomManager.Databases
{
    public class CrName
    {
        /// <summary>
        /// This Class is used as local memory to store temporary data of every reservation
        /// </summary>
        private static List<CrGroup> RESERVATION_DB = new List<CrGroup>();
        //private static int LAST_RESERVATION_ID = 0;


        /// <summary>
        /// To get all data from List of CrGroup
        /// </summary>
        /// <returns>
        /// List of CrGroup
        /// </returns>
        public static List<CrGroup> getCrGroupDb()
        {
            return RESERVATION_DB;
        }

        /*public static int getLastCrGroupId()
        {
            return LAST_RESERVATION_ID;
        }*/

        /// <summary>
        /// To add new CrGroup into RESERVATION_DB
        /// </summary>
        /// <param name="reserve"></param>
        /// <returns>Added CrGroup to DB</returns>
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

        /// <summary>
        /// Find the CrGroup by card reader name
        /// </summary>
        /// <param name="id"></param>
        /// <returns>It will return CrGroup</returns>
        public static CrGroup getCrGroupName(string id)
        {
            return RESERVATION_DB.Find(x => x.Name == id);
        }

        /// <summary>
        /// Remove CrGroup on the List
        /// </summary>
        /// <param name="id"></param>
        /// <returns>It will return true(success) or false(failed)</returns>
        public static bool removeCrGroup(string id)
        {
            RESERVATION_DB.Remove(RESERVATION_DB.Find(x => x.Name == id));
            return false;
        }

        /// <summary>
        /// Delete all CrGroup on the List
        /// </summary>
        /// <returns>It will return return true(success) or false(failed)</returns>
        public static bool clearCrGroup()
        {
            RESERVATION_DB.Clear();
            return true;
        }

        /// <summary>
        /// Find that CrGroup Exist on List
        /// </summary>
        /// <param name="name"></param>
        /// <returns>It will return True (Exist) False (Not Exist)</returns>
        public static bool findCrGroup(string name)
        {
            return RESERVATION_DB.Exists(x => x.Name == name);
        }

        /// <summary>
        /// Find the index of crgroup as the id
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cardNumber"></param>
        /// <returns>It will retrun index as Id</returns>
        public static int findIndexPerson(string name, string cardNumber)
        {
            return RESERVATION_DB.Find(x => x.Name == name).People.FindIndex(x => x.CardNumber == cardNumber);
        }

    }
}
