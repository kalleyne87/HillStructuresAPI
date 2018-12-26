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
using Microsoft.AspNetCore.Cors;
using HillStructuresAPI.Models;


namespace HillStructuresAPI.Controllers
{
    [Route("api/SubContractor")]
    public class SubContractorsController : ControllerBase
    {
        private readonly HillStructuresContext _context;

        public SubContractorsController(HillStructuresContext context)
        {
            _context = context;            
        }

        // GET api/Subcontractors/get
        [EnableCors("AllowAll")]
        [Produces("application/json")]
        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var subcontractor = _context.SubContractor.ToList();
                return Ok(subcontractor);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        // GET api/Subcontractors/get/2
        [EnableCors("AllowAll")]
        [Produces("application/json")]
        [HttpGet("Get/{CompanyID}")]
        public async Task<IActionResult> Get(int CompanyID)
        {
            try
            {
                var subcontractor = _context.SubContractor.SingleOrDefault(p => p.CompanyID == CompanyID);
                return Ok(subcontractor);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        // POST api/Subcontractors/create
        [EnableCors("AllowAll")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] SubContractor subcontractor)
        {
            try
            {
                _context.SubContractor.Add(subcontractor);
                _context.SaveChanges();
                return Ok(subcontractor);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        // PUT api/Subcontractors/update
        [EnableCors("AllowAll")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] SubContractor subcontractor)
        {
            try
            {
                _context.Entry(subcontractor).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
                return Ok(subcontractor);
            }
            catch (Exception e)
            {
                return BadRequest(e);

            }
        }

        // DELETE api/Subcontractors/delete/4
        [EnableCors("AllowAll")]
        [HttpDelete("Delete/{CompanyID}")]
        public async Task<IActionResult> Delete(int CompanyID)
        {
            try
            {
                _context.Remove(_context.SubContractor.Find(CompanyID));
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        /*// GET: SubContractors
        public async Task<IActionResult> Index()
        {
            if(_signInManager.IsSignedIn(User))
            {            
                var subcontractors = await _context.SubContractor.ToListAsync();
                
                foreach (var subContractor in subcontractors)
                {
                    loadingSubContractorJobs(subContractor);
                }
                return View(subcontractors);
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }             
        }

        // GET: SubContractors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(_signInManager.IsSignedIn(User))
            {              
                if (id == null)
                {
                    return NotFound();
                }

                var subContractor = await _context.SubContractor
                    .FirstOrDefaultAsync(m => m.CompanyID == id);
                

                if (subContractor == null)
                {
                    return NotFound();
                }
                List<SubContractorJob> subcontractorJobs = _context.SubContractorJobs.Where(m => m.JobID == id).ToList();
                List<Job> jobs = await _context.Job.ToListAsync();
                List<Job> listCurrentJobs = new List<Job>();
                foreach(var item in jobs)
                {
                    foreach(var item2 in subcontractorJobs)
                    {
                        if(item.JobID == item2.JobID)
                        {
                            listCurrentJobs.Add(item);
                        }
                    }
                }
                ViewBag.currentJobs = listCurrentJobs; 
                return View(subContractor);
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }              
        }

        // GET: SubContractors/Create
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

        // POST: Subcontractors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CompanyID,CompanyName,Address,PhoneNumber")] SubContractor subContractor)
        {
            if(_signInManager.IsSignedIn(User))
            {              
                if (ModelState.IsValid)
                {
                    _context.Add(subContractor);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(subContractor);
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }               
        }

        // GET: SubContractors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if(_signInManager.IsSignedIn(User))
            {              
                if (id == null)
                {
                    return NotFound();
                }

                var subContractor = await _context.SubContractor.FindAsync(id);
                if (subContractor == null)
                {
                    return NotFound();
                }
                return View(subContractor);
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }                   
        }

        // POST: SubContractors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CompanyID,CompanyName,Address,PhoneNumber")] SubContractor subContractor)
        {
            if(_signInManager.IsSignedIn(User))
            {              
                if (id != subContractor.CompanyID)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(subContractor);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!SubContractorExists(subContractor.CompanyID))
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
                return View(subContractor);
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }                       
        }

        // GET: SubContractors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(_signInManager.IsSignedIn(User))
            {              
                if (id == null)
                {
                    return NotFound();
                }

                var subContractor = await _context.SubContractor
                    .FirstOrDefaultAsync(m => m.CompanyID == id);
                if (subContractor == null)
                {
                    return NotFound();
                }

                return View(subContractor);
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }          
        }

        // POST: SubContractors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if(_signInManager.IsSignedIn(User))
            {               
                var subContractor = await _context.SubContractor.FindAsync(id);
                loadingSubContractorJobs(subContractor);
                loadingPaymentSheets(subContractor);
                foreach(var ps in subContractor.PaymentSheets)
                {
                    _context.PaymentSheets.Remove(ps);
                }
                foreach(var sj in subContractor.SubContractorJobs)
                {
                    _context.SubContractorJobs.Remove(sj);
                }
                _context.SubContractor.Remove(subContractor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }                   
        }

        private bool SubContractorExists(int id)
        {
            return _context.SubContractor.Any(e => e.CompanyID == id);
        }

        // GET: SubContractors/AddJobs/5
        public async Task<IActionResult> AddJobs(int? id)
        {
            if(_signInManager.IsSignedIn(User))
            {               
                if (id == null)
                {
                    return NotFound();
                }

                var subContractor = await _context.SubContractor.FindAsync(id);
                List<Job> joblist = new List<Job>();           
                joblist = _context.Job.ToList();
                List<SubContractorJob> subcontractorjoblist = _context.SubContractorJobs.ToList();
                if(!(subcontractorjoblist == null) && subcontractorjoblist.Count > 0 && joblist.Count > 0)
                {
                    foreach(var ej in subcontractorjoblist)
                    {
                        foreach(var j in joblist)
                        {
                            if(ej.JobID == j.JobID && ej.CompanyID == subContractor.CompanyID)
                            {
                                joblist.Remove(j);
                                break;
                            }
                        }
                    }
                }
                ViewBag.selectedJobID = joblist;

                if (subContractor == null)
                {
                    return NotFound();
                }
                return View(subContractor);
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }                              
        }

        // POST: SubContractors/AddJobs/2/3
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddJobs(int id, int selectedJobID)
        {
            if(_signInManager.IsSignedIn(User))
            {              
                SubContractorJob subContractorjob = new SubContractorJob();
                subContractorjob.CompanyID = id;
                subContractorjob.JobID = selectedJobID;
                _context.SubContractorJobs.Add(subContractorjob);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }                      
        }

        // GET: SubContractors/AddPaymentSheet/5
        public async Task<IActionResult> AddPaymentSheet(int? id)
        {
            if(_signInManager.IsSignedIn(User))
            {              
                if (id == null)
                {
                    return NotFound();
                }

                var subcontractor = await _context.SubContractor.FindAsync(id);
                
                if (subcontractor == null)
                {
                    return NotFound();
                }            
                ViewBag.companyid = subcontractor.CompanyID;
                //need to add a filter pertaining to when job is open *******************************
                ViewBag.selectedYear = _context.Year.ToList();     
                List<Job> jobs =  _context.Job.ToList();
                List<Job> joblist = new List<Job>();
                List<SubContractorJob> subcontractorjoblist = _context.SubContractorJobs.ToList();
                if(!(subcontractorjoblist == null))
                {
                    foreach(var sj in subcontractorjoblist)
                    {
                        foreach(var j in jobs)
                        {
                            if(sj.JobID == j.JobID && sj.CompanyID == subcontractor.CompanyID)
                            {
                                joblist.Add(j);
                                break;
                            }
                        }
                    }
                }
                ViewBag.selectedJobID = joblist;
                return View(subcontractor);
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }                  
        }

        // POST: SubContractors/AddPaymentSheet/2/3/4/1/4/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPaymentSheet(int id, int selectedJobID, int selectedYear, int Month, int Week)
        {
            if(_signInManager.IsSignedIn(User))
            {             
                PaymentSheet paymentsheet = new PaymentSheet();
                var endWeekDate = await _context.Week.FindAsync(Week);
                paymentsheet.WeekEndingDate = endWeekDate.EndOfWeekDate;
                paymentsheet.Company = await _context.SubContractor.FindAsync(id);
                paymentsheet.Job = await _context.Job.FindAsync(selectedJobID);
                var result = _context.PaymentSheets.Add(paymentsheet);
                await _context.SaveChangesAsync();
                return RedirectToAction("PaymentSheetDetails", new {id = result.Entity.PaymentSheetID});
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }                
        }

        // GET: SubContractors/PaymentSheetDetails/5
        public async Task<IActionResult> PaymentSheetDetails(int? id)
        {
            if(_signInManager.IsSignedIn(User))
            {                         
                if (id == null)
                {
                    return NotFound();
                }

                var paymentSheet = await _context.PaymentSheets.FindAsync(id);
                var subcontractor = await _context.SubContractor.FindAsync(paymentSheet.CompanyID);
                var job = await _context.Job.FindAsync(paymentSheet.JobID);
                var week = _context.Week.First(m => m.EndOfWeekDate == paymentSheet.WeekEndingDate);
                ViewBag.SubContractor = subcontractor;
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

        // POST: SubContractors/PaymentSheetDetails/3/30/30/30/40/42/33/33
        [HttpPost]
        public async Task<IActionResult> PaymentSheetDetails(int? id, int? Sunday, int? Monday, int? Tuesday, int? Wednesday, int? Thursday, int? Friday, int? Saturday)
        {
            if(_signInManager.IsSignedIn(User))
            {              
                PaymentSheet paymentSheet = await _context.PaymentSheets.FindAsync(id);
                if(Sunday != null)
                {
                    PaymentSheetDetail paymentsheetdetail = new PaymentSheetDetail();
                    paymentsheetdetail.WorkDate = paymentSheet.WeekEndingDate.AddDays(-6);
                    paymentsheetdetail.Payout = (decimal)Sunday;
                    paymentsheetdetail.dayOfWeek = (int)paymentSheet.WeekEndingDate.AddDays(-6).DayOfWeek;
                    paymentsheetdetail.PaymentSheet = paymentSheet;
                    _context.PaymentSheetDetails.Add(paymentsheetdetail);
                }
                if(Monday != null)
                {
                    PaymentSheetDetail paymentsheetdetail = new PaymentSheetDetail();
                    paymentsheetdetail.WorkDate = paymentSheet.WeekEndingDate.AddDays(-5);
                    paymentsheetdetail.Payout = (decimal)Monday;
                    paymentsheetdetail.dayOfWeek = (int)paymentSheet.WeekEndingDate.AddDays(-5).DayOfWeek;
                    paymentsheetdetail.PaymentSheet = paymentSheet;
                    _context.PaymentSheetDetails.Add(paymentsheetdetail);
                }
                if(Tuesday != null)
                {
                    PaymentSheetDetail paymentsheetdetail = new PaymentSheetDetail();
                    paymentsheetdetail.WorkDate = paymentSheet.WeekEndingDate.AddDays(-4);
                    paymentsheetdetail.Payout = (decimal)Tuesday;
                    paymentsheetdetail.dayOfWeek = (int)paymentSheet.WeekEndingDate.AddDays(-4).DayOfWeek;
                    paymentsheetdetail.PaymentSheet = paymentSheet;
                    _context.PaymentSheetDetails.Add(paymentsheetdetail);
                }
                if(Wednesday != null)
                {
                    PaymentSheetDetail paymentsheetdetail = new PaymentSheetDetail();
                    paymentsheetdetail.WorkDate = paymentSheet.WeekEndingDate.AddDays(-3);
                    paymentsheetdetail.Payout = (decimal)Wednesday;
                    paymentsheetdetail.dayOfWeek = (int)paymentSheet.WeekEndingDate.AddDays(-3).DayOfWeek;
                    paymentsheetdetail.PaymentSheet = paymentSheet;
                    _context.PaymentSheetDetails.Add(paymentsheetdetail);
                }
                if(Thursday != null)
                {
                    PaymentSheetDetail paymentsheetdetail = new PaymentSheetDetail();
                    paymentsheetdetail.WorkDate = paymentSheet.WeekEndingDate.AddDays(-2);
                    paymentsheetdetail.Payout = (decimal)Thursday;
                    paymentsheetdetail.dayOfWeek = (int)paymentSheet.WeekEndingDate.AddDays(-2).DayOfWeek;
                    paymentsheetdetail.PaymentSheet = paymentSheet;
                    _context.PaymentSheetDetails.Add(paymentsheetdetail);
                }
                if(Friday != null)
                {
                    PaymentSheetDetail paymentsheetdetail = new PaymentSheetDetail();
                    paymentsheetdetail.WorkDate = paymentSheet.WeekEndingDate.AddDays(-1);
                    paymentsheetdetail.Payout = (decimal)Friday;
                    paymentsheetdetail.dayOfWeek = (int)paymentSheet.WeekEndingDate.AddDays(-1).DayOfWeek;
                    paymentsheetdetail.PaymentSheet = paymentSheet;
                    _context.PaymentSheetDetails.Add(paymentsheetdetail);
                }
                if(Saturday != null)
                {
                    PaymentSheetDetail paymentsheetdetail = new PaymentSheetDetail();
                    paymentsheetdetail.WorkDate = paymentSheet.WeekEndingDate;
                    paymentsheetdetail.Payout = (decimal)Saturday;
                    paymentsheetdetail.dayOfWeek = (int)paymentSheet.WeekEndingDate.DayOfWeek;
                    paymentsheetdetail.PaymentSheet = paymentSheet;
                    _context.PaymentSheetDetails.Add(paymentsheetdetail);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new {id = paymentSheet.CompanyID});
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }           
        }

        // GET: SubContractors/JobSubContractorDetail/5/1
        public async Task<IActionResult> JobSubContractorDetail(int? jobID, int? companyID)
        {
            if(_signInManager.IsSignedIn(User))
            {               
                if (jobID == null || companyID == null)
                {
                    return NotFound();
                }

                Job job = await _context.Job.FirstOrDefaultAsync(m => m.JobID == jobID);
                SubContractor subContractor = await _context.SubContractor.FirstOrDefaultAsync(e => e.CompanyID == companyID);
                if (job == null || subContractor == null)
                {
                    return NotFound();
                }
                ViewBag.SubContractor = subContractor;
                List<PaymentSheet> paymentsheets = _context.PaymentSheets.Where(t => t.CompanyID == companyID && t.JobID == jobID).ToList();                        
                List<PaymentSheetDisplay> paymentsheetweeks = setPaymentSheet(paymentsheets).OrderBy(t => t.Year).ToList();
                ViewBag.paymentsheetweeks = paymentsheetweeks;
                ViewBag.totalWeeks = paymentsheetweeks.Sum(ps => ps.TotalPayment);
                return View(job);
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }                 
        }

        private List<PaymentSheetDisplay> setPaymentSheet(List<PaymentSheet> paymentsheets) 
        {
            List<PaymentSheetDisplay> psheetDisplays = new List<PaymentSheetDisplay>();
            List<Week> weeks = new List<Week>();
            foreach(var paymentsheet in paymentsheets)
            {
                weeks.Add(_context.Week.First(w => w.EndOfWeekDate == paymentsheet.WeekEndingDate));
                paymentsheet.PaymentSheetDetails = addingPaymentSheetDetail(paymentsheet);
            }
            foreach(var week in weeks)
            {
                PaymentSheetDisplay psheetdisplay = new PaymentSheetDisplay();
                psheetdisplay.WeekName = week.WeekName;
                psheetdisplay.Year = week.EndOfWeekDate.Year;                            
                foreach(var paymentsheet in paymentsheets)
                {   
                                        
                    if(paymentsheet.WeekEndingDate == week.EndOfWeekDate)
                    {
                        foreach(var paymentsheetdetail in paymentsheet.PaymentSheetDetails)
                        {
                            psheetdisplay.PaymentSheetID = paymentsheet.PaymentSheetID;                            
                            switch(paymentsheetdetail.dayOfWeek)
                            {
                                case 0:
                                    psheetdisplay.Sunday = paymentsheetdetail.Payout;
                                    break;
                                case 1:
                                    psheetdisplay.Monday = paymentsheetdetail.Payout;
                                    break;                                                            
                                case 2:
                                    psheetdisplay.Tuesday = paymentsheetdetail.Payout;
                                    break;   
                                case 3:
                                    psheetdisplay.Wednesday = paymentsheetdetail.Payout;
                                    break;                               
                                case 4:
                                    psheetdisplay.Thursday = paymentsheetdetail.Payout;
                                    break;                               
                                case 5:
                                    psheetdisplay.Friday = paymentsheetdetail.Payout;
                                    break;   
                                case 6:
                                    psheetdisplay.Saturday = paymentsheetdetail.Payout;
                                    break;   
                            }
                            psheetdisplay.TotalPayment += paymentsheetdetail.Payout;
                        }
                    }
                }            
                psheetDisplays.Add(psheetdisplay);                                      
            }
            return psheetDisplays;
        }

        private ICollection<PaymentSheetDetail> addingPaymentSheetDetail(PaymentSheet paymentsheet)
        {
            var paymentsheetdetails = _context.PaymentSheetDetails.Where(t => t.PaymentSheetID == paymentsheet.PaymentSheetID).ToList();
            return paymentsheetdetails;
        }
        private SubContractor loadingSubContractorJobs(SubContractor subcontractor)
        {
            List<SubContractorJob> subcontractorjobs = _context.SubContractorJobs.ToList();
            for (int i = subcontractorjobs.Count - 1; i >= 0; i--)
            {
                if (subcontractorjobs[i].CompanyID != subcontractor.CompanyID)
                {
                    subcontractorjobs.RemoveAt(i);
                }
            }
            subcontractor.SubContractorJobs = subcontractorjobs;
            return subcontractor;
        }

        private SubContractor loadingPaymentSheets(SubContractor subcontractor)
        {
            subcontractor.PaymentSheets = _context.PaymentSheets.Where(p => p.CompanyID == subcontractor.CompanyID).ToList();
            foreach(var psd in subcontractor.PaymentSheets)
            {
                psd.PaymentSheetDetails = _context.PaymentSheetDetails.Where(ps => ps.PaymentSheetID == psd.PaymentSheetID).ToList();
            }
            return subcontractor;
        }

        public JsonResult GetMonths(string id)
        {
            int iD = Int32.Parse(id);
            List<Month> months = new List<Month>();
            months = _context.Month.Where(m => m.Year.YearID == iD).ToList();
            //removeMonths
            return Json(new SelectList(months, "MonthID", "MonthName"));
        }

        public JsonResult GetWeeks(string month, int companyId)
        {
            int monthid = Int32.Parse(month);
            List<Week> weeks = _context.Week.Where(w => w.Month.MonthID == monthid).ToList();
            List<PaymentSheet> paymentsheets = _context.PaymentSheets.Where(t => t.CompanyID == companyId).ToList();
            if(paymentsheets.Count != 0) {
                foreach(var paymentsheet in paymentsheets) {                
                    for(int i = weeks.Count -1; i>= 0; i--) {
                        if(weeks[i].EndOfWeekDate == paymentsheet.WeekEndingDate)
                        {
                            weeks.RemoveAt(i);
                        }
                    }
                }     
            } 
            return Json(new SelectList(weeks, "WeekID", "WeekName"));
        }*/
    }
}