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
    public class FeeTypeController : ApiController
    {
        public IHttpActionResult GetAllFeeType()
        {
            IList<FeeTypeViewModel> ftp = null;

            using (var ctx = new EMSEntities())
            {
                ftp = ctx.FEETYPES
                            .Select(s => new FeeTypeViewModel()
                            {
                                TRNNO = s.TRNNO,
                                FTYPE = s.FTYPE,
                                STATUS = s.STATUS
                            }).ToList<FeeTypeViewModel>();
            }

            if (ftp.Count == 0)
            {
                return NotFound();
            }

            return Ok(ftp);
        }
        public IHttpActionResult GetFeeTypeById(int id)
        {
            FeeTypeViewModel ftp = null;

            using (var ctx = new EMSEntities())
            {
                ftp = ctx.FEETYPES
                    .Where(s => s.TRNNO == id)
                    .Select(s => new FeeTypeViewModel()
                    {
                        TRNNO = s.TRNNO,
                        FTYPE = s.FTYPE,
                        STATUS = s.STATUS
                    }).FirstOrDefault<FeeTypeViewModel>();
            }

            if (ftp == null)
            {
                return NotFound();
            }

            return Ok(ftp);
        }

        public IHttpActionResult GetAllFeeTypes(string name)
        {
            IList<FeeTypeViewModel> ftp = null;

            using (var ctx = new EMSEntities())
            {
                ftp = ctx.FEETYPES
                    .Where(s => s.FTYPE.ToLower() == name.ToLower())
                    .Select(s => new FeeTypeViewModel()
                    {
                        TRNNO = s.TRNNO,
                        FTYPE = s.FTYPE,
                        STATUS = s.STATUS

                    }).ToList<FeeTypeViewModel>();
            }

            if (ftp.Count == 0)
            {
                return NotFound();
            }

            return Ok(ftp);
        }
        public IHttpActionResult PostNewFeeType(FeeTypeViewModel bp)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            using (var ctx = new EMSEntities())
            {
                int totalConunt = ctx.FEETYPES.Count<FEETYPE>();
                bp.TRNNO = totalConunt + 1;
                ctx.FEETYPES.Add(new FEETYPE()
                {
                    TRNNO = (bp.TRNNO),
                    FTYPE = bp.FTYPE,
                    STATUS = bp.STATUS
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
        public IHttpActionResult Put(FeeTypeViewModel bp)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            using (var ctx = new EMSEntities())
            {
                try
                {
                    var existingFeeType = ctx.FEETYPES.Where(s => s.TRNNO == bp.TRNNO)
                                                        .FirstOrDefault<FEETYPE>();

                    if (existingFeeType != null)
                    {
                        existingFeeType.FTYPE = bp.FTYPE;
                        existingFeeType.STATUS = bp.STATUS;

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
        public IHttpActionResult DeleteFeeType(int id)
        {
            if (id <= 0)
                return BadRequest("Not a valid student id");

            using (var ctx = new EMSEntities())
            {
                var bp = ctx.FEETYPES
                    .Where(s => s.TRNNO == id)
                    .FirstOrDefault();
                ctx.Entry(bp).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }

            return Ok();
        }
    }
}
