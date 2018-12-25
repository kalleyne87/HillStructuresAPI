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
using HillStructuresAPI.Models;


namespace HillStructuresAPI.Controllers
{
    public class TimeSheetsController : Controller
    {

        private readonly HillStructuresContext _context;
        private readonly SignInManager<SuperUser> _signInManager;             

        public TimeSheetsController(HillStructuresContext context,
                                        SignInManager<SuperUser> signInManager)
        {
            _context = context;
            _signInManager = signInManager;              
        }

        // GET: TimeSheet/Delete/5
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
        }
    }
}