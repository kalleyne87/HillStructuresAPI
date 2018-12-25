using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HillStructuresAPI.Models
{
    public class Year
    {
        [Key]        
        public int YearID  { get; set; }
        public int YearDate { get; set; }
        public ICollection<Month> Months {get; set; }
    }
}