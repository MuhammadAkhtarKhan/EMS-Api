using EMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EMS.Controllers
{
    //[RoutePrefix("api")]
    public class StudentController : ApiController
    {

        [HttpGet]
        //[Route("allstudents")]
        public IHttpActionResult GetAllLs()
        {
            IList<StudentViewModel> lss = null;
             //allStudents[] allStudents;

            using (var ctx = new EMSEntities())
            {

                lss = ctx.spAllCurrentStudents().Select(s => new StudentViewModel()
                {

                    TRNNO = s.TRNNO,
                    EMP_ID = s.EMP_ID,
                    EMP_NAME = s.EMP_NAME,
                    // CST_TRNNO = s.CST_TRNNO,
                    EMP_F_NAME = s.EMP_F_NAME,
                    // DOB = s.DOB,
                    //BP_TRNNO = s.BP_TRNNO,
                    //ETYPE = s.ETYPE,
                    //CL_TRNNO = s.CST_TRNNO,
                    //CL_TRNNO1 = s.CL_TRNNO1,
                    //CERTIFICATE = s.CERTIFICATE,
                    //PSCHDT = s.PSCHDT,
                    //EMAIL = s.EMAIL,
                    //ADT = s.ADT,
                    //FTYPE = s.FTYPE,
                    //FEE = s.FEE,
                    //EMAILST = s.EMAILST,
                    //GRPMST_TRNNO = s.GRPMST_TRNNO,
                    //CNIC = s.CNIC,
                    //SECMST_TRNNO = s.SECMST_TRNNO,
                    //SECDTL_SR = s.SECDTL_SR,
                    //SEX = s.SEX,
                    //BSAL = s.BSAL,
                    //FCNIC = s.FCNIC,
                    //ADIMNO = s.ADIMNO,
                    //STYPE = s.STYPE
                }).ToList();
            }

            if (lss.Count == 0)
            {
                return NotFound();
            }

            return Ok(lss);
        }
        [HttpGet]
       // [Route("allstudents/{id}")]
        public IHttpActionResult GetStudentByClassId(int? id)
        {
            IList<StudentViewModel> lss = null;

            using (var ctx = new EMSEntities())
            {
                lss = ctx.spStudentCurrentClass((double)id, 3)                    
                    .Select(s => new StudentViewModel()
                    {
                        TRNNO = s.TRNNO,
                        EMP_ID = s.EMP_ID,
                        EMP_F_NAME = s.EMP_F_NAME,
                        EMP_NAME=s.EMP_NAME
                    }).ToList<StudentViewModel>();
            }

            if (lss == null)
            {
                return NotFound();
            }

            return Ok(lss);
        }

        //[HttpGet]
        //[Route("lschool/detail")]
        //public IHttpActionResult GetAllLsDetail()
        //{
        //    IList<LSchoolDtlViewModel> lsdtl = null;

        //    using (var ctx = new EMSEntities())
        //    {
        //        lsdtl = ctx.LSCHOOLDTLs
        //                    .Select(s => new LSchoolDtlViewModel()
        //                    {
        //                        TRNNO = s.TRNNO,
        //                        EM_TRNNO = s.EM_TRNNO,
        //                        SR = s.SR
        //                    }).ToList<LSchoolDtlViewModel>();
        //    }

        //    if (lsdtl.Count == 0)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(lsdtl);
        //}
        //[HttpPost]
        //[Route("currClStud")]
        public IHttpActionResult PostStudentsByClIdandGrpId(PromotionViewModel st)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");
            
                IList<StudentViewModel> lss = null;

                using (var ctx = new EMSEntities())
                {
                    lss = ctx.spStudentCurrentClass((double)st.CLASS_TRNNO, st.GRPMST_TRNNO)
                        .Select(s => new StudentViewModel()
                        {
                            TRNNO = s.TRNNO,
                            EMP_ID = s.EMP_ID,
                            EMP_F_NAME = s.EMP_F_NAME,
                            EMP_NAME = s.EMP_NAME
                        }).ToList<StudentViewModel>();
                }

                if (lss == null)
                {
                    return NotFound();
                }

                return Ok(lss);
            }
        //public IHttpActionResult Put(LSchoolViewModel bp)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest("Not a valid model");

        //    using (var ctx = new EMSEntities())
        //    {
        //        try
        //        {
        //            var existingLs = ctx.LSCHOOLMSTs.Where(s => s.TRNNO == bp.TRNNO)
        //                                                .FirstOrDefault<LSCHOOLMST>();

        //            if (existingLs != null)
        //            {
        //                existingLs.CLASS_TRNNO = bp.CLASS_TRNNO;
        //                existingLs.LDATE = bp.LDATE;

        //                ctx.SaveChanges();
        //            }
        //            else
        //            {
        //                return NotFound();
        //            }
        //        }
        //        catch (DbEntityValidationException e)
        //        {
        //            foreach (var eve in e.EntityValidationErrors)
        //            {
        //                Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
        //                    eve.Entry.Entity.GetType().Name, eve.Entry.State);
        //                foreach (var ve in eve.ValidationErrors)
        //                {
        //                    Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
        //                        ve.PropertyName, ve.ErrorMessage);
        //                }
        //            }
        //            throw;
        //        }
        //    }

        //    return Ok();
        //}
        //[HttpDelete]
        //public IHttpActionResult DeleteLs(int id)
        //{
        //    if (id <= 0)
        //        return BadRequest("Not a valid student id");

        //    using (var ctx = new EMSEntities())
        //    {
        //        var bp = ctx.LSCHOOLMSTs
        //            .Where(s => s.TRNNO == id)
        //            .FirstOrDefault();
        //        ctx.Entry(bp).State = System.Data.Entity.EntityState.Deleted;
        //        ctx.SaveChanges();
        //    }

        //    return Ok();
        // }
    }
}
