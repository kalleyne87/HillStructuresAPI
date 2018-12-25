using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HillStructuresAPI.Models
{
    public class Client : User
    {        
        public string Address { get; set; }
        public ICollection<Job> Jobs {get; set;}
    }
}