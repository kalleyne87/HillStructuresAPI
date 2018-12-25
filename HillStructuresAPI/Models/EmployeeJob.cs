using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HillStructuresAPI.Models
{
    public class EmployeeJob
    {
        [Key]
        public int UserID {get; set;}
        [Key]
        public int JobID {get; set;}

        [ForeignKey("UserID")]    
        public Employee Employee {get; set;}
        [ForeignKey("JobID")]  
        public Job Job {get; set;}
    }
}