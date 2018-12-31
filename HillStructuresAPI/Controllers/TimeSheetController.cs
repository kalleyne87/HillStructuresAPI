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
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using HillStructuresAPI.Models;


namespace HillStructuresAPI.Controllers
{
    [Route("api/TimeSheet")]
    public class TimeSheetsController : ControllerBase
    {

        private readonly HillStructuresContext _context;          

        public TimeSheetsController(HillStructuresContext context)
        {
            _context = context;       
        }

        // GET api/timesheets/get
        [EnableCors("AllowAll")]
        [Produces("application/json")]
        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var timesheets = _context.TimeSheets
                    .Include(ts => ts.Employee)
                    .Include(ts => ts.Job)
                    .Include(ts => ts.TimeSheetDetails)
                    .ToList();
                return Ok(timesheets);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        // GET api/timesheets/get/2
        [EnableCors("AllowAll")]
        [Produces("application/json")]
        [HttpGet("Get/{TimeSheetID}")]
        public async Task<IActionResult> Get(int TimeSheetID)
        {
            try
            {
                var timesheet = _context.TimeSheets
                    .Include(ts => ts.Employee)
                    .Include(ts => ts.Job)
                    .Include(ts => ts.TimeSheetDetails)
                    .SingleOrDefault(p => p.TimeSheetID == TimeSheetID);
                return Ok(timesheet);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        // POST api/timesheets/create
        [EnableCors("AllowAll")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] TimeSheet timeSheet)
        {
            try
            {
                _context.TimeSheets.Add(timeSheet);
                _context.SaveChanges();
                return Ok(timeSheet);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        // PUT api/timesheets/update
        [EnableCors("AllowAll")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] TimeSheet timeSheet)
        {
            try
            {
                _context.Entry(timeSheet).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
                return Ok(timeSheet);
            }
            catch (Exception e)
            {
                return BadRequest(e);

            }
        }

        // DELETE api/timesheets/delete/4
        [EnableCors("AllowAll")]
        [HttpDelete("Delete/{TimeSheetID}")]
        public async Task<IActionResult> Delete(int TimeSheetID)
        {
            try
            {
                _context.Remove(_context.TimeSheets.Find(TimeSheetID));
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        /*// GET: TimeSheet/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(_signInManager.IsSignedIn(User))
            {              
                if (id == null)
                {
                    return NotFound();
                }

                var timesheet = await _context.TimeSheets
                    .FirstOrDefaultAsync(m => m.TimeSheetID == id);
                if (timesheet == null)
                {
                    return NotFound();
                }
                ViewBag.timeSheetDisplay = setTimeSheet(timesheet);
                return View(timesheet);
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }          
        }

        // POST: PaymentSheet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if(_signInManager.IsSignedIn(User))
            {               
                var timeSheet = await _context.TimeSheets.FindAsync(id);
                _context.TimeSheets.Remove(timeSheet);
                await _context.SaveChangesAsync();
                return RedirectToAction("JobEmployeeDetail", "Employees", new { jobID = timeSheet.JobID, employeeID = timeSheet.EmployeeID });      
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }                   
        }

        private TimeSheetDisplay setTimeSheet(TimeSheet timesheet) 
        {
            TimeSheetDisplay tsheetdisplay = new TimeSheetDisplay();
            tsheetdisplay.WeekName = _context.Week.First(w => w.EndOfWeekDate == timesheet.WeekEndingDate).WeekName;
            timesheet.TimeSheetDetails = addingTimeSheetDetail(timesheet);
            tsheetdisplay.TimeSheetID = timesheet.TimeSheetID;
            tsheetdisplay.PayRate = timesheet.payrate;                              
            foreach(var timesheetdetail in timesheet.TimeSheetDetails)
            {
                switch(timesheetdetail.dayOfWeek)
                {
                    case 0:
                        tsheetdisplay.Sunday = timesheetdetail.hours;
                        break;
                    case 1:
                        tsheetdisplay.Monday = timesheetdetail.hours;
                        break;                                                            
                    case 2:
                        tsheetdisplay.Tuesday = timesheetdetail.hours;
                        break;   
                    case 3:
                        tsheetdisplay.Wednesday = timesheetdetail.hours;
                        break;                               
                    case 4:
                        tsheetdisplay.Thursday = timesheetdetail.hours;
                        break;                               
                    case 5:
                        tsheetdisplay.Friday = timesheetdetail.hours;
                        break;   
                    case 6:
                        tsheetdisplay.Saturday = timesheetdetail.hours;
                        break;   
                }
                tsheetdisplay.TotalHours += timesheetdetail.hours;
            }    
            tsheetdisplay.TotalPayout = Math.Round(tsheetdisplay.TotalHours * tsheetdisplay.PayRate);            
            return tsheetdisplay;
        }

        private ICollection<TimeSheetDetail> addingTimeSheetDetail(TimeSheet timesheet)
        {
            var timesheetdetails = _context.TimeSheetDetails.Where(t => t.TimeSheetID == timesheet.TimeSheetID).ToList();
            return timesheetdetails;
        }*/
    }
}