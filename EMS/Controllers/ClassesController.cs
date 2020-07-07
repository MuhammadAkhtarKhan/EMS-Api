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
    public class ClassesController : ApiController
    {
        public IHttpActionResult GetAllClasses()
        {
            IList<ClassesViewModel> cls = null;

            using (var ctx = new EMSEntities())
            {
                cls = ctx.CLASSes
                            .Select(s => new ClassesViewModel()
                            {
                                TRNNO = s.TRNNO,
                                CNAME = s.CNAME,
                                CABR = s.CABR
                            }).ToList<ClassesViewModel>();
            }

            if (cls.Count == 0)
            {
                return NotFound();
            }

            return Ok(cls);
        }

        public IHttpActionResult GetClassById(int id)
        {
            ClassesViewModel cls = null;

            using (var ctx = new EMSEntities())
            {
                cls = ctx.CLASSes
                    .Where(s => s.TRNNO == id)
                    .Select(s => new ClassesViewModel()
                    {
                        TRNNO = s.TRNNO,
                        CNAME = s.CNAME,
                        CABR = s.CABR
                    }).FirstOrDefault<ClassesViewModel>();
            }

            if (cls == null)
            {
                return NotFound();
            }

            return Ok(cls);
        }


        public IHttpActionResult PostNewBp(ClassesViewModel cls)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            using (var ctx = new EMSEntities())
            {
                int totalConunt = ctx.CLASSes.Count<CLASS>();
                cls.TRNNO = totalConunt + 1;
                ctx.CLASSes.Add(new CLASS()
                {
                    TRNNO = (cls.TRNNO),
                    CNAME = cls.CNAME,
                    CABR = cls.CABR
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
        public IHttpActionResult Put(ClassesViewModel cls)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            using (var ctx = new EMSEntities())
            {
                try
                {
                    var existClass = ctx.CLASSes.Where(s => s.TRNNO == cls.TRNNO)
                                                        .FirstOrDefault<CLASS>();

                    if (existClass != null)
                    {
                        existClass.CNAME = cls.CNAME;
                        existClass.CABR = cls.CABR;

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
        public IHttpActionResult DeleteClass(int id)
        {
            if (id <= 0)
                return BadRequest("Not a valid student id");

            using (var ctx = new EMSEntities())
            {
                var cls = ctx.CLASSes
                    .Where(s => s.TRNNO == id)
                    .FirstOrDefault();

                ctx.Entry(cls).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }

            return Ok();
        }

        public IHttpActionResult GetClassByName(string name)
        {
            IList<ClassesViewModel> bps = null;

            using (var ctx = new EMSEntities())
            {
                bps = ctx.CLASSes
                    .Where(s => s.CNAME.ToLower() == name.ToLower())
                    .Select(s => new ClassesViewModel()
                    {
                        TRNNO = s.TRNNO,
                        CNAME = s.CNAME,
                        CABR = s.CABR

                    }).ToList<ClassesViewModel>();
            }

            if (bps.Count == 0)
            {
                return NotFound();
            }

            return Ok(bps);
        }

    }
}
