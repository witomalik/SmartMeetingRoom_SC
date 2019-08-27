using CtrNet_v;
using RestSharp;
using RoomManager.Models;
using RoomManager.Databases;
using RoomManager.ResponseModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;

namespace RoomManager.Services
{
    public class ReservationService : MainService
    {
        /// <summary>
        /// Constant variable to set period of EnableRoom function to be executed
        /// </summary>
        private const int Period = 86400000;

        /// <summary>
        /// Variable to store temp memory of counting data
        /// </summary>
        private string gotCounting;


        //public List<CrGroup> CrName = new List<CrGroup>();
        //public List<PrGroup> PrName = new List<PrGroup>();

        /// <summary>
        /// Function to calculate initial (12.00 AM) time into ms
        /// </summary>
        /// <returns></returns>
        public long initTime()
        {
            DateTime dateTime = DateTime.Today.AddDays(1);
            return Math.Abs(((DateTimeOffset)dateTime).ToUnixTimeSeconds() - DateTimeOffset.UtcNow.ToUnixTimeSeconds()) * 1000;
        }

        /// <summary>
        /// Function to count End Time as a countdown for sending Tap Count logs
        /// </summary>
        /// <param name="eTime"></param>
        /// <returns></returns>
        public long countTime(long eTime)
        {
            long result = Math.Abs(eTime - DateTimeOffset.UtcNow.ToUnixTimeSeconds()) * 1000;
            Debug.WriteLine(result);
            return result;
        }

        /// <summary>
        /// Funtion to create timer object and start them 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public int DayChecker(string command)
        {
            if (command == "start")
            {
                //new Timer(EnableRoom, null, 0, Timeout.Infinite);
                
                Timer _timer = new Timer(EnableRoom, null, 0, Timeout.Infinite);

                _timer.Change(initTime(), Period);
                
                Debug.WriteLine("Current thread" + Thread.CurrentThread.ManagedThreadId);
                return Thread.CurrentThread.ManagedThreadId;
                
            }
            return 0;
        }

        /// <summary>
        /// Function for process count tap from Card Reader to an readable integer
        /// </summary>
        /// <param name="stateInfo"></param>
        public void CountTap(Object stateInfo)
        {
            Reservation reservation = (Reservation)stateInfo;

            

            do
            {
                while (!GetConnected(reservation.CardReader.IP, reservation.CardReader.Port)) ;
                gotCounting = GetCounting(reservation.CardReader.CrAddress);
            } while (gotCounting == null);

            string trimmedCount = gotCounting.Remove(0, 34).Remove(10);
            

            string res = String.Empty;

            for (int a = 0; a < trimmedCount.Length; a = a + 2)

            {

                string Char2Convert = trimmedCount.Substring(a, 2);

                int n = Convert.ToInt32(Char2Convert, 16);

                char c = (char)n;

                res += c.ToString();

                
            }

            String str = res.TrimStart('0');
            

            Tap tapCounting = new Tap() { Person = reservation.Person.Campus_ID, StartTime = reservation.StartTime, EndTime = reservation.EndTime, TapCount = int.Parse(str) };

            var client = new RestClient("http://140.118.123.95:5555");
            var request = new RestRequest("/smr/records/add", Method.POST);

            string jsonToSend = SimpleJson.SerializeObject(tapCounting);

            request.AddParameter("application/json; charset=utf-8", jsonToSend, ParameterType.RequestBody);
            request.RequestFormat = DataFormat.Json;

            IRestResponse response = client.Execute(request);

            if (response.IsSuccessful)
            {
                Debug.WriteLine("ok");
            }

            while (!ClearCo(reservation.CardReader.CrAddress)) ;
        }


        /// <summary>
        /// Function executed every time the timer is (12.00 AM)
        /// </summary>
        /// <param name="stateInfo"></param>
        public void EnableRoom(Object stateInfo)
        {
            
            /*GroupNumber = 0;
            ZoneNumber = 0;*/
            var client = new RestClient("http://140.118.123.95:5555");
            var request = new RestRequest("smr/reservations/today", Method.GET);
            IRestResponse response = client.Execute(request);

            if (response.IsSuccessful)
            {


                foreach (CrGroup crGroup in CrName.getCrGroupDb())
                {
                    foreach (Person person in crGroup.People)
                    {
                        while (!GetConnected(crGroup.IP, crGroup.Port)) ;
                        while (!RemovePerson(crGroup.Addr, person.CardNumber, crGroup.People.IndexOf(person) + 1));
                    }
                }

                CrName.clearCrGroup();

                ReservationResponse resList = SimpleJson.DeserializeObject<ReservationResponse>(response.Content);
                
                foreach (Reservation reservation in resList.Data)
                {
                    while (!GetConnected(reservation.CardReader.IP, reservation.CardReader.Port)) ;

                    if (CreateBooking(reservation))
                    {
                        Timer _countTimer = new Timer(CountTap, reservation, countTime(reservation.EndTime), Timeout.Infinite);
                    }

                }
            }
            
        }

        /// <summary>
        /// Function for immediately book a room today
        /// </summary>
        /// <param name="reservation"></param>
        /// <returns></returns>
        public bool BookRoomNow(Reservation reservation)
        {
            if (!GetConnected(reservation.CardReader.IP, reservation.CardReader.Port))
            {
                return false;
            }
            return CreateBooking(reservation);
        }

        /// <summary>
        /// Function for creating packet of booking to card reader
        /// </summary>
        /// <param name="reservation"></param>
        /// <returns></returns>
        public bool CreateBooking (Reservation reservation)
        {
            if (reservation.Person.CardNumber.Length == 8)
            {
                if (!CrName.findCrGroup(reservation.CardReader.Name))
                {
                    //while (!SetDate(reservation.CardReader.CrAddress)) ;
                    //while (!SetTime(reservation.CardReader.CrAddress)) ;

                    //while (!ClearPm(reservation.CardReader.CrAddress)) ;
                    while (!ClearZm(reservation.CardReader.CrAddress)) ;
                    while (!ClearAg(reservation.CardReader.CrAddress)) ;

                    while (!EnablePm(reservation.CardReader.CrAddress)) ;
                    while (!EnableZm(reservation.CardReader.CrAddress)) ;

                    CrName.addCrGroup(new CrGroup() { Name = reservation.CardReader.Name, Addr = reservation.CardReader.CrAddress, IP = reservation.CardReader.IP, Port = reservation.CardReader.Port });
                }

                if (!CrName.getCrGroupName(reservation.CardReader.Name).People.Exists(x => x.Campus_ID == reservation.Person.Campus_ID))
                {
                    CrName.getCrGroupName(reservation.CardReader.Name).People.Add(reservation.Person);

                    int groupNum = CrName.findIndexPerson(reservation.CardReader.Name, reservation.Person.CardNumber) + 1;

                    while (!AddNewPerson(reservation.CardReader.CrAddress, reservation.Person.CardNumber, groupNum)) ;

                    while (!AddNewTimeZone(reservation.CardReader.CrAddress, reservation.StartTime, reservation.EndTime, groupNum)) ;
                    while (!AddNewAccessGroup(reservation.CardReader.CrAddress, groupNum, groupNum)) ;
                    return true;
                }
            }
            return false;
        }

    }
}
