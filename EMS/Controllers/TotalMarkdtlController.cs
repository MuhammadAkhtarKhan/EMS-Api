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
    public class TotalMarkdtlController : ApiController
    {
        public IHttpActionResult GetAllTotalMarkDtl()
        {
            IList<TotalMarkDtlViewModel> tmdtls = null;

            using (var ctx = new EMSEntities())
            {
                tmdtls = ctx.MARKTOTALDTLs
                            .Select(s => new TotalMarkDtlViewModel()
                            {
                                TRNNO = s.TRNNO,
                                 SR = s.SR,
                                 SUBJECT_TRNNO = s.SUBJECT_TRNNO,
                                  TOTMARKS=s.TOTMARKS
                            }).ToList<TotalMarkDtlViewModel>();
            }

            if (tmdtls.Count == 0)
            {
                return NotFound();
            }

            return Ok(tmdtls);
        }
        public IHttpActionResult GetTotalMarkDtlById(int id)
        {
            //IList<TotalMarkDtlViewModel> tmdtls = null;
            IList<TotalMarkDtlSubjectViewModel> tmdtls = null;

            using (var ctx = new EMSEntities())
            {
                //tmdtls = ctx.MARKTOTALDTLs
                //    .Where(s => s.TRNNO == id)
                //    .Select(s => new TotalMarkDtlViewModel()
                //    {
                //        TRNNO = s.TRNNO,
                //        SR = s.SR,
                //        SUBJECT_TRNNO = s.SUBJECT_TRNNO,
                //        TOTMARKS = s.TOTMARKS
                //    }).ToList<TotalMarkDtlViewModel>();
                tmdtls = ctx.Database.SqlQuery<TotalMarkDtlSubjectViewModel>("SELECT s.TRNNO , s.SNAME, s.SCODE, mkdtl.TRNNO as MKTOTAL_TRNNO, mkdtl.SR, mkdtl.TOTMARKS FROM EMS.MARKTOTALDTL mkdtl INNER JOIN EMS.SUBJECT s ON s.TRNNO = mkdtl.SUBJECT_TRNNO WHERE mkdtl.TRNNO = @id", new SqlParameter("@id", id)).ToList<TotalMarkDtlSubjectViewModel>();
            }

            if (tmdtls == null)
            {
                return NotFound();
            }

            return Ok(tmdtls);
        }

        //public IHttpActionResult GetAllTotalMarkDtls(string name)
        //{
        //    IList<TotalMarkDtlViewModel> tmdtls = null;

        //    using (var ctx = new EMSEntities())
        //    {
        //        tmdtls = ctx.MARKTOTALDTLs
        //            .Where(s => s.SR.ToLower() == name.ToLower())
        //            .Select(s => new TotalMarkDtlViewModel()
        //            {
        //                TRNNO = s.TRNNO,
        //                SR = s.SR,
        //                SUBJECT_TRNNO = s.SUBJECT_TRNNO

        //            }).ToList<TotalMarkDtlViewModel>();
        //    }

        //    if (tmdtls.Count == 0)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(tmdtls);
        //}
        //public IHttpActionResult PostNewTotalMarkDtl(TotalMarkDtlViewModel bp)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest("Invalid data.");

        //    using (var ctx = new EMSEntities())
        //    {
        //        int totalConunt = ctx.BPs.Count<BP>();
        //        bp.TRNNO = totalConunt + 1;
        //        ctx.BPs.Add(new BP()
        //        {
        //            TRNNO = bp.TRNNO,
        //            SR = bp.SR,
        //            SUBJECT_TRNNO = bp.SUBJECT_TRNNO
        //        });
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
        public IHttpActionResult Put(TotalMarkDtlViewModel bp)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            using (var ctx = new EMSEntities())
            {
                try
                {
                    var existingTotalMarkDtl = ctx.MARKTOTALDTLs.Where(s => s.TRNNO == bp.TRNNO)
                                                        .FirstOrDefault<MARKTOTALDTL>();

                    if (existingTotalMarkDtl != null)
                    {
                        existingTotalMarkDtl.SR = bp.SR;
                        existingTotalMarkDtl.SUBJECT_TRNNO = bp.SUBJECT_TRNNO;
                        existingTotalMarkDtl.TOTMARKS = bp.TOTMARKS;

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
        public IHttpActionResult DeleteTotalMarkDtl(int id)
        {
            if (id <= 0)
                return BadRequest("Not a valid student id");

            using (var ctx = new EMSEntities())
            {
                var bp = ctx.BPs
                    .Where(s => s.TRNNO == id)
                    .FirstOrDefault();
                ctx.Entry(bp).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }

            return Ok();
        }
    }
}
