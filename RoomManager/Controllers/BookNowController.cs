using System;
using System.Collections.Generic;
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
    [SwaggerTag("This controller used for immediately create reservation")]
    public class BookNowController : ControllerBase
    {
        ReservationService reserve = new ReservationService();

        // GET: api/BookNow
        [HttpGet]
        [SwaggerOperation(
            Summary = "For Testing",
            Description = "Test The Controller is working or not"
        )]
        public string Get()
        {
            return "BookNow's controller is working!";
        }

        // GET: api/BookNow/5
        /*[HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }*/

        // POST: api/BookNow
        [HttpPost]
        [SwaggerOperation(
            Summary = "Immediately create reservation",
            Description = "Requires admin privileges. Only used on the same date. Reservation service must be started"
        )]
        public IActionResult Post(Reservation reservations)
        {
            try
            {
                if (SystemStatus.RsvSvc == true)
                {

                    long sDay = ((DateTimeOffset)DateTime.Today).ToUnixTimeSeconds();
                    long eDay = ((DateTimeOffset)DateTime.Today.AddDays(1)).ToUnixTimeSeconds();

                    if (reservations.StartTime > sDay && reservations.EndTime < eDay)
                    {
                        if (reserve.BookRoomNow(reservations))
                        {
                            return Ok("Direct booking created!");
                        } else
                        {
                            return BadRequest("Failed to create booking! please check and try again.");
                        }
                    }
                    return Conflict("Cannot create direct booking! (Only for booking today)");
                }
                return Conflict("Cannot create direct booking! (Reservation service is not started)");
            } catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }

        // PUT: api/BookNow/5
        /*[HttpPut("{id}")]
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
