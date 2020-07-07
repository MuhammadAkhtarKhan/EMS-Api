using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.Models
{
    public class UserViewModel
    {
        public double Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Nullable<decimal> STATUS { get; set; }
    }
}