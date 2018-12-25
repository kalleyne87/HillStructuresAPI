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
    public class EmployeesController : Controller
    {
        private readonly HillStructuresContext _context;
        private readonly SignInManager<SuperUser> _signInManager;        

        public EmployeesController(HillStructuresContext context,
                                   SignInManager<SuperUser> signInManager)
        {
            _context = context;
            _signInManager = signInManager;            
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            if(_signInManager.IsSignedIn(User))
            {
                var employees = await _context.Employee.ToListAsync();

                foreach (var employee in employees)
                {
                    loadingEmployeeJobs(employee);
                }
                return View(employees);
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }   
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(_signInManager.IsSignedIn(User))
            {            
                if (id == null)
                {
                    return NotFound();
                }

                var employee = await _context.Employee
                    .FirstOrDefaultAsync(m => m.UserID == id);
                if (employee == null)
                {
                    return NotFound();
                }
                IQueryable<EmployeeJob> employeeJobs = _context.EmployeeJobs.Where(m => m.UserID == id);
                IList<Job> jobs = await _context.Job.ToListAsync();
                List<Job> listCurrentJobs = new List<Job>();
                foreach(var item in jobs)
                {
                    foreach(var item2 in employeeJobs)
                    {
                        if(item.JobID == item2.JobID)
                        {
                            listCurrentJobs.Add(item);
                        }
                    }
                }
                ViewBag.currentJobs = listCurrentJobs; 
                return View(employee);
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }              
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            if(_signInManager.IsSignedIn(User))
            {               
                return View();
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }     
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserID,FirstName,LastName,PhoneNumber,EmailAddress,Role")] Employee employee)
        {
            if(_signInManager.IsSignedIn(User))
            {                 
                if (ModelState.IsValid)
                {
                    _context.Add(employee);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(employee);
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }              
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if(_signInManager.IsSignedIn(User))
            {  
                if (id == null)
                {
                    return NotFound();
                }

                var employee = await _context.Employee.FindAsync(id);
                if (employee == null)
                {
                    return NotFound();
                }
                return View(employee);
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }   
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserID,FirstName,LastName,PhoneNumber,EmailAddress,Role")] Employee employee)
        {
            if(_signInManager.IsSignedIn(User))
            {                       
                if (id != employee.UserID)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(employee);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!EmployeeExists(employee.UserID))
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
                return View(employee);
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }     

        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(_signInManager.IsSignedIn(User))
            {               
                if (id == null)
                {
                    return NotFound();
                }

                var employee = await _context.Employee
                    .FirstOrDefaultAsync(m => m.UserID == id);
                if (employee == null)
                {
                    return NotFound();
                }
                return View(employee);
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }  
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if(_signInManager.IsSignedIn(User))
            {                  
                var employee = await _context.Employee.FindAsync(id);
                loadingEmployeeJobs(employee);
                loadingTimeSheets(employee);
                foreach(var ts in employee.TimeSheets)
                {
                    _context.TimeSheets.Remove(ts);
                }
                foreach(var tsd in employee.EmployeeJobs)
                {
                    _context.EmployeeJobs.Remove(tsd);
                }
                _context.Employee.Remove(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }  
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employee.Any(e => e.UserID == id);
        }


        // GET: Employees/AddJobs/5
        public async Task<IActionResult> AddJobs(int? id)
        {
            if(_signInManager.IsSignedIn(User))
            {  
                if (id == null)
                {
                    return NotFound();
                }

                var employee = await _context.Employee.FindAsync(id);
                List<Job> joblist = _context.Job.ToList();        
                List<EmployeeJob> employeejoblist = _context.EmployeeJobs.ToList();
                if(!(employeejoblist == null) && employeejoblist.Count > 0 && joblist.Count > 0)
                {
                    foreach(var ej in employeejoblist)
                    {
                        foreach(var j in joblist)
                        {
                            if(ej.JobID == j.JobID && ej.UserID == employee.UserID)
                            {
                                joblist.Remove(j);
                                break;
                            }
                        }
                    }
                }
                ViewBag.selectedJobID = joblist;

                if (employee == null)
                {
                    return NotFound();
                }
                return View(employee);
            } else 
            {
                return RedirectToAction("Login", "Account");                
            } 
        }

        // POST: Employees/AddJobs/2/3
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddJobs(int id, int selectedJobID)
        {
            if(_signInManager.IsSignedIn(User))
            {              
                EmployeeJob employeejob = new EmployeeJob();
                employeejob.UserID = id;
                employeejob.JobID = selectedJobID; 
                _context.EmployeeJobs.Add(employeejob);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }             
        }

        // GET: Employees/AddTimeSheet/5
        public async Task<IActionResult> AddTimeSheet(int? id)
        {
            if(_signInManager.IsSignedIn(User))
            {               
                if (id == null)
                {
                    return NotFound();
                }

                var employee = await _context.Employee.FindAsync(id);
                
                if (employee == null)
                {
                    return NotFound();
                }            
                ViewBag.employeeid = employee.UserID;
                ViewBag.selectedYear = _context.Year.ToList();     
                List<Job> jobs =  _context.Job.ToList();
                List<Job> joblist = new List<Job>();
                List<EmployeeJob> employeejoblist = _context.EmployeeJobs.ToList();
                if(!(employeejoblist == null))
                {
                    foreach(var ej in employeejoblist)
                    {
                        foreach(var j in jobs)
                        {
                            if(ej.JobID == j.JobID && ej.UserID == employee.UserID)
                            {
                                joblist.Add(j);
                                break;
                            }
                        }
                    }
                }
                ViewBag.selectedJobID = joblist;

                return View(employee);
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }          
        }

        // POST: Employees/AddJobTimeSheet/2/3/4/1/4/25.00
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTimeSheet(int id, int selectedJobID, int selectedYear, int Month, int Week, decimal Payrate)
        {
            if(_signInManager.IsSignedIn(User))
            {  
                TimeSheet timesheet = new TimeSheet();
                var endWeekDate = await _context.Week.FindAsync(Week);
                timesheet.WeekEndingDate = endWeekDate.EndOfWeekDate;
                timesheet.payrate = Payrate;
                timesheet.Employee = await _context.Employee.FindAsync(id);
                timesheet.Job = await _context.Job.FindAsync(selectedJobID);
                var result = _context.TimeSheets.Add(timesheet);
                await _context.SaveChangesAsync();
                return RedirectToAction("TimeSheetDetails", new {id = result.Entity.TimeSheetID});
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }          
        }

        // GET: Employees/TimeSheetDetails/5
        public async Task<IActionResult> TimeSheetDetails(int? id)
        {
            if(_signInManager.IsSignedIn(User))
            {              
                if (id == null)
                {
                    return NotFound();
                }

                var timeSheet = await _context.TimeSheets.FindAsync(id);
                var employee = await _context.Employee.FindAsync(timeSheet.EmployeeID);
                var job = await _context.Job.FindAsync(timeSheet.JobID);
                var week = _context.Week.First(m => m.EndOfWeekDate == timeSheet.WeekEndingDate);
                ViewBag.Employee = employee;
                ViewBag.Job = job;
                ViewBag.weekdays = week.daysInWeek;
                ViewBag.weekName = week.WeekName;
                ViewBag.isEndofWeek = Convert.ToInt32(week.weekEndingMonth);
                return View(timeSheet);
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }                 
        }

        // POST: Employees/TimeSheetDetails/3
        [HttpPost]
        public async Task<IActionResult> TimeSheetDetails(int? id, int? Sunday, int? Monday, int? Tuesday, int? Wednesday, int? Thursday, int? Friday, int? Saturday)
        {
            if(_signInManager.IsSignedIn(User))
            {               
                TimeSheet timeSheet = await _context.TimeSheets.FindAsync(id);
                if(Sunday != null)
                {
                    TimeSheetDetail timesheetdetail = new TimeSheetDetail();
                    timesheetdetail.WorkDate = timeSheet.WeekEndingDate.AddDays(-6);
                    timesheetdetail.hours = (decimal)Sunday;
                    timesheetdetail.dayOfWeek = (int)timeSheet.WeekEndingDate.AddDays(-6).DayOfWeek;
                    timesheetdetail.TimeSheet = timeSheet;
                    _context.TimeSheetDetails.Add(timesheetdetail);
                }
                if(Monday != null)
                {
                    TimeSheetDetail timesheetdetail = new TimeSheetDetail();
                    timesheetdetail.WorkDate = timeSheet.WeekEndingDate.AddDays(-5);
                    timesheetdetail.hours = (decimal)Monday;
                    timesheetdetail.dayOfWeek = (int)timeSheet.WeekEndingDate.AddDays(-5).DayOfWeek;
                    timesheetdetail.TimeSheet = timeSheet;
                    _context.TimeSheetDetails.Add(timesheetdetail);
                }
                if(Tuesday != null)
                {
                    TimeSheetDetail timesheetdetail = new TimeSheetDetail();
                    timesheetdetail.WorkDate = timeSheet.WeekEndingDate.AddDays(-4);
                    timesheetdetail.hours = (decimal)Tuesday;
                    timesheetdetail.dayOfWeek = (int)timeSheet.WeekEndingDate.AddDays(-4).DayOfWeek;
                    timesheetdetail.TimeSheet = timeSheet;
                    _context.TimeSheetDetails.Add(timesheetdetail);
                }
                if(Wednesday != null)
                {
                    TimeSheetDetail timesheetdetail = new TimeSheetDetail();
                    timesheetdetail.WorkDate = timeSheet.WeekEndingDate.AddDays(-3);
                    timesheetdetail.hours = (decimal)Wednesday;
                    timesheetdetail.dayOfWeek = (int)timeSheet.WeekEndingDate.AddDays(-3).DayOfWeek;
                    timesheetdetail.TimeSheet = timeSheet;
                    _context.TimeSheetDetails.Add(timesheetdetail);
                }
                if(Thursday != null)
                {
                    TimeSheetDetail timesheetdetail = new TimeSheetDetail();
                    timesheetdetail.WorkDate = timeSheet.WeekEndingDate.AddDays(-2);
                    timesheetdetail.hours = (decimal)Thursday;
                    timesheetdetail.dayOfWeek = (int)timeSheet.WeekEndingDate.AddDays(-2).DayOfWeek;
                    timesheetdetail.TimeSheet = timeSheet;
                    _context.TimeSheetDetails.Add(timesheetdetail);
                }
                if(Friday != null)
                {
                    TimeSheetDetail timesheetdetail = new TimeSheetDetail();
                    timesheetdetail.WorkDate = timeSheet.WeekEndingDate.AddDays(-1);
                    timesheetdetail.hours = (decimal)Friday;
                    timesheetdetail.dayOfWeek = (int)timeSheet.WeekEndingDate.AddDays(-1).DayOfWeek;
                    timesheetdetail.TimeSheet = timeSheet;
                    _context.TimeSheetDetails.Add(timesheetdetail);
                }
                if(Saturday != null)
                {
                    TimeSheetDetail timesheetdetail = new TimeSheetDetail();
                    timesheetdetail.WorkDate = timeSheet.WeekEndingDate;
                    timesheetdetail.hours = (decimal)Saturday;
                    timesheetdetail.dayOfWeek = (int)timeSheet.WeekEndingDate.DayOfWeek;
                    timesheetdetail.TimeSheet = timeSheet;
                    _context.TimeSheetDetails.Add(timesheetdetail);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new {id = timeSheet.EmployeeID});
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }             
        }
        
        // GET: Employees/JobEmployeeDetail/5/1
        public async Task<IActionResult> JobEmployeeDetail(int? jobID, int? employeeID)
        {
            if(_signInManager.IsSignedIn(User))
            {   
                if (jobID == null || employeeID == null)
                {
                    return NotFound();
                }

                Job job = await _context.Job.FirstOrDefaultAsync(m => m.JobID == jobID);
                Employee employee = await _context.Employee.FirstOrDefaultAsync(e => e.UserID == employeeID);
                if (job == null || employee == null)
                {
                    return NotFound();
                }
                ViewBag.employee = employee;
                List<TimeSheet> timesheets = _context.TimeSheets.Where(t => t.EmployeeID == employeeID && t.JobID == jobID).ToList();                        
                List<TimeSheetDisplay> timesheetweeks = setTimeSheet(timesheets).OrderBy(t => t.Year).ToList();
                ViewBag.timesheetweeks = timesheetweeks;
                ViewBag.totalWeeks = timesheetweeks.Sum(ts => ts.TotalPayout);
                return View(job);
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }                     
        }

        private List<TimeSheetDisplay> setTimeSheet(List<TimeSheet> timesheets) 
        {             
            List<TimeSheetDisplay> tsheetDisplays = new List<TimeSheetDisplay>();
            List<Week> weeks = new List<Week>();
            foreach(var timesheet in timesheets)
            {
                weeks.Add(_context.Week.First(w => w.EndOfWeekDate == timesheet.WeekEndingDate));
                timesheet.TimeSheetDetails = addingTimeSheetDetail(timesheet);
            }
            foreach(var week in weeks)
            {
                TimeSheetDisplay tsheetdisplay = new TimeSheetDisplay();
                tsheetdisplay.WeekName = week.WeekName;
                tsheetdisplay.Year = week.EndOfWeekDate.Year;                            
                foreach(var timesheet in timesheets)
                {
                   
                    if(timesheet.WeekEndingDate == week.EndOfWeekDate)
                    {
                        tsheetdisplay.PayRate = timesheet.payrate;
                        foreach(var timesheetdetail in timesheet.TimeSheetDetails)
                        {
                            tsheetdisplay.TimeSheetID = timesheet.TimeSheetID; 
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
                    }
                }
                tsheetdisplay.TotalPayout = Math.Round(tsheetdisplay.TotalHours * tsheetdisplay.PayRate);
                tsheetDisplays.Add(tsheetdisplay);                                      
            }
            return tsheetDisplays;
        }

        private bool HasAJob(List<Employee> employees)
        {
            foreach (var employee in employees)
            {
                if (employee.EmployeeJobs.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public JsonResult GetMonths(string id)
        {
            int iD = Int32.Parse(id);
            List<Month> months = new List<Month>();
            months = _context.Month.Where(m => m.Year.YearID == iD).ToList();
            //removeMonths =
            return Json(new SelectList(months, "MonthID", "MonthName"));
        }

        public JsonResult GetWeeks(string month, int employeeID)
        {
            int monthid = Int32.Parse(month);
            int employeeId = employeeID;
            List<Week> weeks = _context.Week.Where(w => w.Month.MonthID == monthid).ToList();
            List<TimeSheet> timesheets = _context.TimeSheets.Where(t => t.EmployeeID == employeeId).ToList();
            if(timesheets.Count != 0) {
                foreach(var timesheet in timesheets) {                
                    for(int i = weeks.Count -1; i>= 0; i--) {
                        if(weeks[i].EndOfWeekDate == timesheet.WeekEndingDate)
                        {
                            weeks.RemoveAt(i);
                        }
                    }
                }     
            } 
            return Json(new SelectList(weeks, "WeekID", "WeekName"));
        }

        private Employee loadingEmployeeJobs(Employee employee)
        {
            List<EmployeeJob> employeejobs = _context.EmployeeJobs.ToList();
            for (int i = employeejobs.Count - 1; i >= 0; i--)
            {
                if (employeejobs[i].UserID != employee.UserID)
                {
                    employeejobs.RemoveAt(i);
                }
            }
            employee.EmployeeJobs = employeejobs;
            return employee;
        }
        
        private Employee loadingTimeSheets(Employee employee)
        {
            employee.TimeSheets = _context.TimeSheets.Where(t => t.EmployeeID == employee.UserID).ToList();
            foreach(var tsd in employee.TimeSheets)
            {
                tsd.TimeSheetDetails = _context.TimeSheetDetails.Where(ts => ts.TimeSheetID == tsd.TimeSheetID).ToList();
            }
            return employee;
        }

        private ICollection<TimeSheetDetail> addingTimeSheetDetail(TimeSheet timesheet)
        {
            var timesheetdetails = _context.TimeSheetDetails.Where(t => t.TimeSheetID == timesheet.TimeSheetID).ToList();
            return timesheetdetails;
        }
    }
}
