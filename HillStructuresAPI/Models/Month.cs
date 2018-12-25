using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HillStructuresAPI.Models
{
    public class Month
    {
        [Key]        
        public int MonthID { get; set; }
        public string MonthName { get; set; }
        public int YearID {get; set; }

        [ForeignKey("YearID")]
        public Year Year { get; set; }
        public ICollection<Week> Weeks {get; set; }
    }
}