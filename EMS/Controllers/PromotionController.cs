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
    public class PromotionController : ApiController
    {
        public IHttpActionResult GetAllPromotions()
        {
            //IEnumerable<PromotionViewModel> markTotal = null;
            IList<PromotionViewModel> fcs = null;

            using (var ctx = new EMSEntities())
            {
                // fcs = ctx.PROMMSTs.ToList<MARKTOTAL>();
                //var st= ctx.PROMMSTs.SqlQuery("select * from ems.MARKTOTAL where trnno=19").FirstOrDefault<MARKTOTAL>();
                // var nestedquery = from s in ctx.PROMMSTs
                //                   from c in s.MARKTOTALDTLs
                //                   where s.TRNNO == 19
                //                   select new { s.TRNNO, c };
                // var result = nestedquery.ToList();
                //var mktotal=ctx.PROMMSTs

                fcs = ctx.PROMMSTs
                            .Select(s => new PromotionViewModel()
                            {
                                TRNNO = s.TRNNO,
                                PDT = s.PDT,
                                FCLASS_TRNNO = s.FCLASS_TRNNO,
                                GRPMST_TRNNO = s.GRPMST_TRNNO,                              
                                CLASS_TRNNO = s.CLASS_TRNNO,
                                SECDTL_TRNNO = s.SECDTL_TRNNO,
                                SECDTL_SR = s.SECDTL_SR,                               

                            }).ToList<PromotionViewModel>();

            }

            if (fcs.Count == 0)
            {
                return NotFound();
            }

            return Ok(fcs);
        }
        [HttpGet]
        [Route("promotion/detail")]
        public IHttpActionResult GetPromotionDetails()
        {
            //IEnumerable<PromotionViewModel> markTotal = null;
            IList<PromotionDtlViewModel> pmdtl = null;

            using (var ctx = new EMSEntities())
            {
               
                pmdtl = ctx.PROMDTLs
                            .Select(s => new PromotionDtlViewModel()
                            {
                                TRNNO = s.TRNNO,
                                SR = s.SR,
                                EM_TRNNO = s.EM_TRNNO,
                                ADIMNO = s.ADIMNO,
                                STATUS   = s.STATUS 
                            }).ToList<PromotionDtlViewModel>();

            }
            if (pmdtl.Count == 0)
            {
                return NotFound();
            }

            return Ok(pmdtl);
        }
        public IHttpActionResult GetFeeCollectById(int id)
        {
            PromotionViewModel fcs = null;

            using (var ctx = new EMSEntities())
            {
                fcs = ctx.PROMMSTs
                    .Where(s => s.TRNNO == id)
                    .Select(s => new PromotionViewModel()
                    {
                        TRNNO = s.TRNNO,
                        PDT = s.PDT,
                        FCLASS_TRNNO = s.FCLASS_TRNNO,
                        GRPMST_TRNNO = s.GRPMST_TRNNO,                       
                        CLASS_TRNNO = s.CLASS_TRNNO,
                        SECDTL_TRNNO = s.SECDTL_TRNNO,
                        SECDTL_SR = s.SECDTL_SR,                       
                    }).FirstOrDefault<PromotionViewModel>();
            }

            if (fcs == null)
            {
                return NotFound();
            }

            return Ok(fcs);
        }


        //public IHttpActionResult GetAllFeeCollects(PromotionViewModel mv)
        //{
        //    IList<MARKTOTALDTL> fcs = null;

        //    using (var ctx = new EMSEntities())
        //    {
        //        int _trnno = Convert.ToInt32(ctx.PROMMSTs.Where(s => s.CLASS_TRNNO == mv.CLASS_TRNNO && s.EXAM_TRNNO == mv.EXAM_TRNNO && s.GRPMST_TRNNO == mv.GRPMST_TRNNO && s.MDT == mv.MDT));
        //        fcs = ctx.MARKTOTALDTLs
        //            .Where(s => s.TRNNO == _trnno)
        //            .ToList<MARKTOTALDTL>();
        //    }

        //    if (fcs.Count == 0)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(fcs);
        //}


        public IHttpActionResult PostNewFeeCollect(PromotionViewModel fc)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            using (var ctx = new EMSEntities())
            {
                int _trnno;
                int _sr = 1;
                //db.Users.OrderByDescending(u => u.UserId).FirstOrDefault();
                if (fc.TRNNO == 0)
                {
                    _trnno = Convert.ToInt32(ctx.PROMMSTs.OrderByDescending(t => t.TRNNO).FirstOrDefault().TRNNO);
                    _trnno = _trnno + 1;
                    //_trnno = Convert.ToInt32(ctx.EMs.OrderByDescending(t => t.TRNNO).First().ToString());
                }
                else
                {
                    _trnno = Convert.ToInt32(fc.TRNNO) + 1;
                }
                // int totalConunt = ctx.PROMMSTs.Count<MARKTOTAL>();
                fc.TRNNO = _trnno;
                ctx.PROMMSTs.Add(new PROMMST()
                {
                    TRNNO = fc.TRNNO,
                    PDT = fc.PDT,
                    FCLASS_TRNNO = fc.FCLASS_TRNNO,
                    GRPMST_TRNNO = fc.GRPMST_TRNNO,                   
                    CLASS_TRNNO = fc.CLASS_TRNNO,
                    SECDTL_TRNNO = fc.SECDTL_TRNNO,
                    SECDTL_SR = fc.SECDTL_SR,                   
                });
                foreach (var dtls in fc.PROMDTLs)
                {
                    var fcdetail = new PROMDTL
                    {
                       
                        TRNNO = _trnno,
                        SR = _sr,
                        EM_TRNNO = dtls.EM_TRNNO,
                        STATUS = dtls.STATUS,                       
                        ADIMNO = dtls.ADIMNO,                       
                    };
                    ctx.PROMDTLs.Add(fcdetail);
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
        public IHttpActionResult Put(PromotionViewModel fc)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            using (var ctx = new EMSEntities())
            {
                try
                {
                    var existingFeeCollect = ctx.PROMMSTs.Where(s => s.TRNNO == fc.TRNNO)
                                                        .FirstOrDefault<PROMMST>();

                    if (existingFeeCollect != null)
                    {
                        existingFeeCollect.PDT = fc.PDT;
                        existingFeeCollect.FCLASS_TRNNO = fc.FCLASS_TRNNO;
                        existingFeeCollect.GRPMST_TRNNO = fc.GRPMST_TRNNO;
                       
                        existingFeeCollect.CLASS_TRNNO = fc.CLASS_TRNNO;
                        existingFeeCollect.SECDTL_TRNNO = fc.SECDTL_TRNNO;
                        existingFeeCollect.SECDTL_SR = fc.SECDTL_SR;                       

                        foreach (var dtl in fc.PROMDTLs)
                        {
                            var mkdetail = new PROMDTL
                            {
                                TRNNO = dtl.TRNNO,
                                SR = dtl.SR,
                                EM_TRNNO = dtl.EM_TRNNO,
                                STATUS = dtl.STATUS,
                                ADIMNO = dtl.ADIMNO,
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
        public IHttpActionResult DeleteFeeCollect(int id)
        {
            if (id <= 0)
                return BadRequest("Not a valid student id");

            using (var ctx = new EMSEntities())
            {
                var fc = ctx.PROMMSTs
                    .Where(s => s.TRNNO == id)
                    .FirstOrDefault();
                ctx.Entry(fc).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }
            return Ok();
        }
    }
}
