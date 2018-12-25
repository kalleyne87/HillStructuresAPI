using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HillStructuresAPI.Models
{
    public class Supplier : Company
    {
        public ICollection<SupplierJob> SupplierJobs {get; set;}
    }
}