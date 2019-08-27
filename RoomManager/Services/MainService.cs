using CtrNet_v;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomManager.Services
{
    /// <summary>
    /// Main service of all
    /// </summary>
    public class MainService
    {
        /// <summary>
        /// Build new object from .dll library
        /// </summary>
        Devices newCardReader = new Devices();

        /// <summary>
        /// Variable to check connection response
        /// </summary>
        public bool ConnectionStatus { get; set; } = false;

        /// <summary>
        /// Variable to check Message respond
        /// </summary>
        public string MessageReceived { get; set; } = "";

        /// <summary>
        /// Variable to check Message received status
        /// </summary>
        public bool ReceivedStatus { get; set; } = false;

        /// <summary>
        /// Variable to contruct response message
        /// </summary>
        public string HexMsg { get; set; } = "";

        public MainService()
        {

            newCardReader.ConnectFailed += new Devices.ConnectFailedEventHandler(NewDevice_ConnectFailed);
            newCardReader.ConnectSucceed += new Devices.ConnectSucceedEventHandler(NewDevice_ConnectSucceed);
            newCardReader.GetBufferData += new Devices.GetBufferDataHandler(NewDevice_GetBufferData);
            newCardReader.ShowEvent += new Devices.ShowEventEventHandler(NewDevice_ShowEvent);
        }


        /// <summary>
        /// Connection succed handler
        /// </summary>
        void NewDevice_ConnectSucceed()
        {
            ConnectionStatus = newCardReader.ConnectState;
        }

        /// <summary>
        /// Connection failed handler
        /// </summary>
        void NewDevice_ConnectFailed()
        {
            ConnectionStatus = newCardReader.ConnectState;
        }

        /// <summary>
        /// Response success handler
        /// </summary>
        private void NewDevice_GetBufferData(string CommandMode, byte[] FeedBack, string EventLog)
        {
            StringBuilder hex = new StringBuilder(FeedBack.Length * 2);

            foreach (byte b in FeedBack)
            {
                hex.AppendFormat("{0:x2}", b);
            }

            HexMsg = hex.ToString();

            string strMsg = Encoding.ASCII.GetString(FeedBack);

            string[] strSMsg = strMsg.Split(Convert.ToChar(0x0a));

            string timeLog = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");

            MessageReceived = HexMsg;

            Debug.WriteLine(HexMsg);
        }

        /// <summary>
        /// Response failed handler
        /// </summary>
        private void NewDevice_ShowEvent(string Events, string tResult)
        {
            MessageReceived = Events + " " + tResult + " FAILED";

            ReceivedStatus = false;

            MessageReceived = Events;

            Debug.WriteLine(Events);
        }

        /// <summary>
        /// Used to create connection to Card Reader
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns>True (Success) or False (Failed)</returns>
        public bool GetConnected(string ip, int port)
        {
            newCardReader.DisConnect();
            newCardReader.Cnt_IP = ip;
            newCardReader.Cnt_PortNo = port;
            newCardReader.ConnectMode = 0;
            newCardReader.Connect();
            return ConnectionStatus;
        }

        /// <summary>
        /// Add a person information such as Card Number and Group Number to Card Reader
        /// </summary>
        /// <param name="cardReader"></param>
        /// <param name="cardNum"></param>
        /// <param name="groupNum"></param>
        /// <returns>True (Success) or False (Failed)</returns>
        public bool AddNewPerson(string cardReader, string cardNum, int groupNum)
        {
            // ( Address, GroupNumber, CardNumber, Legal, AllTimePass, Password, CampusID, UserName, Timing )
            newCardReader.PAM(
                        cardReader,
                        groupNum.ToString().PadLeft(2, '0'),
                        cardNum,
                        true,
                        false,
                        "0000",
                        "00000000",
                        "00000000",
                        2000);
            if (MessageReceived == "54063030030d0a54473030030d0a")
            {
                MessageReceived = "";
                return true;
            }
            return false;
        }

        /// <summary>
        /// Remove person information such as Card Number and Group Number from Card Reader
        /// </summary>
        /// <param name="cardReader"></param>
        /// <param name="cardNum"></param>
        /// <param name="groupNum"></param>
        /// <returns>True (Success) or False (Failed)</returns>
        public bool RemovePerson(string cardReader, string cardNum, int groupNum)
        {
            // ( Address, GroupNumber, CardNumber, Legal, AllTimePass, Password, CampusID, UserName, Timing )
            newCardReader.DelPAM(
                cardReader,
                groupNum.ToString().PadLeft(2, '0'),
                cardNum,
                2000
                );
            if (MessageReceived == "54063030030d0a54473030030d0a")
            {
                MessageReceived = "";
                return true;
            }
            return false;
        }

        /// <summary>
        /// Add a timezone information such as Start Time, End Time, and Zone Number (as an Id)
        /// </summary>
        /// <param name="cardReader"></param>
        /// <param name="cardNum"></param>
        /// <param name="groupNum"></param>
        /// <returns>True (Success) or False (Failed)</returns>
        public bool AddNewTimeZone(string cardReader, long startTime, long endTime, int zoneNum)
        {
            // ( Address, ZoneNumber, StartTime, EndTime, Weekdays, Timing )
            string sTime = DateTimeOffset.FromUnixTimeSeconds(startTime).AddHours(8).ToString("HHmm");
            string eTime = DateTimeOffset.FromUnixTimeSeconds(endTime).AddHours(8).ToString("HHmm");

            newCardReader.TimeZone(
                        cardReader,
                        zoneNum.ToString().PadLeft(2, '0'),
                        sTime,
                        eTime,
                        "1111111",
                        2000);

            if (MessageReceived == "54063030030d0a54473030030d0a")
            {
                MessageReceived = "";
                return true;
            }
            return false;
        }

        /// <summary>
        /// Add an access group matching person's group number and zone number
        /// </summary>
        /// <param name="cardReader"></param>
        /// <param name="cardNum"></param>
        /// <param name="groupNum"></param>
        /// <returns>True (Success) or False (Failed)</returns>
        public bool AddNewAccessGroup(string cardReader, int groupNum, int zoneNum)
        {
            // ( Address, GroupNumber, ZoneNumber, Legal, AllTimPass, FF, Timing )


            newCardReader.AcsGroup(
                cardReader,
                groupNum.ToString().PadLeft(2, '0'),
                Convert.ToString(zoneNum, 2).PadLeft(8, '0'),
                true,
                false,
                "0",
                2000);
            if (MessageReceived == "54063030030d0a54473030030d0a")
            {
                MessageReceived = "";
                return true;
            }
            return false;
        }


        /// <summary>
        /// Set Card Reader date to Now
        /// </summary>
        /// <param name="cardReader"></param>
        /// <param name="cardNum"></param>
        /// <param name="groupNum"></param>
        /// <returns>True (Success) or False (Failed)</returns>
        public bool SetDate(string cardReader)
        {
            // ( Address, GroupNumber, ZoneNumber, Legal, AllTimPass, FF, Timing )
            newCardReader.Setting_Date(
                cardReader,
                DateTime.UtcNow.AddHours(8),
                2000);
            Debug.WriteLine(DateTime.UtcNow.AddHours(8).ToString());

            if (MessageReceived == "54063030030d0a")
            {
                MessageReceived = "";
                return true;
            }
            return false;
        }

        /// <summary>
        /// Set Card Reader time to Now
        /// </summary>
        /// <param name="cardReader"></param>
        /// <param name="cardNum"></param>
        /// <param name="groupNum"></param>
        /// <returns>True (Success) or False (Failed)</returns>
        public bool SetTime(string cardReader)
        {
            // ( Address, GroupNumber, ZoneNumber, Legal, AllTimPass, FF, Timing )
            newCardReader.Setting_Time(
                cardReader,
                DateTime.UtcNow.AddHours(8),
                2000);
            Debug.WriteLine(DateTime.UtcNow.AddHours(8).ToString());
            if (MessageReceived == "54063030030d0a")
            {
                MessageReceived = "";
                return true;
            }
            return false;
        }

        /// <summary>
        /// Set Card Reader flag to check user legal status
        /// </summary>
        /// <param name="cardReader"></param>
        /// <param name="cardNum"></param>
        /// <param name="groupNum"></param>
        /// <returns>True (Success) or False (Failed)</returns>
        public bool EnablePm(string cardReader)
        {
            // ( Address, GroupNumber, ZoneNumber, Legal, AllTimPass, FF, Timing )
            newCardReader.FunctionCode(
                cardReader,
                "1501",
                2000);
            if (MessageReceived == "54063030030d0a")
            {
                MessageReceived = "";
                return true;
            }
            return false;
        }

        /// <summary>
        /// Set Card Reader flag to check zone legal status
        /// </summary>
        /// <param name="cardReader"></param>
        /// <param name="cardNum"></param>
        /// <param name="groupNum"></param>
        /// <returns>True (Success) or False (Failed)</returns>
        public bool EnableZm(String cardReader)
        {
            // ( Address, GroupNumber, ZoneNumber, Legal, AllTimPass, FF, Timing )
            newCardReader.FunctionCode(
                cardReader,
                "1601",
                2000);
            if (MessageReceived == "54063030030d0a")
            {
                MessageReceived = "";
                return true;
            }
            return false;
        }

        /// <summary>
        /// Clear all user from Card Reader
        /// </summary>
        /// <param name="cardReader"></param>
        /// <param name="cardNum"></param>
        /// <param name="groupNum"></param>
        /// <returns>True (Success) or False (Failed)</returns>
        public bool ClearPm(String cardReader)
        {
            // ( Address, GroupNumber, ZoneNumber, Legal, AllTimPass, FF, Timing )
            newCardReader.FunctionCode(
                cardReader,
                "0000",
                2000);
            if (MessageReceived == "54063030030d0a54473030030d0a")
            {
                MessageReceived = "";
                return true;
            }
            return false;
        }

        /// <summary>
        /// Clear all time map from Card Reader
        /// </summary>
        /// <param name="cardReader"></param>
        /// <param name="cardNum"></param>
        /// <param name="groupNum"></param>
        /// <returns>True (Success) or False (Failed)</returns>
        public bool ClearZm(String cardReader)
        {
            // ( Address, GroupNumber, ZoneNumber, Legal, AllTimPass, FF, Timing )
            newCardReader.FunctionCode(
                cardReader,
                "0016",
                2000);
            if (MessageReceived == "54063030030d0a54473030030d0a")
            {
                MessageReceived = "";
                return true;
            }
            return false;
        }

        /// <summary>
        /// Clear all access group from Card Reader
        /// </summary>
        /// <param name="cardReader"></param>
        /// <param name="cardNum"></param>
        /// <param name="groupNum"></param>
        /// <returns>True (Success) or False (Failed)</returns>
        public bool ClearAg(String cardReader)
        {
            // ( Address, GroupNumber, ZoneNumber, Legal, AllTimPass, FF, Timing )
            newCardReader.FunctionCode(
                cardReader,
                "0072",
                2000);
            if (MessageReceived == "54063030030d0a54473030030d0a")
            {
                MessageReceived = "";
                return true;
            }
            return false;
        }

        /// <summary>
        /// Immediately open the door
        /// </summary>
        /// <param name="cardReader"></param>
        /// <param name="cardNum"></param>
        /// <param name="groupNum"></param>
        /// <returns>True (Success) or False (Failed)</returns>
        public bool OpenDoor(String cardReader)
        {
            // ( Address, GroupNumber, ZoneNumber, Legal, AllTimPass, FF, Timing )
            newCardReader.OpenDoor(
                cardReader,
                2000);
            if (MessageReceived == "54063030030d0a")
            {
                MessageReceived = "";
                return true;
            }
            return false;
        }

        /// <summary>
        /// Get user tap counting from Card Reader
        /// </summary>
        /// <param name="cardReader"></param>
        /// <param name="cardNum"></param>
        /// <param name="groupNum"></param>
        /// <returns>True (Success) or False (Failed)</returns>
        public string GetCounting(String cardReader)
        {
            // ( Address, GroupNumber, ZoneNumber, Legal, AllTimPass, FF, Timing )
            newCardReader.CheckCI(
                cardReader,
                2000);
            if (MessageReceived.Length == "54063030030d0a023030523030303037573030303837470d0a".Length)
            {
                string respond = MessageReceived;
                MessageReceived = "";

                return respond;
            }
            return null;
        }

        /// <summary>
        /// Clear user tap counting
        /// </summary>
        /// <param name="cardReader"></param>
        /// <param name="cardNum"></param>
        /// <param name="groupNum"></param>
        /// <returns>True (Success) or False (Failed)</returns>
        public bool ClearCo(String cardReader)
        {
            // ( Address, GroupNumber, ZoneNumber, Legal, AllTimPass, FF, Timing )
            newCardReader.Setting_Counter(
                cardReader,
                0,
                "AAAAA",
                2000);
            if (MessageReceived == "54063030030d0a")
            {
                MessageReceived = "";
                return true;
            }
            return false;
        }
    }
}
