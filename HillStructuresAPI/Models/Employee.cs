using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HillStructuresAPI.Models
{
    public class Employee : User
    {
        public string Role { get; set; }
        public ICollection<EmployeeJob> EmployeeJobs {get; set;}
        public ICollection<TimeSheet> TimeSheets { get; set; }
    }
}