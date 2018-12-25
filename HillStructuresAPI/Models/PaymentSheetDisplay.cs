using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HillStructuresAPI.Models
{
    public class PaymentSheetDisplay
    {
        public int PaymentSheetID { get; set; } 
        public string WeekName { get; set; }
        public int Year { get; set; }
        [DataType(DataType.Currency)]
        public Company Company { get; set; }
        public decimal Sunday { get; set; }
        [DataType(DataType.Currency)]
        public decimal Monday { get; set; }
        [DataType(DataType.Currency)]
        public decimal Tuesday { get; set; }
        [DataType(DataType.Currency)]
        public decimal Wednesday { get; set; }
        [DataType(DataType.Currency)]
        public decimal Thursday { get; set; }
        [DataType(DataType.Currency)]
        public decimal Friday { get; set; }
        [DataType(DataType.Currency)]
        public decimal Saturday { get; set; }
        [DataType(DataType.Currency)]
        public decimal TotalPayment { get; set; }
    }
}