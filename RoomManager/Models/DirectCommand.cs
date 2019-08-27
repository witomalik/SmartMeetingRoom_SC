using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomManager.Models
{
    public class DirectCommand
    {
        public string Command { get; set; }
        public string CardNum { get; set; }
        public string FuncNo { get; set; }
        public CardReader CardReader { get; set; }
    }
}
