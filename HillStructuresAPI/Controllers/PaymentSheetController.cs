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
    [Route("api/PaymentSheet")]
    public class PaymentSheetsController : ControllerBase
    {

        private readonly HillStructuresContext _context;         

        public PaymentSheetsController(HillStructuresContext context)
        {
            _context = context;            
        }

        // GET api/PaymentSheets/get
        [EnableCors("AllowAll")]
        [Produces("application/json")]
        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var paymentSheets = _context.PaymentSheets.ToList();
                return Ok(paymentSheets);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        // GET api/PaymentSheets/get/2
        [EnableCors("AllowAll")]
        [Produces("application/json")]
        [HttpGet("Get/{paymentSheetID}")]
        public async Task<IActionResult> Get(int PaymentSheetID)
        {
            try
            {
                var paymentSheet = _context.PaymentSheets.SingleOrDefault(p => p.PaymentSheetID == PaymentSheetID);
                return Ok(paymentSheet);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        // POST api/PaymentSheets/create
        [EnableCors("AllowAll")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] PaymentSheet paymentSheet)
        {
            try
            {
                _context.PaymentSheets.Add(paymentSheet);
                _context.SaveChanges();
                return Ok(paymentSheet);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        // PUT api/PaymentSheet/update
        [EnableCors("AllowAll")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] PaymentSheet paymentSheet)
        {
            try
            {
                _context.Entry(paymentSheet).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
                return Ok(paymentSheet);
            }
            catch (Exception e)
            {
                return BadRequest(e);

            }
        }

        // DELETE api/PaymentSheet/delete/4
        [EnableCors("AllowAll")]
        [HttpDelete("Delete/{paymentSheetID}")]
        public async Task<IActionResult> Delete(int paymentSheetID)
        {
            try
            {
                _context.Remove(_context.PaymentSheets.Find(paymentSheetID));
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        /*// GET: PaymentSheet/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(_signInManager.IsSignedIn(User))
            {              
                if (id == null)
                {
                    return NotFound();
                }

                var paymentsheet = await _context.PaymentSheets
                    .FirstOrDefaultAsync(m => m.PaymentSheetID == id);
                if (paymentsheet == null)
                {
                    return NotFound();
                }
                ViewBag.paymentSheetDisplay = setPaymentSheet(paymentsheet);
                return View(paymentsheet);
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
                var paymentSheet = await _context.PaymentSheets.FindAsync(id);
                var supplier = await _context.Supplier.FindAsync(paymentSheet.CompanyID);
                _context.PaymentSheets.Remove(paymentSheet);
                await _context.SaveChangesAsync();
                if(supplier != null)
                {
                    return RedirectToAction("JobSupplierDetail", "Suppliers", new { jobID = paymentSheet.JobID, companyID = paymentSheet.CompanyID });
                } else
                {
                    return RedirectToAction("JobSubContractorDetail", "SubContractors", new { jobID = paymentSheet.JobID, companyID = paymentSheet.CompanyID });       
                }         
            } else 
            {
                return RedirectToAction("Login", "Account");                
            }                   
        }

        private PaymentSheetDisplay setPaymentSheet(PaymentSheet paymentsheet) 
        {
            PaymentSheetDisplay psheetdisplay = new PaymentSheetDisplay();
            psheetdisplay.WeekName = _context.Week.First(w => w.EndOfWeekDate == paymentsheet.WeekEndingDate).WeekName;
            paymentsheet.PaymentSheetDetails = addingPaymentSheetDetail(paymentsheet);
            psheetdisplay.PaymentSheetID = paymentsheet.PaymentSheetID;                      
            {
                foreach(var paymentsheetdetail in paymentsheet.PaymentSheetDetails)
                {
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
            return psheetdisplay;
        }

        private ICollection<PaymentSheetDetail> addingPaymentSheetDetail(PaymentSheet paymentsheet)
        {
            var paymentsheetdetails = _context.PaymentSheetDetails.Where(t => t.PaymentSheetID == paymentsheet.PaymentSheetID).ToList();
            return paymentsheetdetails;
        }*/
    }
}