using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EMS.Models;

namespace EMS.Controllers
{
    [RoutePrefix("api")]
    public class MarkTotalController : ApiController
    {
        [HttpGet]
        [Route("markTotal")]
        public IHttpActionResult GetAllMarkTotal()
        {
            //IEnumerable<MarkTotalViewModel> markTotal = null;
            IList<MarkTotalViewModel> mks = null;

            using (var ctx = new EMSEntities())
           {
                // mks = ctx.MARKTOTALs.ToList<MARKTOTAL>();
                //var st= ctx.MARKTOTALs.SqlQuery("select * from ems.MARKTOTAL where trnno=19").FirstOrDefault<MARKTOTAL>();
                // var nestedquery = from s in ctx.MARKTOTALs
                //                   from c in s.MARKTOTALDTLs
                //                   where s.TRNNO == 19
                //                   select new { s.TRNNO, c };
                // var result = nestedquery.ToList();
               //var mktotal=ctx.MARKTOTALs

                mks = ctx.MARKTOTALs
                            .Select(s => new MarkTotalViewModel()
                            {
                                TRNNO = s.TRNNO,
                                CLASS_TRNNO = s.CLASS_TRNNO,
                                EXAM_TRNNO = s.EXAM_TRNNO,
                                GRPMST_TRNNO = s.GRPMST_TRNNO,
                                MDT = s.MDT,

                            }).ToList<MarkTotalViewModel>();

            }

            if (mks.Count == 0)
            {
                return NotFound();
            }

            return Ok(mks);
        }
        public IHttpActionResult GetMarkTotalById(int id)
        {
            MarkTotalViewModel mks = null;

            using (var ctx = new EMSEntities())
            {
                mks = ctx.MARKTOTALs
                    .Where(s => s.TRNNO == id)
                    .Select(s => new MarkTotalViewModel()
                    {
                        TRNNO = s.TRNNO,
                        CLASS_TRNNO = s.CLASS_TRNNO,
                        EXAM_TRNNO = s.EXAM_TRNNO,
                        GRPMST_TRNNO = s.GRPMST_TRNNO,
                        MDT = s.MDT
                    }).FirstOrDefault<MarkTotalViewModel>();
            }

            if (mks == null)
            {
                return NotFound();
            }

            return Ok(mks);
        }

        [HttpGet]
        [Route("markdetail")]
        public IHttpActionResult GetAllMarkTotals(MarkTotalViewModel mv)
        {
            IList<MARKTOTALDTL> mks = null;

            using (var ctx = new EMSEntities())
            {
                int _trnno = Convert.ToInt32(ctx.MARKTOTALs.Where(s => s.CLASS_TRNNO == mv.CLASS_TRNNO && s.EXAM_TRNNO == mv.EXAM_TRNNO && s.GRPMST_TRNNO == mv.GRPMST_TRNNO && s.MDT == mv.MDT));
                mks = ctx.MARKTOTALDTLs
                    .Where(s =>s.TRNNO==_trnno)
                    .ToList<MARKTOTALDTL>();
            }

            if (mks.Count == 0)
            {
                return NotFound();
            }

            return Ok(mks);
        }

        [HttpPost]
        [Route("markTotal")]        
        public IHttpActionResult PostNewMarkTotal(MarkTotalViewModel mt)
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
                    _trnno = Convert.ToInt32(ctx.MARKTOTALs.OrderByDescending(t => t.TRNNO).FirstOrDefault().TRNNO);
                    _trnno = _trnno + 1;
                    //_trnno = Convert.ToInt32(ctx.EMs.OrderByDescending(t => t.TRNNO).First().ToString());
                }
                else
                {
                    _trnno = Convert.ToInt32(mt.TRNNO) + 1;
                }
               // int totalConunt = ctx.MARKTOTALs.Count<MARKTOTAL>();
                mt.TRNNO = _trnno;
                ctx.MARKTOTALs.Add(new MARKTOTAL()
                {
                    TRNNO = mt.TRNNO,
                    CLASS_TRNNO = mt.CLASS_TRNNO,
                    EXAM_TRNNO = mt.EXAM_TRNNO,
                    GRPMST_TRNNO = mt.GRPMST_TRNNO,
                    MDT = mt.MDT
                });
                foreach (var dtls in mt.MARKTOTALDTLs)
                {
                    var markdetail = new MARKTOTALDTL
                    {
                        //BudgetId = Here i need to get id of the Budget table that i inserted before
                        TRNNO = _trnno,
                        SR = _sr,
                        TOTMARKS = dtls.TOTMARKS,
                        SUBJECT_TRNNO = dtls.TRNNO,
                    };
                    ctx.MARKTOTALDTLs.Add(markdetail);
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
        public IHttpActionResult Put(MarkTotalViewModel mt)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            using (var ctx = new EMSEntities())
            {
                try
                {
                    var existingMarkTotal = ctx.MARKTOTALs.Where(s => s.TRNNO == mt.TRNNO)
                                                        .FirstOrDefault<MARKTOTAL>();

                    if (existingMarkTotal != null)
                    {
                        existingMarkTotal.CLASS_TRNNO = mt.CLASS_TRNNO;
                        existingMarkTotal.EXAM_TRNNO = mt.EXAM_TRNNO;
                        existingMarkTotal.GRPMST_TRNNO = mt.GRPMST_TRNNO;
                        existingMarkTotal.MDT = mt.MDT;
                        foreach (var dtl in mt.MARKTOTALDTLs)
                        {
                            var mkdetail = new MARKTOTALDTL
                            {
                                
                                TRNNO = dtl.MKTOTAL_TRNNO,
                                SR = dtl.SR,
                                SUBJECT_TRNNO=dtl.TRNNO,
                                TOTMARKS=dtl.TOTMARKS
                            };
                            var subject = new SUBJECT
                            {
                                TRNNO=dtl.TRNNO,
                                 SNAME=dtl.SNAME,
                                 SCODE=dtl.SCODE
                            };

                            ctx.Entry(mkdetail).State = System.Data.Entity.EntityState.Modified;
                            ctx.Entry(subject).State = System.Data.Entity.EntityState.Modified;
                            //ctx.MARKTOTALDTLs.Add(mkdetail);
                            
                            //ctx.SUBJECTs.Add(subject);
                            
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
        public IHttpActionResult DeleteMarkTotal(int id)
        {
            if (id <= 0)
                return BadRequest("Not a valid student id");

            using (var ctx = new EMSEntities())
            {
                var mt = ctx.MARKTOTALs
                    .Where(s => s.TRNNO == id)
                    .FirstOrDefault();
                ctx.Entry(mt).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }

            return Ok();
        }
    }
}

