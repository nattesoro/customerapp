using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CustomerApp.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Full Name")]
        public string FullName { get; set; }

        [DisplayName("Email Address")]
        public string Email { get; set; }

        [DisplayName("Mobile Number")]
        public string MobileNumber { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:d}")]
        [DisplayName("Date Created")]
        public DateTime CreateDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        [DisplayName("Date Updated")]
        public DateTime UpdateDate { get; set; }

        public ICollection<CustomerLog> CustomerLogs { get; set; }

    }
}
