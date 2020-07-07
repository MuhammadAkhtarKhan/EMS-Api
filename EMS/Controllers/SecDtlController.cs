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
    public class SecDtlController : ApiController
    {
        public IHttpActionResult GetAllSecDtl()
        {
            IList<SecDtlViewModel> SecDtls = null;

            using (var ctx = new EMSEntities())
            {
                SecDtls = ctx.SECDTLs
                            .Select(s => new SecDtlViewModel()
                            {
                                TRNNO = s.TRNNO,
                                SNAME = s.SNAME,
                                SR=s.SR,
                                STATUS = s.STATUS
                            }).ToList<SecDtlViewModel>();
            }

            if (SecDtls.Count == 0)
            {
                return NotFound();
            }

            return Ok(SecDtls);
        }
        public IHttpActionResult GetSecDtlById(int id)
        {
            SecDtlViewModel SecDtls = null;

            using (var ctx = new EMSEntities())
            {
                SecDtls = ctx.SECDTLs
                    .Where(s => s.TRNNO == id)
                    .Select(s => new SecDtlViewModel()
                    {
                        TRNNO = s.TRNNO,
                        SNAME = s.SNAME,
                        SR=s.SR,
                        STATUS = s.STATUS
                    }).FirstOrDefault<SecDtlViewModel>();
            }

            if (SecDtls == null)
            {
                return NotFound();
            }

            return Ok(SecDtls);
        }

        public IHttpActionResult GetAllSecDtls(string name)
        {
            IList<SecDtlViewModel> SecDtls = null;

            using (var ctx = new EMSEntities())
            {
                SecDtls = ctx.SECDTLs
                    .Where(s => s.SNAME.ToLower() == name.ToLower())
                    .Select(s => new SecDtlViewModel()
                    {
                        TRNNO = s.TRNNO,
                        SNAME = s.SNAME,
                        SR=s.SR,
                        STATUS = s.STATUS

                    }).ToList<SecDtlViewModel>();
            }

            if (SecDtls.Count == 0)
            {
                return NotFound();
            }

            return Ok(SecDtls);
        }
        public IHttpActionResult PostNewSecDtl(SecDtlViewModel SecDtl)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            using (var ctx = new EMSEntities())
            {
                int totalConunt = ctx.SECDTLs.Count<SECDTL>();
                SecDtl.TRNNO = totalConunt + 1;
                ctx.SECDTLs.Add(new SECDTL()
                {
                    TRNNO = SecDtl.TRNNO,
                    SNAME = SecDtl.SNAME,
                    SR=SecDtl.SR,
                    STATUS = SecDtl.STATUS
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
        public IHttpActionResult Put(SecDtlViewModel SecDtl)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            using (var ctx = new EMSEntities())
            {
                try
                {
                    var existingSecDtl = ctx.SECDTLs.Where(s => s.TRNNO == SecDtl.TRNNO)
                                                        .FirstOrDefault<SECDTL>();

                    if (existingSecDtl != null)
                    {
                        existingSecDtl.SNAME = SecDtl.SNAME;
                        existingSecDtl.STATUS = SecDtl.STATUS;

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
        public IHttpActionResult DeleteSecDtl(int id)
        {
            if (id <= 0)
                return BadRequest("Not a valid student id");

            using (var ctx = new EMSEntities())
            {
                var SecDtl = ctx.SECDTLs
                    .Where(s => s.TRNNO == id)
                    .FirstOrDefault();
                ctx.Entry(SecDtl).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }

            return Ok();
        }
    }
}
