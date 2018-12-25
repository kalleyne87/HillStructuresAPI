using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HillStructuresAPI.Models
{
    public class TimeSheetDisplay
    {
        public int TimeSheetID { get; set; } 
        public string WeekName { get; set; }
        public int Year { get; set; }
        [DataType(DataType.Currency)]
        public Employee Employee { get; set;}
        public decimal PayRate { get; set; }
        public decimal Sunday { get; set; }
        public decimal Monday { get; set; }        
        public decimal Tuesday { get; set; }
        public decimal Wednesday { get; set; }
        public decimal Thursday { get; set; }
        public decimal Friday { get; set; }        
        public decimal Saturday { get; set; }
        public decimal TotalHours { get; set; }
        [DataType(DataType.Currency)]
        public decimal TotalPayout { get; set; }
    }
}