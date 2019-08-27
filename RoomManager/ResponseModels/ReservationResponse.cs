using RoomManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomManager.ResponseModels
{
    public class ReservationResponse
    {
        public bool Flag { get; set; }
        public int Code { get; set; }
        public List<Reservation> Data { get; set; }
        public string result { get; set; }
    }
}
