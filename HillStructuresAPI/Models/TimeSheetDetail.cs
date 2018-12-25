using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HillStructuresAPI.Models
{
    public class TimeSheetDetail
    {
        [Key]        
        public int TimeSheetDetailID { get; set; }
        public DateTime WorkDate { get; set; }
        public int dayOfWeek { get; set; }
        public decimal hours { get; set; }
        public int TimeSheetID { get; set; }

        [ForeignKey("TimeSheetID")]
        public TimeSheet TimeSheet {get; set; }
    }
}