using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoomManager.Models;
using RoomManager.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace RoomManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("This controller used for Reservation System")]
    public class ReservationsController : ControllerBase
    {
        ReservationService reserve = new ReservationService();


        // GET: api/Reservations
        [HttpGet]
        [SwaggerOperation(
            Summary = "For Testing",
            Description = "Test The Controller is working or not"
        )]
        public string Get()
        {
            return "Reservation's controller is working!";
        }

        // GET: api/Reservations/5
        /*[HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }*/

        // POST: api/Reservations
        [HttpPost]
        [SwaggerOperation(
            Summary = "Start the reservation service",
            Description = "Requires admin privileges. Once this request sent the service will start. Cannot start more than one reservation service."
        )]
        public IActionResult Post(Automation auto)
        {
            try
            {
                if (SystemStatus.RsvSvc == false)
                {
                    if (auto.Command == "start")
                    {
                        reserve.DayChecker(auto.Command);
                        SystemStatus.RsvSvc = true;
                        return Ok("Starting service...");
                    }
                    return BadRequest("Service can not start");
                }
                return Conflict("Service already started");
            } catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT: api/Reservations/5
       /* [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }*/

        // DELETE: api/ApiWithActions/5
        /*[HttpDelete("{id}")]
        public void Delete(int id)
        {
        }*/
    }
}
