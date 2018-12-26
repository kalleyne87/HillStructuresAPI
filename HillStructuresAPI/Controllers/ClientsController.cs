using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
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
                var clients = _context.Client.ToList();
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
                var client = _context.Client.SingleOrDefault(p => p.UserID == userID);
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

        /*// GET: Clients
        [Authorize]
        public async Task<IActionResult> Index()
        {
            if(_signInManager.IsSignedIn(User))
            {
                return View(await _context.Client.ToListAsync());
            } else 
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(_signInManager.IsSignedIn(User))
            {
                if (id == null)
                {
                    return NotFound();
                }

                var client = await _context.Client
                    .FirstOrDefaultAsync(m => m.UserID == id);


                if (client == null)
                {
                    return NotFound();
                }
                IQueryable<Job> currentjobs = _context.Job
                    .Where(m => m.Client.UserID == client.UserID);                
                ViewBag.currentJobs = currentjobs;
                return View(client);
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }
        }

        // GET: Clients/Create
        public IActionResult Create()
        {      
                return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserID,FirstName,LastName,PhoneNumber,EmailAddress,Address")] Client client)
        {
            if(_signInManager.IsSignedIn(User))
            {               
                if (ModelState.IsValid)
                {
                    _context.Add(client);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(client);
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }
        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if(_signInManager.IsSignedIn(User))
            {                   
                if (id == null)
                {
                    return NotFound();
                }

                var client = await _context.Client.FindAsync(id);
                if (client == null)
                {
                    return NotFound();
                }
                return View(client);
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserID,FirstName,LastName,PhoneNumber,EmailAddress,Address")] Client client)
        {
            if(_signInManager.IsSignedIn(User))
            {             
                if (id != client.UserID)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(client);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ClientExists(client.UserID))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(client);
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }
        }

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(_signInManager.IsSignedIn(User))
            {  
                if (id == null)
                {
                    return NotFound();
                }

                var client = await _context.Client
                    .FirstOrDefaultAsync(m => m.UserID == id);
                if (client == null)
                {
                    return NotFound();
                }
                return View(client);
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }            
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if(_signInManager.IsSignedIn(User))
            {
                var client = await _context.Client.FindAsync(id);
                var clientjoblist = _context.Job.Where(j => j.Client.UserID == id).ToList();
                if(!(clientjoblist == null) || clientjoblist.Count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                _context.Client.Remove(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }              
        }

        private bool ClientExists(int id)
        {
            return _context.Client.Any(e => e.UserID == id);
        }

        // GET: Clients/AddJobs/5
        public async Task<IActionResult> AddJobs(int? id)
        {
            if(_signInManager.IsSignedIn(User))
            {
                if (id == null)
                {
                    return NotFound();
                }

                var client = await _context.Client.FindAsync(id);
                List<Job> joblist = new List<Job>();           
                joblist = _context.Job.ToList();
                if(!(client.Jobs == null) && client.Jobs.Count > 0 && joblist.Count > 0)
                {
                    foreach(var j in client.Jobs)
                    {
                        foreach(var x in joblist)
                        {
                            if(j.JobID == x.JobID)
                            {
                                joblist.Remove(x);
                                break;
                            }
                        }
                    }
                }
                ViewBag.selectedJobID = joblist;

                if (client == null)
                {
                    return NotFound();
                }
                return View(client);
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }              
        }

        // POST: Clients/AddJobs/2/3
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddJobs(int id, int selectedJobID)
        {
            if(_signInManager.IsSignedIn(User))
            {
                var client = await _context.Client.FindAsync(id);
                var selectedJob = await _context.Job.FindAsync(selectedJobID);
                if(client.Jobs == null) 
                {
                    client.Jobs = new List<Job>();
                }
                client.Jobs.Add(selectedJob);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }             
        }*/
    }
        }