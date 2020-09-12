using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity.Core.Objects;
using EMS.Models;

namespace EMS.Controllers
{
    [RoutePrefix("api")]
    public class SpFeeController : ApiController
    {
        [HttpGet]
        [Route("spfee")]
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
        [HttpGet]
        [Route("spfee/{id}")]
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
        [HttpPost]
        [Route("getspfee")]
        public IHttpActionResult PostNewSpFee(SpFeeViewModel spfee)
        {           
          
            ObjectParameter FEE1 = new System.Data.Entity.Core.Objects.ObjectParameter("FEE1", typeof(double));
            
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            using (var ctx = new EMSEntities())
            {
                                
               ctx.spStudentCurrentFee(spfee.SPFEEDTLs[0].TRNNO, FEE1, spfee.CLASS_TRNNO);
               var res= FEE1.Value;     

                if (res == null)
                {
                    return NotFound();
                }

                return Ok(res);
            }
        }
        [HttpPost]
        [Route("spfee")]
        public IHttpActionResult AddNewSpFee(SpFeeViewModel spfee)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");
            int _trnno;
            int _sr = 1;

            using (var ctx = new EMSEntities())
            {
                if (spfee.TRNNO == 0)
                {
                    _trnno = Convert.ToInt32(ctx.SPFEEMSTs.OrderByDescending(t => t.TRNNO).FirstOrDefault().TRNNO);
                    _trnno = _trnno + 1;
                }
                else
                {
                    _trnno = Convert.ToInt32(spfee.TRNNO) + 1;
                }
                spfee.TRNNO = _trnno;
                ctx.SPFEEMSTs.Add(new SPFEEMST()
                {
                    TRNNO = spfee.TRNNO,
                    CLASS_TRNNO = spfee.CLASS_TRNNO,
                    SPDATE = spfee.SPDATE,
                });
                foreach (var dtls in spfee.SPFEEDTLs)
                {                    
                    
                        var spdetail = new SPFEEDTL
                        {
                            TRNNO = _trnno,
                            SR = _sr,
                            EM_TRNNO = dtls.EM_TRNNO,
                            SPFEE=Convert.ToDouble(dtls.SPFEE)
                        };
                        ctx.SPFEEDTLs.Add(spdetail);
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
