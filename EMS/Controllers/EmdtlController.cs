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
    public class EmdtlController : ApiController
    {
        public IHttpActionResult GetAllEmdtl()
        {
            IList<EMDTLViewModel> emdtls = null;

            using (var ctx = new EMSEntities())
            {
                emdtls = ctx.EMDTLs
                            .Select(s => new EMDTLViewModel()
                            {
                                TRNNO = s.TRNNO,
                                SR = s.SR,
                                ADDDESC = s.ADDDESC
                            }).ToList<EMDTLViewModel>();
            }

            if (emdtls.Count == 0)
            {
                return NotFound();
            }

            return Ok(emdtls);
        }
        public IHttpActionResult GetEmdtlById(int id)
        {
            EMDTLViewModel emdtls = null;

            using (var ctx = new EMSEntities())
            {
                emdtls = ctx.EMDTLs
                    .Where(s => s.TRNNO == id)
                    .Select(s => new EMDTLViewModel()
                    {
                        TRNNO = s.TRNNO,
                        SR = s.SR,
                        ADDDESC=s.ADDDESC,
                        ADDTYPE = s.ADDTYPE
                    }).FirstOrDefault<EMDTLViewModel>();
            }

            if (emdtls == null)
            {
                return NotFound();
            }

            return Ok(emdtls);
        }

        public IHttpActionResult GetAllEmdtls(string name)
        {
            IList<EMDTLViewModel> emdtls = null;

            using (var ctx = new EMSEntities())
            {
                emdtls = ctx.EMDTLs
                    .Where(s => s.ADDDESC.ToLower() == name.ToLower())
                    .Select(s => new EMDTLViewModel()
                    {
                        TRNNO = s.TRNNO,
                        SR = s.SR,
                        ADDDESC=s.ADDDESC,
                        ADDTYPE = s.ADDTYPE

                    }).ToList<EMDTLViewModel>();
            }

            if (emdtls.Count == 0)
            {
                return NotFound();
            }

            return Ok(emdtls);
        }
        public IHttpActionResult PostNewEmdtl(EMDTLViewModel bp)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            using (var ctx = new EMSEntities())
            {
                int totalConunt = ctx.EMDTLs.Count<EMDTL>();
                bp.TRNNO = totalConunt + 1;
                ctx.EMDTLs.Add(new EMDTL()
                {
                    TRNNO = bp.TRNNO,
                    SR = bp.SR,
                    ADDDESC=bp.ADDDESC,
                    ADDTYPE = bp.ADDTYPE
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
        public IHttpActionResult Put(EMDTLViewModel bp)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            using (var ctx = new EMSEntities())
            {
                try
                {
                    var existingBp = ctx.EMDTLs.Where(s => s.TRNNO == bp.TRNNO)
                                                        .FirstOrDefault<EMDTL>();

                    if (existingBp != null)
                    {
                        existingBp.SR = bp.SR;
                        existingBp.ADDTYPE = bp.ADDTYPE;

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
                var bp = ctx.EMDTLs
                    .Where(s => s.TRNNO == id)
                    .FirstOrDefault();
                ctx.Entry(bp).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }

            return Ok();
        }
    }
}
