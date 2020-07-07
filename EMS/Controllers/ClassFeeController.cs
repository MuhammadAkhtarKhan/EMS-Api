using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EMS.Models;

namespace EMS.Controllers
{
    public class ClassFeeController : ApiController
    {
        [HttpGet]       
        public IHttpActionResult GetAllClassFee()
        {
            //IEnumerable<ClassFeeViewModel> markTotal = null;
            IList<ClassFeeViewModel> mks = null;

            using (var ctx = new EMSEntities())
            {
                // mks = ctx.CLFEEMSTs.ToList<CLFEEMST>();
                //var st= ctx.CLFEEMSTs.SqlQuery("select * from ems.CLFEEMST where trnno=19").FirstOrDefault<CLFEEMST>();
                // var nestedquery = from s in ctx.CLFEEMSTs
                //                   from c in s.CLFEEDTLs
                //                   where s.TRNNO == 19
                //                   select new { s.TRNNO, c };
                // var result = nestedquery.ToList();
                //var mktotal=ctx.CLFEEMSTs

                mks = ctx.CLFEEMSTs
                            .Select(s => new ClassFeeViewModel()
                            {
                                TRNNO = s.TRNNO,
                                CLASS_TRNNO = s.CLASS_TRNNO,                               

                            }).ToList<ClassFeeViewModel>();

            }

            if (mks.Count == 0)
            {
                return NotFound();
            }

            return Ok(mks);
        }
        public IHttpActionResult GetClassFeeById(int id)
        {
            ClassFeeViewModel mks = null;

            using (var ctx = new EMSEntities())
            {
                mks = ctx.CLFEEMSTs
                    .Where(s => s.TRNNO == id)
                    .Select(s => new ClassFeeViewModel()
                    {
                        TRNNO = s.TRNNO,
                        CLASS_TRNNO = s.CLASS_TRNNO                       
                    }).FirstOrDefault<ClassFeeViewModel>();
            }
            

            if (mks == null)
            {
                return NotFound();
            }

            return Ok(mks);
        }
       

        //[HttpGet]
        //[Route("markdetail")]
        //public IHttpActionResult GetAllClassFees(ClassFeeViewModel mv)
        //{
        //    IList<CLFEEDTL> mks = null;

        //    using (var ctx = new EMSEntities())
        //    {
        //        int _trnno = Convert.ToInt32(ctx.CLFEEMSTs.Where(s => s.CLASS_TRNNO == mv.CLASS_TRNNO && s.EXAM_TRNNO == mv.EXAM_TRNNO && s.GRPMST_TRNNO == mv.GRPMST_TRNNO && s.MDT == mv.MDT));
        //        mks = ctx.MARKTOTALDTLs
        //            .Where(s => s.TRNNO == _trnno)
        //            .ToList<CLFEEDTL>();
        //    }

        //    if (mks.Count == 0)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(mks);
        //}

        public IHttpActionResult PostNewClassFee(ClassFeeViewModel mt)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            using (var ctx = new EMSEntities())
            {
                int _trnno;
                int _sr = 1;
                //db.Users.OrderByDescending(u => u.UserId).FirstOrDefault();
                if (mt.TRNNO == 0)
                {
                    _trnno = Convert.ToInt32(ctx.CLFEEMSTs.OrderByDescending(t => t.TRNNO).FirstOrDefault().TRNNO);
                    _trnno = _trnno + 1;
                    //_trnno = Convert.ToInt32(ctx.EMs.OrderByDescending(t => t.TRNNO).First().ToString());
                }
                else
                {
                    _trnno = Convert.ToInt32(mt.TRNNO);
                    _sr = Convert.ToInt16(ctx.CLFEEDTLs.Where(x => x.TRNNO == mt.TRNNO)
                           .OrderByDescending(x => x.SR)
                           .First().SR);
                    _sr = _sr + 1;
                }

                // int totalConunt = ctx.CLFEEMSTs.Count<CLFEEMST>();
                //var sr= ctx.Database.SqlQuery<ClassFeeViewModel>("SELECT TOP 1 SR FROM ems.CLFEEDTL where TRNNO= @id ORDER BY SR DESC", new SqlParameter("@id", mt.TRNNO)).ToString();
               
                if (mt.TRNNO==0)
                {
                    ctx.CLFEEMSTs.Add(new CLFEEMST()
                    {
                        TRNNO = _trnno,
                        CLASS_TRNNO = mt.CLASS_TRNNO

                    });
                }
               
                foreach (var dtls in mt.CLFEEDTLs)
                {
                    var markdetail = new CLFEEDTL
                    {
                        //BudgetId = Here i need to get id of the Budget table that i inserted before
                        TRNNO = _trnno,
                        SR = _sr,
                       FEE=dtls.FEE,
                       ADT=dtls.ADT
                    };
                    ctx.CLFEEDTLs.Add(markdetail);
                    _sr++;
                }
                try
                {
                    ctx.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    // Retrieve the error messages as a list of strings.
                    var errorMessages = ex.EntityValidationErrors
                            .SelectMany(x => x.ValidationErrors)
                            .Select(x => x.ErrorMessage);

                    // Join the list to a single string.
                    var fullErrorMessage = string.Join("; ", errorMessages);

                    // Combine the original exception message with the new one.
                    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                    // Throw a new DbEntityValidationException with the improved exception message.
                    throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                }
            }

            return Ok();
        }
        public IHttpActionResult Put(ClassFeeViewModel mt)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            using (var ctx = new EMSEntities())
            {
                try
                {
                    var existingClassFee = ctx.CLFEEMSTs.Where(s => s.TRNNO == mt.TRNNO)
                                                        .FirstOrDefault<CLFEEMST>();

                    if (existingClassFee != null)
                    {
                        existingClassFee.CLASS_TRNNO = mt.CLASS_TRNNO;
                       
                        foreach (var dtl in mt.CLFEEDTLs)
                        {
                            var mkdetail = new CLFEEDTL
                            {

                                TRNNO = dtl.TRNNO,
                                SR = dtl.SR,
                                FEE = dtl.FEE,
                                ADT = dtl.ADT
                            };
                            ctx.Entry(mkdetail).State = System.Data.Entity.EntityState.Modified;                           
                        }

                        ctx.SaveChanges();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }
            }

            return Ok();
        }
        [HttpDelete]
        public IHttpActionResult DeleteClassFee(int id)
        {
            if (id <= 0)
                return BadRequest("Not a valid student id");

            using (var ctx = new EMSEntities())
            {
                var mt = ctx.CLFEEMSTs
                    .Where(s => s.TRNNO == id)
                    .FirstOrDefault();
                ctx.Entry(mt).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }

            return Ok();
        }
    }
}
