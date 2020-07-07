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
    public class DeptController : ApiController
    {
        public IHttpActionResult GetAllDepts()
        {
            IList<DeptViewModel> depts = null;

            using (var ctx = new EMSEntities())
            {
                depts = ctx.DEPTs
                            .Select(s => new DeptViewModel()
                            {
                                TRNNO = s.TRNNO,
                                 ABR = s.ABR,
                                DESCRIP = s.DESCRIP,
                                STATUS=s.STATUS
                            }).ToList<DeptViewModel>();
            }

            if (depts.Count == 0)
            {
                return NotFound();
            }

            return Ok(depts);
        }
        public IHttpActionResult GetDeptById(int id)
        {
            DeptViewModel depts = null;

            using (var ctx = new EMSEntities())
            {
                depts = ctx.DEPTs
                    .Where(s => s.TRNNO == id)
                    .Select(s => new DeptViewModel()
                    {
                        TRNNO = s.TRNNO,
                        ABR = s.ABR,
                        DESCRIP = s.DESCRIP,
                        STATUS = s.STATUS
                    }).FirstOrDefault<DeptViewModel>();
            }

            if (depts == null)
            {
                return NotFound();
            }

            return Ok(depts);
        }

        public IHttpActionResult GetAllBps(string name)
        {
            IList<DeptViewModel> depts = null;

            using (var ctx = new EMSEntities())
            {
                depts = ctx.DEPTs
                    .Where(s => s.ABR .ToLower() == name.ToLower())
                    .Select(s => new DeptViewModel()
                    {
                        TRNNO = s.TRNNO,
                        ABR = s.ABR,
                        DESCRIP = s.DESCRIP,
                        STATUS = s.STATUS

                    }).ToList<DeptViewModel>();
            }

            if (depts.Count == 0)
            {
                return NotFound();
            }

            return Ok(depts);
        }
        public IHttpActionResult PostNewBp(DeptViewModel dept)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            using (var ctx = new EMSEntities())
            {
                int totalConunt = ctx.DEPTs.Count<DEPT>();
                dept.TRNNO = totalConunt + 1;
                ctx.DEPTs.Add(new DEPT()
                {
                    TRNNO = (dept.TRNNO),
                    ABR  = dept.ABR ,
                    DESCRIP=dept.DESCRIP,
                    STATUS = dept.STATUS
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
        public IHttpActionResult Put(DeptViewModel dept)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            using (var ctx = new EMSEntities())
            {
                try
                {
                    var existingDept = ctx.DEPTs.Where(s => s.TRNNO == dept.TRNNO)
                                                        .FirstOrDefault<DEPT>();

                    if (existingDept != null)
                    {
                        existingDept.ABR  = dept.ABR ;                        
                        existingDept.DESCRIP = dept.DESCRIP;
                        existingDept.STATUS = dept.STATUS;


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
        public IHttpActionResult DeleteDept(int id)
        {
            if (id <= 0)
                return BadRequest("Not a valid Department id");

            using (var ctx = new EMSEntities())
            {
                var dept = ctx.DEPTs
                    .Where(s => s.TRNNO == id)
                    .FirstOrDefault();

                ctx.Entry(dept).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }

            return Ok();
        }
    }
}
