using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomManager.Models
{
    /// <summary>
    /// This model is used in Direct Command service
    /// </summary>
    public class Automation
    {
        /// <summary>
        /// Required attribute used in Direct Command service
        /// </summary>
        public string Command { get; set; }
    }
}
