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
    [SwaggerTag("This controller used for Direct Control")]
    public class DirectsController : ControllerBase
    {
        ManagementService management = new ManagementService();

        /*[Route("api/Directs/OpenDoorNow")]
        [HttpPost]
        public IActionResult Post(DirectCommand directCommand)
        {
            if (management.OpenDoorNow(directCommand.CardReader))
            {
                return Ok("The door just opened!");
            }
            return BadRequest("The door can not be opened!");
        }*/

        // GET: api/Directs
        [HttpGet]
        [SwaggerOperation(
            Summary = "For Testing",
            Description = "Test The Controller is working or not"
        )]
        public string Get()
        {
            return "Direct's controller is working!";
        }

        // GET: api/Directs/5
        /*[HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }*/

        // POST: api/Directs
        [HttpPost]
        [SwaggerOperation(
            Summary = "Execute direct command to Card Reader",
            Description = "Requires admin privileges"
        )]
        public IActionResult Post(DirectCommand directCommand)
        {
            try
            {
                if (directCommand.Command != null)
                {
                    return Ok(management.Commander(directCommand));
                }
                return BadRequest("failed");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }

        // PUT: api/Directs/5
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
