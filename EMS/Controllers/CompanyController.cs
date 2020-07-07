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
    public class CompanyController : ApiController
    {
        public IHttpActionResult GetAllCompanies()
        {
            IList<CompanyViewModel> bps = null;

            using (var ctx = new EMSEntities())
            {
                bps = ctx.COMPANies
                            .Select(s => new CompanyViewModel()
                            {
                                TRNNO = s.TRNNO,
                                CNAME = s.CNAME,
                                PIC = s.PIC
                            }).ToList<CompanyViewModel>();
            }

            if (bps.Count == 0)
            {
                return NotFound();
            }

            return Ok(bps);
        }
        public IHttpActionResult GetCompanyById(int id)
        {
            CompanyViewModel bps = null;

            using (var ctx = new EMSEntities())
            {
                bps = ctx.COMPANies
                    .Where(s => s.TRNNO == id)
                    .Select(s => new CompanyViewModel()
                    {
                        TRNNO = s.TRNNO,
                        CNAME = s.CNAME,
                        PIC = s.PIC
                    }).FirstOrDefault<CompanyViewModel>();
            }

            if (bps == null)
            {
                return NotFound();
            }

            return Ok(bps);
        }

        public IHttpActionResult GetAllComps(string name)
        {
            IList<CompanyViewModel> bps = null;

            using (var ctx = new EMSEntities())
            {
                bps = ctx.COMPANies
                    .Where(s => s.CNAME.ToLower() == name.ToLower())
                    .Select(s => new CompanyViewModel()
                    {
                        TRNNO = s.TRNNO,
                        CNAME = s.CNAME,
                        PIC = s.PIC

                    }).ToList<CompanyViewModel>();
            }

            if (bps.Count == 0)
            {
                return NotFound();
            }

            return Ok(bps);
        }
        public IHttpActionResult PostNewBp(CompanyViewModel bp)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            using (var ctx = new EMSEntities())
            {
                int totalConunt = ctx.COMPANies.Count<COMPANY>();
                bp.TRNNO = totalConunt + 1;
                ctx.COMPANies.Add(new COMPANY()
                {
                    TRNNO = (bp.TRNNO),
                    CNAME = bp.CNAME,
                    PIC = bp.PIC
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
        public IHttpActionResult Put(CompanyViewModel bp)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            using (var ctx = new EMSEntities())
            {
                try
                {
                    var existingBp = ctx.COMPANies.Where(s => s.TRNNO == bp.TRNNO)
                                                        .FirstOrDefault<COMPANY>();

                    if (existingBp != null)
                    {
                        existingBp.CNAME = bp.CNAME;
                        existingBp.PIC = bp.PIC;

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
        public IHttpActionResult DeleteComp(int id)
        {
            if (id <= 0)
                return BadRequest("Not a valid Company id");

            using (var ctx = new EMSEntities())
            {
                var bp = ctx.COMPANies
                    .Where(s => s.TRNNO == id)
                    .FirstOrDefault();

                ctx.Entry(bp).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }

            return Ok();
        }
    }
}
