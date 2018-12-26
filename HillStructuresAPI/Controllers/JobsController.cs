using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Cors;
using HillStructuresAPI.Models;

namespace HillStructuresAPI.Controllers
{
    [Route("api/Jobs")]
    public class JobsController : ControllerBase
    {
        private readonly HillStructuresContext _context;      

        public JobsController(HillStructuresContext context)
        {
            _context = context;
        }

        // GET api/Jobs/get
        [EnableCors("AllowAll")]
        [Produces("application/json")]
        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var jobs = _context.Job.ToList();
                return Ok(jobs);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        // GET api/Jobs/get/2
        [EnableCors("AllowAll")]
        [Produces("application/json")]
        [HttpGet("Get/{jobID}")]
        public async Task<IActionResult> Get(int jobID)
        {
            try
            {
                var job = _context.Job.SingleOrDefault(p => p.JobID == jobID);
                return Ok(job);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        // POST api/Jobs/create
        [EnableCors("AllowAll")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] Job job)
        {
            try
            {
                _context.Job.Add(job);
                _context.SaveChanges();
                return Ok(job);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        // PUT api/Jobs/update
        [EnableCors("AllowAll")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] Job job)
        {
            try
            {
                _context.Entry(job).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
                return Ok(job);
            }
            catch (Exception e)
            {
                return BadRequest(e);

            }
        }

        // DELETE api/Jobs/delete/4
        [EnableCors("AllowAll")]
        [HttpDelete("Delete/{JobID}")]
        public async Task<IActionResult> Delete(int jobID)
        {
            try
            {
                _context.Remove(_context.Job.Find(jobID));
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        /*// GET: Jobs
        public async Task<IActionResult> Index()
        {
            if(_signInManager.IsSignedIn(User))
            {             
                return View(await _context.Job.ToListAsync());
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }                  
        }

        // GET: Jobs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(_signInManager.IsSignedIn(User))
            {            
                if (id == null)
                {
                    return NotFound();
                }

                var job = await _context.Job
                    .FirstOrDefaultAsync(m => m.JobID == id);
                if (job == null)
                {
                    return NotFound();
                }
                decimal employeesTotal = 0;
                decimal subcontractorsTotal = 0;
                decimal suppliersTotal = 0;
                decimal jobtotal = 0; 
                //Creating list of employees of job
                IQueryable<EmployeeJob> employeeJobs = _context.EmployeeJobs.Where(m => m.JobID == id);
                IList<Employee> employees = await _context.Employee.ToListAsync();
                List<TimeSheetDisplay> tsdisplaylist = new List<TimeSheetDisplay>();
                foreach(var item in employees)
                {
                    foreach(var item2 in employeeJobs)
                    {
                        if(item.UserID == item2.UserID)
                        {
                            item.TimeSheets = addTimeSheets(item, job);
                            TimeSheetDisplay tsdisplay = new TimeSheetDisplay();
                            tsdisplay.Employee = item;
                            tsdisplay.TotalPayout = retrieveEmployeeTotal(item);
                            employeesTotal += tsdisplay.TotalPayout;
                            tsdisplaylist.Add(tsdisplay);
                        }
                    }
                }
                jobtotal += employeesTotal;
                ViewBag.totalForEmployees = employeesTotal;
                ViewBag.currentEmployees = tsdisplaylist;

                //Creating list of subcontractors of job
                IQueryable<SubContractorJob> subcontractorJobs = _context.SubContractorJobs.Where(m => m.JobID == id);
                IList<SubContractor> subcontractors = await _context.SubContractor.ToListAsync();
                List<PaymentSheetDisplay> psDisplaySubcontractorList = new List<PaymentSheetDisplay>();
                foreach(var item in subcontractors)
                {
                    foreach(var item2 in subcontractorJobs)
                    {
                        if(item.CompanyID == item2.CompanyID)
                        {
                            item.PaymentSheets = addPaymentSheets(item, job);
                            PaymentSheetDisplay psDisplaySubcontractor = new PaymentSheetDisplay();
                            psDisplaySubcontractor.Company = item;
                            psDisplaySubcontractor.TotalPayment = retrieveCompanyTotal(item);
                            subcontractorsTotal += psDisplaySubcontractor.TotalPayment;
                            psDisplaySubcontractorList.Add(psDisplaySubcontractor);
                        }
                    }
                }
                jobtotal += subcontractorsTotal;
                ViewBag.totalForSubContractors = subcontractorsTotal;
                ViewBag.currentSubContractors = psDisplaySubcontractorList;

                //Creating list of suppliers of job
                IQueryable<SupplierJob> supplierJobs = _context.SupplierJobs.Where(m => m.JobID == id);
                List<Supplier> suppliers = await _context.Supplier.ToListAsync();
                List<PaymentSheetDisplay> psDisplaySupplierList = new List<PaymentSheetDisplay>();
                foreach(var item in suppliers)
                {
                    foreach(var item2 in supplierJobs)
                    {
                        if(item.CompanyID == item2.CompanyID)
                        {
                            item.PaymentSheets = addPaymentSheets(item, job);
                            PaymentSheetDisplay psDisplaySupplier = new PaymentSheetDisplay();
                            psDisplaySupplier.Company = item;
                            psDisplaySupplier.TotalPayment = retrieveCompanyTotal(item);
                            suppliersTotal += psDisplaySupplier.TotalPayment;
                            psDisplaySupplierList.Add(psDisplaySupplier);
                        }
                    }
                }
                jobtotal += suppliersTotal;
                ViewBag.totalForSuppliers = suppliersTotal;
                ViewBag.OverallTotal = jobtotal;
                ViewBag.currentSuppliers = psDisplaySupplierList;
                return View(job);
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }               
        }

        // GET: Jobs/Create
        public IActionResult Create()
        {
            if(_signInManager.IsSignedIn(User))
            {                
                List<Client> clientList = new List<Client>();           
                clientList = _context.Client.ToList();
                ViewBag.selectedUserId = clientList;
                
                return View();
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }               
        }

        // POST: Jobs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("JobID,JobName,Address,StartDate,EndDate")] Job job, int selectedClientId)
        {
            if(_signInManager.IsSignedIn(User))
            {            
                var client = await _context.Client.FindAsync(selectedClientId);
                if (ModelState.IsValid)
                {
                    _context.Add(job);
                    job.Client = client;
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(job);
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }                   
        }

        // GET: Jobs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if(_signInManager.IsSignedIn(User))
            {                
                if (id == null)
                {
                    return NotFound();
                }

                var job = await _context.Job.FindAsync(id);
                if (job == null)
                {
                    return NotFound();
                }
                return View(job);
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }               
        }

        // POST: Jobs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("JobID,JobName,Address,StartDate,EndDate")] Job job)
        {
            if(_signInManager.IsSignedIn(User))
            {               
                if (id != job.JobID)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(job);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!JobExists(job.JobID))
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
                return View(job);
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }                   
        }

        // GET: Jobs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(_signInManager.IsSignedIn(User))
            {             
                if (id == null)
                {
                    return NotFound();
                }

                var job = await _context.Job
                    .FirstOrDefaultAsync(m => m.JobID == id);
                if (job == null)
                {
                    return NotFound();
                }

                return View(job);
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }   
        }

        // POST: Jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if(_signInManager.IsSignedIn(User))
            {              
                var job = await _context.Job.FindAsync(id);
                _context.Job.Remove(job);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }              
        }

        private bool JobExists(int id)
        {
            return _context.Job.Any(e => e.JobID == id);
        }

        // GET: Jobs/AddEmployees/5
        public async Task<IActionResult> AddEmployees(int? id)
        {
            if(_signInManager.IsSignedIn(User))
            {              
                if (id == null)
                {
                    return NotFound();
                }

                var job = await _context.Job.FindAsync(id);
                List<Employee> employeelist = new List<Employee>();           
                employeelist = _context.Employee.ToList();
                List<EmployeeJob> employeejoblist = _context.EmployeeJobs.ToList();
                if(!(employeejoblist == null) && employeejoblist.Count > 0 && employeelist.Count > 0)
                {
                    foreach(var ej in employeejoblist)
                    {
                        foreach(var e in employeelist)
                        {
                            if(ej.UserID == e.UserID && ej.JobID == job.JobID)
                            {
                                employeelist.Remove(e);
                                break;
                            }
                        }
                    }
                }
                ViewBag.selectedUserID = employeelist;

                if (job == null)
                {
                    return NotFound();
                }
                return View(job);
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }                  
        }

        // POST: Jobs/AddEmployees/2/3
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddEmployees(int id, int selectedUserID)
        {
            if(_signInManager.IsSignedIn(User))
            {                
                var job = await _context.Job.FindAsync(id);
                EmployeeJob employeejob = new EmployeeJob();
                employeejob.UserID = selectedUserID;
                employeejob.JobID = id;
                _context.EmployeeJobs.Add(employeejob);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }               
        }

        // GET: Jobs/AddSubContractors/5
        public async Task<IActionResult> AddSubContractors(int? id)
        {
            if(_signInManager.IsSignedIn(User))
            {             
                if (id == null)
                {
                    return NotFound();
                }

                var job = await _context.Job.FindAsync(id);
                List<SubContractor> subContractorlist = _context.SubContractor.ToList();           
                List<SubContractorJob> subcontractorjoblist = _context.SubContractorJobs.ToList();
                if(!(subcontractorjoblist == null) && subcontractorjoblist.Count > 0 && subContractorlist.Count > 0)
                {
                    foreach(var ej in subcontractorjoblist)
                    {
                        foreach(var e in subContractorlist)
                        {
                            if(ej.CompanyID == e.CompanyID && ej.JobID == job.JobID)
                            {
                                subContractorlist.Remove(e);
                                break;
                            }
                        }
                    }
                }
                ViewBag.selectedCompanyID = subContractorlist;

                if (job == null)
                {
                    return NotFound();
                }
                return View(job);
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }                 
        }

        // POST: Jobs/AddSubContractors/2/3
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddSubContractors(int id, int selectedCompanyID)
        {
            if(_signInManager.IsSignedIn(User))
            {                   
                var job = await _context.Job.FindAsync(id);
                SubContractorJob subcontractorjob = new SubContractorJob();
                subcontractorjob.CompanyID = selectedCompanyID;
                subcontractorjob.JobID = id;
                _context.SubContractorJobs.Add(subcontractorjob);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }               
        }

        // GET: Jobs/AddSuppliers/5
        public async Task<IActionResult> AddSuppliers(int? id)
        {
            if(_signInManager.IsSignedIn(User))
            {               
                if (id == null)
                {
                    return NotFound();
                }

                var job = await _context.Job.FindAsync(id);
                List<Supplier> supplierlist = _context.Supplier.ToList();           
                List<SupplierJob> supplierjoblist = _context.SupplierJobs.ToList();
                if(!(supplierjoblist == null) && supplierjoblist.Count > 0 && supplierlist.Count > 0)
                {
                    foreach(var ej in supplierjoblist)
                    {
                        foreach(var e in supplierlist)
                        {
                            if(ej.CompanyID == e.CompanyID && ej.JobID == job.JobID)
                            {
                                supplierlist.Remove(e);
                                break;
                            }
                        }
                    }
                }
                ViewBag.selectedCompanyID = supplierlist;

                if (job == null)
                {
                    return NotFound();
                }
                return View(job);
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }                 
        }

        // POST: Jobs/AddSupplier/2/3
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddSuppliers(int id, int selectedCompanyID)
        {
            if(_signInManager.IsSignedIn(User))
            {             
                var job = await _context.Job.FindAsync(id);
                SupplierJob supplierjob = new SupplierJob();
                supplierjob.CompanyID = selectedCompanyID;
                supplierjob.JobID = id;
                _context.SupplierJobs.Add(supplierjob);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }                 
        }

        // GET: Jobs/AddTimeSheet/5
        public async Task<IActionResult> AddTimeSheet(int? id)
        {
            if(_signInManager.IsSignedIn(User))
            {             
                if (id == null)
                {
                    return NotFound();
                }

                var job = await _context.Job.FindAsync(id);
                List<Employee> employeelist = _context.Employee.ToList();
                List<Employee> employees = new List<Employee>();
                List<EmployeeJob> employeejoblist = _context.EmployeeJobs.Where(ej => ej.JobID == job.JobID).ToList();
                if(!(employeejoblist == null) && employeejoblist.Count > 0 && employeelist.Count > 0)
                {
                    foreach (var e in employeelist)
                    {
                        foreach(var ej in employeejoblist)
                        {
                            if(ej.UserID == e.UserID)
                            {
                                employees.Add(e);
                                break;
                            }                       
                        }
                    }
                }
                ViewBag.jobid = job.JobID; 
                ViewBag.selectedEmployeeID = employees;
                ViewBag.selectedYear = _context.Year.ToList();
                if (job == null)
                {
                    return NotFound();
                }
                return View(job);
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }                  
        }

        // POST: Employees/AddJobTimeSheet/2/3/4/1/4/25.00
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTimeSheet(int id, int selectedEmployeeID, int selectedYear, int Month, int Week, decimal Payrate)
        {
            if(_signInManager.IsSignedIn(User))
            {               
                TimeSheet timesheet = new TimeSheet();
                var endWeekDate = await _context.Week.FindAsync(Week);
                timesheet.WeekEndingDate = endWeekDate.EndOfWeekDate;
                timesheet.payrate = Payrate;
                timesheet.Job = await _context.Job.FindAsync(id);
                timesheet.Employee = await _context.Employee.FindAsync(selectedEmployeeID);
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
                return RedirectToAction("Details", new {id = timeSheet.JobID});
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }                 
        }


        // GET: Jobs/AddPaymentSheet/5
        public async Task<IActionResult> AddPaymentSheet(int? id)
        {
            if(_signInManager.IsSignedIn(User))
            {             
                if (id == null)
                {
                    return NotFound();
                }

                var job = await _context.Job.FindAsync(id);

                if (job == null)
                {
                    return NotFound();
                }
                ViewBag.jobid = job.JobID;
                //need to add a filter pertaining to when job is open *******************************
                ViewBag.selectedYear = _context.Year.ToList();
                return View(job);
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }                  
        }

        // POST: Jobs/AddPaymentSheet/2/3/4/1/4/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPaymentSheet(int id, int selectCompanyType, int selectedCompanyID, int selectedYear, int Month, int Week)
        {
            if(_signInManager.IsSignedIn(User))
            {             
                PaymentSheet paymentsheet = new PaymentSheet();
                var endWeekDate = await _context.Week.FindAsync(Week);
                paymentsheet.WeekEndingDate = endWeekDate.EndOfWeekDate;
                if (selectCompanyType == 1)
                {
                    paymentsheet.Company = await _context.SubContractor.FindAsync(selectedCompanyID);
                }
                if (selectCompanyType == 2)
                {
                    paymentsheet.Company = await _context.Supplier.FindAsync(selectedCompanyID);
                }
                paymentsheet.Job = await _context.Job.FindAsync(id);
                var result = _context.PaymentSheets.Add(paymentsheet);
                await _context.SaveChangesAsync();
                return RedirectToAction("PaymentSheetDetails", new { id = result.Entity.PaymentSheetID });
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }                                 
        }

        // GET: Jobs/PaymentSheetDetails/5
        public async Task<IActionResult> PaymentSheetDetails(int? id)
        {
            if(_signInManager.IsSignedIn(User))
            {            
                if (id == null)
                {
                    return NotFound();
                }

                var paymentSheet = await _context.PaymentSheets.FindAsync(id);
                var subContractor = await _context.SubContractor.FindAsync(paymentSheet.CompanyID);
                if (subContractor == null)
                {
                    var supplier = await _context.Supplier.FindAsync(paymentSheet.CompanyID);
                    ViewBag.Company = supplier;
                } else
                {
                    ViewBag.Company = subContractor;
                }
                var job = await _context.Job.FindAsync(paymentSheet.JobID);
                var week = _context.Week.First(m => m.EndOfWeekDate == paymentSheet.WeekEndingDate);
                ViewBag.Job = job;
                ViewBag.weekdays = week.daysInWeek;
                ViewBag.weekName = week.WeekName;
                ViewBag.isEndofWeek = Convert.ToInt32(week.weekEndingMonth);
                return View(paymentSheet);
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }               
        }

        // POST: Jobs/PaymentSheetDetails/3/30/30/30/40/42/33/33
        [HttpPost]
        public async Task<IActionResult> PaymentSheetDetails(int? id, int? Sunday, int? Monday, int? Tuesday, int? Wednesday, int? Thursday, int? Friday, int? Saturday)
        {
            if(_signInManager.IsSignedIn(User))
            {
                PaymentSheet paymentSheet = await _context.PaymentSheets.FindAsync(id);
                if (Sunday != null)
                {
                    PaymentSheetDetail paymentsheetdetail = new PaymentSheetDetail();
                    paymentsheetdetail.WorkDate = paymentSheet.WeekEndingDate.AddDays(-6);
                    paymentsheetdetail.Payout = (decimal)Sunday;
                    paymentsheetdetail.dayOfWeek = (int)paymentSheet.WeekEndingDate.AddDays(-6).DayOfWeek;
                    paymentsheetdetail.PaymentSheet = paymentSheet;
                    _context.PaymentSheetDetails.Add(paymentsheetdetail);
                }
                if (Monday != null)
                {
                    PaymentSheetDetail paymentsheetdetail = new PaymentSheetDetail();
                    paymentsheetdetail.WorkDate = paymentSheet.WeekEndingDate.AddDays(-5);
                    paymentsheetdetail.Payout = (decimal)Monday;
                    paymentsheetdetail.dayOfWeek = (int)paymentSheet.WeekEndingDate.AddDays(-5).DayOfWeek;
                    paymentsheetdetail.PaymentSheet = paymentSheet;
                    _context.PaymentSheetDetails.Add(paymentsheetdetail);
                }
                if (Tuesday != null)
                {
                    PaymentSheetDetail paymentsheetdetail = new PaymentSheetDetail();
                    paymentsheetdetail.WorkDate = paymentSheet.WeekEndingDate.AddDays(-4);
                    paymentsheetdetail.Payout = (decimal)Tuesday;
                    paymentsheetdetail.dayOfWeek = (int)paymentSheet.WeekEndingDate.AddDays(-4).DayOfWeek;
                    paymentsheetdetail.PaymentSheet = paymentSheet;
                    _context.PaymentSheetDetails.Add(paymentsheetdetail);
                }
                if (Wednesday != null)
                {
                    PaymentSheetDetail paymentsheetdetail = new PaymentSheetDetail();
                    paymentsheetdetail.WorkDate = paymentSheet.WeekEndingDate.AddDays(-3);
                    paymentsheetdetail.Payout = (decimal)Wednesday;
                    paymentsheetdetail.dayOfWeek = (int)paymentSheet.WeekEndingDate.AddDays(-3).DayOfWeek;
                    paymentsheetdetail.PaymentSheet = paymentSheet;
                    _context.PaymentSheetDetails.Add(paymentsheetdetail);
                }
                if (Thursday != null)
                {
                    PaymentSheetDetail paymentsheetdetail = new PaymentSheetDetail();
                    paymentsheetdetail.WorkDate = paymentSheet.WeekEndingDate.AddDays(-2);
                    paymentsheetdetail.Payout = (decimal)Thursday;
                    paymentsheetdetail.dayOfWeek = (int)paymentSheet.WeekEndingDate.AddDays(-2).DayOfWeek;
                    paymentsheetdetail.PaymentSheet = paymentSheet;
                    _context.PaymentSheetDetails.Add(paymentsheetdetail);
                }
                if (Friday != null)
                {
                    PaymentSheetDetail paymentsheetdetail = new PaymentSheetDetail();
                    paymentsheetdetail.WorkDate = paymentSheet.WeekEndingDate.AddDays(-1);
                    paymentsheetdetail.Payout = (decimal)Friday;
                    paymentsheetdetail.dayOfWeek = (int)paymentSheet.WeekEndingDate.AddDays(-1).DayOfWeek;
                    paymentsheetdetail.PaymentSheet = paymentSheet;
                    _context.PaymentSheetDetails.Add(paymentsheetdetail);
                }
                if (Saturday != null)
                {
                    PaymentSheetDetail paymentsheetdetail = new PaymentSheetDetail();
                    paymentsheetdetail.WorkDate = paymentSheet.WeekEndingDate;
                    paymentsheetdetail.Payout = (decimal)Saturday;
                    paymentsheetdetail.dayOfWeek = (int)paymentSheet.WeekEndingDate.DayOfWeek;
                    paymentsheetdetail.PaymentSheet = paymentSheet;
                    _context.PaymentSheetDetails.Add(paymentsheetdetail);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = paymentSheet.JobID });
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }                  
        }

        public JsonResult GetCompany(string id, int jobID)
        {
            if (id == "1")
            {
                int iD = Int32.Parse(id);
                List<SubContractor> subContractors = new List<SubContractor>();
                List<SubContractor> subContractorsList = _context.SubContractor.ToList();
                List<SubContractorJob> subContractorJobs = _context.SubContractorJobs.Where(sj => sj.JobID == jobID).ToList();
                foreach (var s in subContractorsList)
                {
                    foreach (var sj in subContractorJobs)
                    {
                        if (s.CompanyID == sj.CompanyID)
                        {
                            subContractors.Add(s);
                        }
                    }
                }
                return Json(new SelectList(subContractors, "CompanyID", "CompanyName"));
            } else
            {
                int iD = Int32.Parse(id);
                List<Supplier> suppliers = new List<Supplier>();
                List<Supplier> suppliersList = _context.Supplier.ToList();
                List<SupplierJob> supplierJobs = _context.SupplierJobs.Where(sj => sj.JobID == jobID).ToList();
                foreach (var s in suppliersList)
                {
                    foreach (var sj in supplierJobs)
                    {
                        if (s.CompanyID == sj.CompanyID)
                        {
                            suppliers.Add(s);
                        }
                    }
                }
                return Json(new SelectList(suppliers, "CompanyID", "CompanyName"));
            }
        }

        public JsonResult GetMonths(string id)
        {
            int iD = Int32.Parse(id);
            List<Month> months = new List<Month>();
            months = _context.Month.Where(m => m.Year.YearID == iD).ToList();
            return Json(new SelectList(months, "MonthID", "MonthName"));
        }

        public JsonResult GetWeeks(string month, int jobID)
        {
            int monthid = Int32.Parse(month);
            int jobId = jobID;
            List<Week> weeks = _context.Week.Where(w => w.Month.MonthID == monthid).ToList();
            List<TimeSheet> timesheets = _context.TimeSheets.Where(t => t.JobID == jobId).ToList();
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

        private List<TimeSheet> addTimeSheets(Employee employee, Job job)
        {
            List<TimeSheet> timesheets = _context.TimeSheets.Where(t => t.EmployeeID == employee.UserID && t.JobID == job.JobID).ToList();
            foreach(var timesheet in timesheets)
            {
                timesheet.TimeSheetDetails = _context.TimeSheetDetails.Where(ts => ts.TimeSheetID == timesheet.TimeSheetID).ToList();
            }
            return timesheets;
        }

        private decimal retrieveEmployeeTotal(Employee employee)
        {
            decimal total = 0;
            foreach(var ts in employee.TimeSheets)
            {
                foreach(var tsd in ts.TimeSheetDetails)
                {
                    total += ts.payrate * (decimal)tsd.hours;
                }
            }
            return Math.Round(total);
        }

        private List<PaymentSheet> addPaymentSheets(Company company, Job job)
        {
            List<PaymentSheet> paysheets = _context.PaymentSheets.Where(p => p.CompanyID == company.CompanyID && p.JobID == job.JobID).ToList();
            foreach(var paysheet in paysheets)
            {
                paysheet.PaymentSheetDetails = _context.PaymentSheetDetails.Where(ps => ps.PaymentSheetID ==  paysheet.PaymentSheetID).ToList();
            }
            return paysheets;
        }

        private decimal retrieveCompanyTotal(Company company)
        {
            decimal total = 0;
            foreach (var ps in company.PaymentSheets)
            {
                foreach(var psd in ps.PaymentSheetDetails)
                {
                    total += psd.Payout;
                }
            }
            return Math.Round(total);
        }*/
    }
}
