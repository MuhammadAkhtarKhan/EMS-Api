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
    public class SubjectController : ApiController
    {
        public IHttpActionResult GetAllSubject()
        {
            IList<SubjectViewModel> sbs = null;

            using (var ctx = new EMSEntities())
            {
                sbs = ctx.SUBJECTs
                            .Select(s => new SubjectViewModel()
                            {
                                TRNNO = s.TRNNO,
                                SNAME = s.SNAME,
                                SCODE = s.SCODE
                            }).ToList<SubjectViewModel>();
            }

            if (sbs.Count == 0)
            {
                return NotFound();
            }

            return Ok(sbs);
        }
        public IHttpActionResult GetSubjectById(int id)
        {
            SubjectViewModel sbs = null;

            using (var ctx = new EMSEntities())
            {
                sbs = ctx.SUBJECTs
                    .Where(s => s.TRNNO == id)
                    .Select(s => new SubjectViewModel()
                    {
                        TRNNO = s.TRNNO,
                        SNAME = s.SNAME,
                        SCODE = s.SCODE
                    }).FirstOrDefault<SubjectViewModel>();
            }

            if (sbs == null)
            {
                return NotFound();
            }

            return Ok(sbs);
        }

        public IHttpActionResult GetAllSubjects(string name)
        {
            IList<SubjectViewModel> sbs = null;

            using (var ctx = new EMSEntities())
            {
                sbs = ctx.SUBJECTs
                    .Where(s => s.SNAME.ToLower() == name.ToLower())
                    .Select(s => new SubjectViewModel()
                    {
                        TRNNO = s.TRNNO,
                        SNAME = s.SNAME,
                        SCODE = s.SCODE

                    }).ToList<SubjectViewModel>();
            }

            if (sbs.Count == 0)
            {
                return NotFound();
            }

            return Ok(sbs);
        }
        public IHttpActionResult PostNewSubject(SubjectViewModel sb)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            using (var ctx = new EMSEntities())
            {
                int totalConunt = ctx.SUBJECTs.Count<SUBJECT>();
                sb.TRNNO = totalConunt + 1;
                ctx.SUBJECTs.Add(new SUBJECT()
                {
                    TRNNO = sb.TRNNO,
                    SNAME = sb.SNAME,
                    SCODE = sb.SCODE
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
        public IHttpActionResult Put(SubjectViewModel sb)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            using (var ctx = new EMSEntities())
            {
                try
                {
                    var existingSubject = ctx.SUBJECTs.Where(s => s.TRNNO == sb.TRNNO)
                                                        .FirstOrDefault<SUBJECT>();

                    if (existingSubject != null)
                    {
                        existingSubject.SNAME = sb.SNAME;
                        existingSubject.SCODE = sb.SCODE;

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
        public IHttpActionResult DeleteSubject(int id)
        {
            if (id <= 0)
                return BadRequest("Not a valid student id");

            using (var ctx = new EMSEntities())
            {
                var sb = ctx.SUBJECTs
                    .Where(s => s.TRNNO == id)
                    .FirstOrDefault();
                ctx.Entry(sb).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }

            return Ok();
        }

    }
}
