using EMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EMS.Controllers
{
    public class CurrentStudentsController : ApiController
    {
        IList<EmViewModel> currStList = null;
        public IHttpActionResult GetCurrentStudents()
        {
            using (var ctx=new EMSEntities())
            {
              currStList=  ctx.Database.SqlQuery<EmViewModel>("SELECT * FROM EMS.EM t1 LEFT JOIN EMS.LSCHOOLDTL t2 ON t2.EM_TRNNO = t1.TRNNO WHERE t1.ETYPE = 'S' and t2.EM_TRNNO IS NULL").ToList<EmViewModel>();
            }
            if (currStList==null)
            {
                return NotFound();
            }
            return Ok(currStList);
        }
    }
}
