using CtrNet_v;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomManager.Services
{
    public class MainService
    {
        Devices newCardReader = new Devices();

        public bool ConnectionStatus { get; set; } = false;
        public string MessageReceived { get; set; } = "";
        public bool ReceivedStatus { get; set; } = false;
        public string HexMsg { get; set; } = "";

        public MainService()
        {
            newCardReader.ConnectFailed += new Devices.ConnectFailedEventHandler(NewDevice_ConnectFailed);
            newCardReader.ConnectSucceed += new Devices.ConnectSucceedEventHandler(NewDevice_ConnectSucceed);
            newCardReader.GetBufferData += new Devices.GetBufferDataHandler(NewDevice_GetBufferData);
            newCardReader.ShowEvent += new Devices.ShowEventEventHandler(NewDevice_ShowEvent);
        }

        void NewDevice_ConnectSucceed()
        {
            ConnectionStatus = newCardReader.ConnectState;
        }

        void NewDevice_ConnectFailed()
        {
            ConnectionStatus = newCardReader.ConnectState;
        }

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

        private void NewDevice_ShowEvent(string Events, string tResult)
        {
            MessageReceived = Events + " " + tResult + " FAILED";

            ReceivedStatus = false;

            MessageReceived = Events;

            Debug.WriteLine(Events);
        }

        public bool GetConnected(string ip, int port)
        {
            newCardReader.DisConnect();
            newCardReader.Cnt_IP = ip;
            newCardReader.Cnt_PortNo = port;
            newCardReader.ConnectMode = 0;
            newCardReader.Connect();
            return ConnectionStatus;
        }

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
