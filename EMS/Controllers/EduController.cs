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
    public class EduController : ApiController
    {
        public IHttpActionResult GetAllEdus()
        {
            IList<EduViewModel> edus = null;

            using (var ctx = new EMSEntities())
            {
                edus = ctx.EDUCATIONs
                            .Select(s => new EduViewModel()
                            {
                                TRNNO = s.TRNNO,
                                DNAME = s.DNAME,
                                DABRV=s.DABRV,
                                STATUS = s.STATUS
                            }).ToList<EduViewModel>();
            }

            if (edus.Count == 0)
            {
                return NotFound();
            }

            return Ok(edus);
        }
        public IHttpActionResult GetEduById(int id)
        {
            EduViewModel edus = null;

            using (var ctx = new EMSEntities())
            {
                edus = ctx.EDUCATIONs
                    .Where(s => s.TRNNO == id)
                    .Select(s => new EduViewModel()
                    {
                        TRNNO = s.TRNNO,
                        DNAME = s.DNAME,
                        DABRV = s.DABRV,
                        STATUS = s.STATUS
                    }).FirstOrDefault<EduViewModel>();
            }

            if (edus == null)
            {
                return NotFound();
            }

            return Ok(edus);
        }

        public IHttpActionResult GetAllEdus(string name)
        {
            IList<EduViewModel> edus = null;

            using (var ctx = new EMSEntities())
            {
                edus = ctx.EDUCATIONs
                    .Where(s => s.DNAME.ToLower() == name.ToLower())
                    .Select(s => new EduViewModel()
                    {
                        TRNNO = s.TRNNO,
                        DNAME = s.DNAME,
                        DABRV = s.DABRV,
                        STATUS = s.STATUS

                    }).ToList<EduViewModel>();
            }

            if (edus.Count == 0)
            {
                return NotFound();
            }

            return Ok(edus);
        }
        public IHttpActionResult PostNewEdu(EduViewModel edu)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            using (var ctx = new EMSEntities())
            {
                int totalConunt = ctx.EDUCATIONs.Count<EDUCATION>();
                edu.TRNNO = totalConunt + 1;
                ctx.EDUCATIONs.Add(new EDUCATION()
                {
                    TRNNO = (edu.TRNNO),
                    DNAME = edu.DNAME,
                    DABRV = edu.DABRV,
                    STATUS = edu.STATUS
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
        public IHttpActionResult Put(EduViewModel edu)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            using (var ctx = new EMSEntities())
            {
                try
                {
                    var existingEdu = ctx.EDUCATIONs.Where(s => s.TRNNO == edu.TRNNO)
                                                        .FirstOrDefault<EDUCATION>();

                    if (existingEdu != null)
                    {
                        existingEdu.DNAME = edu.DNAME;
                        existingEdu.DABRV = edu.DABRV;
                        existingEdu.STATUS = edu.STATUS;
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
        public IHttpActionResult DeleteEdu(int id)
        {
            if (id <= 0)
                return BadRequest("Not a valid student id");

            using (var ctx = new EMSEntities())
            {
                var edu = ctx.EDUCATIONs
                    .Where(s => s.TRNNO == id)
                    .FirstOrDefault();
                ctx.Entry(edu).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }

            return Ok();
        }
    }
}