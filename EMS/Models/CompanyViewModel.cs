using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.Models
{
    public class CompanyViewModel
    {
        public double TRNNO { get; set; }
        public string CNAME { get; set; }
        public byte[] PIC { get; set; }
    }
}