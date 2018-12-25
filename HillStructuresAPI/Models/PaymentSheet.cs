using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HillStructuresAPI.Models
{
    public class PaymentSheet
    {
        [Key]        
        public int PaymentSheetID { get; set; }
        public DateTime WeekEndingDate { get; set; }
        public int JobID { get; set; }

        public int CompanyID { get; set; }

        [ForeignKey("JobID")]
        public Job Job {get; set;}
        
        [ForeignKey("CompanyID")]
        public Company Company { get; set; }

        public ICollection<PaymentSheetDetail> PaymentSheetDetails { get; set; }
    }
}