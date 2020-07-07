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
    public class SpFeeController : ApiController
    {
        public IHttpActionResult GetAllSpFee()
        {
            IList<SpFeeViewModel> bps = null;

            using (var ctx = new EMSEntities())
            {
                bps = ctx.SPFEEMSTs
                            .Select(s => new SpFeeViewModel()
                            {
                                TRNNO = s.TRNNO,
                                CLASS_TRNNO = s.CLASS_TRNNO,
                                SPDATE=s.SPDATE
                            }).ToList<SpFeeViewModel>();
            }

            if (bps.Count == 0)
            {
                return NotFound();
            }

            return Ok(bps);
        }
        public IHttpActionResult GetSpFeeClassId(int id)
        {
           IList <SpFeeUpdateViewModel> bps = null;

            using (var ctx = new EMSEntities())
            {
                bps = ctx.Database.SqlQuery<SpFeeUpdateViewModel>("SELECT   e.TRNNO, E.EMP_NAME,E.EMP_F_NAME, s.SPFEE, sp.CLASS_TRNNO, sp.SPDATE from ems.EMS.EM  e inner join[EMS].[EMS].[SPFEEDTL] s on s.EM_TRNNO = e.TRNNO inner join EMS.SPFEEMST sp on s.TRNNO = sp.TRNNO where sp.CLASS_TRNNO = @id and sp.SPDATE = (SELECT MAX(SPDATE) FROM EMS.SPFEEMST where CLASS_TRNNO = @cid)", new SqlParameter("@id", id), new SqlParameter("@cid", id)).ToList<SpFeeUpdateViewModel>();
            }

            if (bps == null)
            {
                return NotFound();
            }

            return Ok(bps);
        }

        //public IHttpActionResult GetAllSpFees(string name)
        //{
        //    IList<SpFeeViewModel> bps = null;

        //    using (var ctx = new EMSEntities())
        //    {
        //        bps = ctx.SPFEEMSTs
        //            .Where(s => s.CLASS_TRNNO.ToLower() == name.ToLower())
        //            .Select(s => new SpFeeViewModel()
        //            {
        //                TRNNO = s.TRNNO,
        //                CLASS_TRNNO = s.CLASS_TRNNO,
        //                SPDATE = s.SPDATE

        //            }).ToList<SpFeeViewModel>();
        //    }

        //    if (bps.Count == 0)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(bps);
        //}
        public IHttpActionResult PostNewSpFee(SpFeeViewModel spfee)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            using (var ctx = new EMSEntities())
            {
                //int totalConunt = ctx.SPFEEMSTs.Count<SPFEEMST>();
                //spfee.TRNNO = totalConunt + 1;
                ctx.SPFEEMSTs.Add(new SPFEEMST()
                {
                    TRNNO = spfee.TRNNO,
                    CLASS_TRNNO = spfee.CLASS_TRNNO,
                    SPDATE = spfee.SPDATE
                });
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
        public IHttpActionResult Put(SpFeeViewModel spfee)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            using (var ctx = new EMSEntities())
            {
                try
                {
                    var existingSpFee = ctx.SPFEEMSTs.Where(s => s.TRNNO == spfee.TRNNO)
                                                        .FirstOrDefault<SPFEEMST>();

                    if (existingSpFee != null)
                    {
                        existingSpFee.CLASS_TRNNO = spfee.CLASS_TRNNO;
                        existingSpFee.SPDATE = spfee.SPDATE;

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
        public IHttpActionResult DeleteSpFee(int id)
        {
            if (id <= 0)
                return BadRequest("Not a valid student id");

            using (var ctx = new EMSEntities())
            {
                var spfee = ctx.SPFEEMSTs
                    .Where(s => s.TRNNO == id)
                    .FirstOrDefault();
                ctx.Entry(spfee).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }

            return Ok();
        }
    }
}
