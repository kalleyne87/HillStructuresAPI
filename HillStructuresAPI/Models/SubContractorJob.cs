using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HillStructuresAPI.Models
{
    public class SubContractorJob
    {
        [Key]
        public int CompanyID {get; set;}
        [Key]
        public int JobID {get; set;}
        public virtual SubContractor SubContractor {get; set;}
        public virtual Job Job {get; set;}
    }
}