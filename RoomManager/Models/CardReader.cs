using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomManager.Models
{
    /// <summary>
    /// Model of card reader data
    /// </summary>
    public class CardReader
    {
        /// <summary>
        /// IP Address of CardReader ex: 140.118.122.121
        /// </summary>
        public string IP { get; set; }
        
        /// <summary>
        /// Card Reader Address ex: 00
        /// </summary>
        public string CrAddress { get; set; }

        /// <summary>
        /// Card Reader name ex: EE-809
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Card Reader port ex: 4001
        /// </summary>
        public int Port { get; set; }
    }
}
