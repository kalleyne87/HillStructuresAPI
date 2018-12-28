using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HillStructuresAPI.Models
{
    public class Job
    {
        [Key]
        public int JobID { get; set; }
        [Display(Name = "Job")]
        public string JobName { get; set; }
        public string Address { get; set; }
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        [Display(Name = "Cost Estimate")]
        [DataType(DataType.Currency)]
        public decimal CostEstimate { get; set; }
        public int ClientID { get; set; }

        [ForeignKey("ClientID")]
        public Client Client { get; set; }
        public ICollection<TimeSheet> TimeSheets { get; set; }
        public ICollection<EmployeeJob> EmployeeJobs {get; set;}
        public ICollection<SubContractorJob> SubContractorJobs {get; set;}
        public ICollection<SupplierJob> SupplierJobs {get; set;}
    }
}
