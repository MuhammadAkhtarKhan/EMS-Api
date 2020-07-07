using EMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace EMS.Controllers
{
    public class CastController : ApiController
    {
        public IHttpActionResult GetAllCasts()
        {
            IList<CastViewModel> cst = null;

            using (var ctx = new EMSEntities())
            {
                cst = ctx.CSTs
                            .Select(s => new CastViewModel()
                            {
                                TRNNO = s.TRNNO,
                                CDESC = s.CDESC,
                                STATUS = s.STATUS
                            }).ToList<CastViewModel>();
            }

            if (cst.Count == 0)
            {
                return NotFound();
            }

            return Ok(cst);
        }
        public IHttpActionResult GetCastById(int id)
        {
            CastViewModel cst = null;

            using (var ctx = new EMSEntities())
            {
                cst = ctx.CSTs
                    .Where(s => s.TRNNO == id)
                    .Select(s => new CastViewModel()
                    {
                        TRNNO = s.TRNNO,
                        CDESC = s.CDESC,
                        STATUS = s.STATUS
                    }).FirstOrDefault<CastViewModel>();

            }

            if (cst == null)
            {
                return NotFound();
            }

            return Ok(cst);
        }

        public IHttpActionResult GetAllCasts(string name)
        {
            IList<CastViewModel> bps = null;

            using (var ctx = new EMSEntities())
            {
                bps = ctx.CSTs
                    .Where(s => s.CDESC.ToLower() == name.ToLower())
                    .Select(s => new CastViewModel()
                    {
                        TRNNO = s.TRNNO,
                        CDESC = s.CDESC,
                        STATUS = s.STATUS

                    }).ToList<CastViewModel>();
            }

            if (bps.Count == 0)
            {
                return NotFound();
            }

            return Ok(bps);
        }
        public IHttpActionResult PostNewCast(CastViewModel cst)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            using (var ctx = new EMSEntities())
            {
                int totalConunt = ctx.CSTs.Count<CST>();
                cst.TRNNO = totalConunt + 1;
                ctx.CSTs.Add(new CST()
                {
                    TRNNO = (cst.TRNNO),
                    CDESC = cst.CDESC,
                    STATUS = cst.STATUS
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
        public IHttpActionResult Put(CastViewModel bp)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            using (var ctx = new EMSEntities())
            {
                try
                {
                    var existingBp = ctx.CSTs.Where(s => s.TRNNO == bp.TRNNO)
                                                        .FirstOrDefault<CST>();

                    if (existingBp != null)
                    {
                        existingBp.CDESC = bp.CDESC;
                        existingBp.STATUS = bp.STATUS;

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
        public IHttpActionResult DeleteBp(int id)
        {
            if (id <= 0)
                return BadRequest("Not a valid student id");

            using (var ctx = new EMSEntities())
            {
                var bp = ctx.CSTs
                    .Where(s => s.TRNNO == id)
                    .FirstOrDefault();

                ctx.Entry(bp).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }

            return Ok();
        }

    }
}
