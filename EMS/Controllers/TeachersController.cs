using EMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EMS.Controllers
{
    public class TeachersController : ApiController
    {
        public IHttpActionResult GetAllTeachers()
        {
            IList<TeacherViewModel> ems = null;

            using (var ctx = new EMSEntities())
            {
                ems= ctx.Database.SqlQuery<TeacherViewModel>("SELECT TRNNO ,EMP_ID,EMP_NAME,DOB,DOJ,EMP_F_NAME,ETYPE ,EMAIL ,CNIC ,SEX ,BSAL, FCNIC, CST_TRNNO, BP_TRNNO FROM [EMS].[EMS].[EM] where ETYPE='T'").ToList<TeacherViewModel>();
                
            }

            if (ems.Count == 0)
            {
                return NotFound();
            }

            return Ok(ems);
        }
        public IHttpActionResult GetEmById(int id)
        {
            EmViewModel ems = null;

            using (var ctx = new EMSEntities())
            {
                ems = ctx.EMs
                    .Where(s => s.TRNNO == id)
                    .Select(s => new EmViewModel()
                    {
                        TRNNO = s.TRNNO,
                        EMP_NAME = s.EMP_NAME,
                        EMP_F_NAME = s.EMP_F_NAME
                    }).FirstOrDefault<EmViewModel>();
            }

            if (ems == null)
            {
                return NotFound();
            }

            return Ok(ems);
        }
    }
}
