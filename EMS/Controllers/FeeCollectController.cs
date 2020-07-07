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
    public class FeeCollectController : ApiController
    {
       
        public IHttpActionResult GetAllFeeCollect()
        {
            //IEnumerable<FeeCollectViewModel> markTotal = null;
            IList<FeeCollectViewModel> fcs = null;

            using (var ctx = new EMSEntities())
            {
                // fcs = ctx.FEECOLLECTMSTs.ToList<MARKTOTAL>();
                //var st= ctx.FEECOLLECTMSTs.SqlQuery("select * from ems.MARKTOTAL where trnno=19").FirstOrDefault<MARKTOTAL>();
                // var nestedquery = from s in ctx.FEECOLLECTMSTs
                //                   from c in s.MARKTOTALDTLs
                //                   where s.TRNNO == 19
                //                   select new { s.TRNNO, c };
                // var result = nestedquery.ToList();
                //var mktotal=ctx.FEECOLLECTMSTs

                fcs = ctx.FEECOLLECTMSTs
                            .Select(s => new FeeCollectViewModel()
                            {
                                TRNNO = s.TRNNO,
                                RDATE = s.RDATE,
                                EM_TRNNO = s.EM_TRNNO,
                                FMONTH = s.FMONTH,
                                DDATE = s.DDATE,
                                LDATE = s.LDATE,
                                FSTATUS = s.FSTATUS,
                                DISCOUNT = s.DISCOUNT,
                                DISCOUNTTYPE = s.DISCOUNTTYPE,
                                CLASS_TRNNO = s.CLASS_TRNNO, 
                                SECDTL_TRNNO=s.SECDTL_TRNNO,
                                SECDTL_SR=s.SECDTL_SR,                              
                                PDATE = s.PDATE, 
                                ARRFLG=s.ARRFLG,

                            }).ToList<FeeCollectViewModel>();

            }

            if (fcs.Count == 0)
            {
                return NotFound();
            }

            return Ok(fcs);
        }
        public IHttpActionResult GetFeeCollectById(int id)
        {
            FeeCollectViewModel fcs = null;

            using (var ctx = new EMSEntities())
            {
                fcs = ctx.FEECOLLECTMSTs
                    .Where(s => s.TRNNO == id)
                    .Select(s => new FeeCollectViewModel()
                    {
                        TRNNO = s.TRNNO,
                        RDATE = s.RDATE,
                        EM_TRNNO = s.EM_TRNNO,
                        FMONTH = s.FMONTH,
                        DDATE = s.DDATE,
                        LDATE = s.LDATE,
                        FSTATUS = s.FSTATUS,
                        DISCOUNT = s.DISCOUNT,
                        DISCOUNTTYPE = s.DISCOUNTTYPE,
                        CLASS_TRNNO = s.CLASS_TRNNO,
                        SECDTL_TRNNO = s.SECDTL_TRNNO,
                        SECDTL_SR = s.SECDTL_SR,
                        PDATE = s.PDATE,
                        ARRFLG = s.ARRFLG,
                    }).FirstOrDefault<FeeCollectViewModel>();
            }

            if (fcs == null)
            {
                return NotFound();
            }

            return Ok(fcs);
        }


        //public IHttpActionResult GetAllFeeCollects(FeeCollectViewModel mv)
        //{
        //    IList<MARKTOTALDTL> fcs = null;

        //    using (var ctx = new EMSEntities())
        //    {
        //        int _trnno = Convert.ToInt32(ctx.FEECOLLECTMSTs.Where(s => s.CLASS_TRNNO == mv.CLASS_TRNNO && s.EXAM_TRNNO == mv.EXAM_TRNNO && s.GRPMST_TRNNO == mv.GRPMST_TRNNO && s.MDT == mv.MDT));
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


        //public IHttpActionResult PostNewFeeCollect(FeeCollectViewModel fc)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest("Invalid data.");

        //    using (var ctx = new EMSEntities())
        //    {
        //        int _trnno;
        //        int _sr = 1;
        //        //db.Users.OrderByDescending(u => u.UserId).FirstOrDefault();
        //        if (fc.TRNNO == 0)
        //        {
        //            _trnno = Convert.ToInt32(ctx.FEECOLLECTMSTs.OrderByDescending(t => t.TRNNO).FirstOrDefault().TRNNO);
        //            _trnno = _trnno + 1;
        //            //_trnno = Convert.ToInt32(ctx.EMs.OrderByDescending(t => t.TRNNO).First().ToString());
        //        }
        //        else
        //        {
        //            _trnno = Convert.ToInt32(fc.TRNNO) + 1;
        //        }
        //        // int totalConunt = ctx.FEECOLLECTMSTs.Count<MARKTOTAL>();
        //        fc.TRNNO = _trnno;
        //        ctx.FEECOLLECTMSTs.Add(new FEECOLLECTMST()
        //        {
        //            TRNNO = fc.TRNNO,
        //            RDATE = fc.RDATE,
        //            EM_TRNNO = fc.EM_TRNNO,
        //            FMONTH = fc.FMONTH,
        //            DDATE = fc.DDATE,
        //            LDATE = fc.LDATE,
        //            FSTATUS = fc.FSTATUS,
        //            DISCOUNT = fc.DISCOUNT,
        //            DISCOUNTTYPE = fc.DISCOUNTTYPE,
        //            CLASS_TRNNO = fc.CLASS_TRNNO,
        //            SECDTL_TRNNO = fc.SECDTL_TRNNO,
        //            SECDTL_SR = fc.SECDTL_SR,
        //            PDATE = fc.PDATE,
        //            ARRFLG = fc.ARRFLG,
        //        });
        //        foreach (var dtls in fc.FEECOLLECTDTLs)
        //        {
        //            var fcdetail = new FEECOLLECTDTL
        //            {
        //                //BudgetId = Here i need to get id of the Budget table that i inserted before
        //                TRNNO = _trnno,
        //                FMONTH=dtls.FMONTH,
        //                FEETYPE_TRNNO=dtls.FEETYPE_TRNNO,
        //                SR = _sr,
        //                AMT = dtls.AMT,
        //                FEEMON = dtls.FEEMON,
        //            };
        //            ctx.FEECOLLECTDTLs.Add(fcdetail);
        //            _sr++;
        //        }
        //        try
        //        {
        //            ctx.SaveChanges();
        //        }
        //        catch (DbEntityValidationException ex)
        //        {
        //            // Retrieve the error messages as a list of strings.
        //            var errorMessages = ex.EntityValidationErrors
        //                    .SelectMany(x => x.ValidationErrors)
        //                    .Select(x => x.ErrorMessage);

        //            // Join the list to a single string.
        //            var fullErrorMessage = string.Join("; ", errorMessages);

        //            // Combine the original exception message with the new one.
        //            var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

        //            // Throw a new DbEntityValidationException with the improved exception message.
        //            throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
        //        }
        //    }

        //    return Ok();
        //}
        public IHttpActionResult PostNewFeeCollect(FeeCollectViewModel fc)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            using (var ctx = new EMSEntities())
            {

               var result= ctx.ALLCLASSVOUCHERS(fc.RDATE, fc.TRNNO, fc.DDATE, fc.FMONTH, fc.LDATE);
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

        public IHttpActionResult Put(FeeCollectViewModel fc)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            using (var ctx = new EMSEntities())
            {
                try
                {
                    var existingFeeCollect = ctx.FEECOLLECTMSTs.Where(s => s.TRNNO == fc.TRNNO)
                                                        .FirstOrDefault<FEECOLLECTMST>();

                    if (existingFeeCollect != null)
                    {
                        existingFeeCollect.RDATE = fc.RDATE;
                        existingFeeCollect.EM_TRNNO = fc.EM_TRNNO;
                        existingFeeCollect.FMONTH = fc.FMONTH;
                        existingFeeCollect.DDATE = fc.DDATE;
                        existingFeeCollect.LDATE = fc.LDATE;
                        existingFeeCollect.FSTATUS = fc.FSTATUS;
                        existingFeeCollect.DISCOUNT = fc.DISCOUNT;
                        existingFeeCollect.DISCOUNTTYPE = fc.DISCOUNTTYPE;
                        existingFeeCollect.CLASS_TRNNO = fc.CLASS_TRNNO;
                        existingFeeCollect.SECDTL_TRNNO = fc.SECDTL_TRNNO;
                        existingFeeCollect.SECDTL_SR = fc.SECDTL_SR;
                        existingFeeCollect.PDATE = fc.PDATE;
                        existingFeeCollect.ARRFLG = fc.ARRFLG;

                        foreach (var dtl in fc.FEECOLLECTDTLs)
                        {
                            var mkdetail = new FEECOLLECTDTL
                            {
                                TRNNO = dtl.TRNNO,
                                FMONTH = dtl.FMONTH,
                                FEETYPE_TRNNO = dtl.FEETYPE_TRNNO,
                                SR = dtl.SR,
                                AMT = dtl.AMT,
                                FEEMON = dtl.FEEMON,
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
                var fc = ctx.FEECOLLECTMSTs
                    .Where(s => s.TRNNO == id)
                    .FirstOrDefault();
                ctx.Entry(fc).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }
            return Ok();
        }
    }
}
