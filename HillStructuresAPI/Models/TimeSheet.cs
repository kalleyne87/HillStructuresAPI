using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HillStructuresAPI.Models
{
    public class TimeSheet
    {
        [Key]        
        public int TimeSheetID { get; set; }
        public DateTime WeekEndingDate { get; set; }
        public decimal payrate { get; set; }
        public int JobID { get; set; }

        public int EmployeeID { get; set; }

        [ForeignKey("JobID")]
        public Job Job {get; set;}
        
        [ForeignKey("EmployeeID")]
        public Employee Employee { get; set; }
        public ICollection<TimeSheetDetail> TimeSheetDetails { get; set; }
    }
}