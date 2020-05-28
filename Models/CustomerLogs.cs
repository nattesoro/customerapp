using System;
using System.ComponentModel.DataAnnotations;

namespace CustomerApp.Models
{
    public class CustomerLog
    {
        public int CustomerLogID { get; set; }
        public int CustomerID { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime LogDateTime { get; set; }
        public string LogDescription { get; set; }
        public Customer Customer { get; set; }
    }
}
