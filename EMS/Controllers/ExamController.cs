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
    public class ExamController : ApiController
    {
        public IHttpActionResult GetAllBp()
        {
            IList<ExamViewModel> exms = null;

            using (var ctx = new EMSEntities())
            {
                exms = ctx.EXAMs
                            .Select(s => new ExamViewModel()
                            {
                                TRNNO = s.TRNNO,
                                ETYPE = s.ETYPE,
                                STATUS = s.STATUS
                            }).ToList<ExamViewModel>();
            }

            if (exms.Count == 0)
            {
                return NotFound();
            }

            return Ok(exms);
        }
        public IHttpActionResult GetExamById(int id)
        {
            ExamViewModel exms = null;

            using (var ctx = new EMSEntities())
            {
                exms = ctx.EXAMs
                    .Where(s => s.TRNNO == id)
                    .Select(s => new ExamViewModel()
                    {
                        TRNNO = s.TRNNO,
                        ETYPE = s.ETYPE,
                        STATUS = s.STATUS
                    }).FirstOrDefault<ExamViewModel>();
            }

            if (exms == null)
            {
                return NotFound();
            }

            return Ok(exms);
        }

        public IHttpActionResult GetAllExams(string name)
        {
            IList<ExamViewModel> exms = null;

            using (var ctx = new EMSEntities())
            {
                exms = ctx.EXAMs
                    .Where(s => s.ETYPE.ToLower() == name.ToLower())
                    .Select(s => new ExamViewModel()
                    {
                        TRNNO = s.TRNNO,
                        ETYPE = s.ETYPE,
                        STATUS = s.STATUS

                    }).ToList<ExamViewModel>();
            }

            if (exms.Count == 0)
            {
                return NotFound();
            }

            return Ok(exms);
        }
        public IHttpActionResult PostNewBp(ExamViewModel exam)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            using (var ctx = new EMSEntities())
            {
                int totalConunt = ctx.EXAMs.Count<EXAM>();
                exam.TRNNO = totalConunt + 1;
                ctx.EXAMs.Add(new EXAM()
                {
                    TRNNO = (exam.TRNNO),
                    ETYPE = exam.ETYPE,
                    STATUS = exam.STATUS
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
        public IHttpActionResult Put(ExamViewModel ex)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            using (var ctx = new EMSEntities())
            {
                try
                {
                    var existingExm = ctx.EXAMs.Where(s => s.TRNNO == ex.TRNNO)
                                                        .FirstOrDefault<EXAM>();

                    if (existingExm != null)
                    {
                        existingExm.ETYPE = ex.ETYPE;
                        existingExm.STATUS = ex.STATUS;

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
        public IHttpActionResult DeleteExam(int id)
        {
            if (id <= 0)
                return BadRequest("Not a valid student id");

            using (var ctx = new EMSEntities())
            {
                var ex = ctx.EXAMs
                    .Where(s => s.TRNNO == id)
                    .FirstOrDefault();

                ctx.Entry(ex).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }

            return Ok();
        }
    }
}
