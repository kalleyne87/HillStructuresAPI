using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HillStructuresAPI.Models
{
    public class PaymentSheetDetail
    {
        [Key]        
        public int PaymentSheetDetailID { get; set; }
        public DateTime WorkDate { get; set; }
        public int dayOfWeek { get; set; }
        [DataType(DataType.Currency)]
        public decimal Payout { get; set; }
        public int PaymentSheetID { get; set; }

        [ForeignKey("PaymentSheetID")]
        public PaymentSheet PaymentSheet {get; set; }
    }
}