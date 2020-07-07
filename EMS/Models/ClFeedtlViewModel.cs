using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.Models
{
    public class ClFeedtlViewModel
    {
        public double TRNNO { get; set; }
        public double SR { get; set; }
        public Nullable<double> FEE { get; set; }
        public Nullable<System.DateTime> ADT { get; set; }

        public virtual CLFEEMST CLFEEMST { get; set; }
    }
}