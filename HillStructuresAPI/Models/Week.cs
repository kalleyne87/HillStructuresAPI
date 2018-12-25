using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HillStructuresAPI.Models
{
    public class Week
    {
        [Key]        
        public int WeekID { get; set; }
        public string WeekName { get; set; }
        public DateTime EndOfWeekDate { get; set; }
        public int daysInWeek { get; set; }
        public bool weekEndingMonth { get; set; }
        public int MonthID { get; set; }
        [ForeignKey("MonthID")]    
        public Month Month {get; set; }
    }
}