using CtrNet_v;
using RoomManager.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomManager.Services
{
    public class ManagementService : MainService
    {
      

        public bool AddNewAdmin(string IP, string cardNum)
        {
            while (!AddNewPerson(IP, cardNum, 95)) ;
            return true;
        }

        public bool RemoveAdmin(string addr, string cardNum)
        {
            while (!RemovePerson(addr, cardNum, 95)) ;
            return true;
        }

        public string Commander(DirectCommand command)
        {
            if (!GetConnected(command.CardReader.IP, command.CardReader.Port))
            {
                return ("Failed to connect! Please check and try again.");
            }
            switch (command.Command)
            {
                case "OpnDoor":
                    while (!OpenDoor(command.CardReader.CrAddress)) ;
                    return ("The door is open now!");
                case "AddAdmin":
                    AddNewAdmin(command.CardReader.IP, command.CardNum);
                    return ("add " + command.CardNum + " as admin!");
                case "ClrUser":
                    while (!ClearPm(command.CardReader.CrAddress)) ;
                    return ("Delete all user has done!");
                case "DelAdmin":
                    RemoveAdmin(command.CardReader.CrAddress, command.CardNum);
                    return ("Delete admin has done!");
                case "RstCr":
                    while (!SetDate(command.CardReader.CrAddress)) ;
                    while (!SetTime(command.CardReader.CrAddress)) ;

                    while (!ClearPm(command.CardReader.CrAddress)) ;
                    while (!ClearZm(command.CardReader.CrAddress)) ;
                    while (!ClearAg(command.CardReader.CrAddress)) ;
                    while (!ClearCo(command.CardReader.CrAddress)) ;

                    while (!EnablePm(command.CardReader.CrAddress)) ;
                    while (!EnableZm(command.CardReader.CrAddress)) ;
                    return ("Reset has done!");
                case "SetDate":
                    while (!SetDate(command.CardReader.CrAddress)) ;
                    return ("Set date has done!");
                case "SetTime":
                    while (!SetTime(command.CardReader.CrAddress)) ;
                    return ("Set time has done!");
            }
            return ("Command error!");
        }
    }
}
