using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HillStructuresAPI.Models;
using Microsoft.AspNetCore.Cors;


namespace HillStructuresAPI.Controllers
{

    [Route("api/Client")]
    public class ClientsController : ControllerBase
    {
        private readonly HillStructuresContext _context;

        public ClientsController(HillStructuresContext context)
        {
            _context = context;
        }

        // GET api/Clients/get
        [EnableCors("AllowAll")]
        [Produces("application/json")]
        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var clients = _context.Client.Include(c => c.Jobs).ToList();
                return Ok(clients);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        // GET api/Clients/get/2
        [EnableCors("AllowAll")]
        [Produces("application/json")]
        [HttpGet("get/{userID}")]
        public async Task<IActionResult> Get(int userID)
        {
            try
            {
                var client = _context.Client.Include(c => c.Jobs)
                    .SingleOrDefault(p => p.UserID == userID);
                return Ok(client);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        // POST api/Clients/create
        [EnableCors("AllowAll")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] Client client)
        {
            try
            {
                _context.Client.Add(client);
                _context.SaveChanges();
                return Ok(client);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        // PUT api/Clients/update
        [EnableCors("AllowAll")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] Client client)
        {
            try
            {
                _context.Entry(client).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
                return Ok(client);
            }
            catch (Exception e)
            {
                return BadRequest(e);

            }
        }

        // DELETE api/Clients/delete/3
        [EnableCors("AllowAll")]
        [HttpDelete("delete/{userID}")]
        public async Task<IActionResult> Delete(int userID)
        {
            try
            {
                _context.Remove(_context.Client.Find(userID));
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}