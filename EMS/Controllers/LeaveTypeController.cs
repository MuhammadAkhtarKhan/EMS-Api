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
    public class LeaveTypeController : ApiController
    {
        public IHttpActionResult GetAllLeaveType()
        {
            IList<LeaveTypeViewModel> ltps = null;

            using (var ctx = new EMSEntities())
            {
                ltps = ctx.LEAVETYPEs
                            .Select(s => new LeaveTypeViewModel()
                            {
                                TRNNO = s.TRNNO,
                                LDESC = s.LDESC,
                                LABRV=s.LABRV,
                                LWEIGHT=s.LWEIGHT,
                                STATUS = s.STATUS
                            }).ToList<LeaveTypeViewModel>();
            }

            if (ltps.Count == 0)
            {
                return NotFound();
            }

            return Ok(ltps);
        }
        public IHttpActionResult GetLeaveTypeById(int id)
        {
            LeaveTypeViewModel ltps = null;

            using (var ctx = new EMSEntities())
            {
                ltps = ctx.LEAVETYPEs
                    .Where(s => s.TRNNO == id)
                    .Select(s => new LeaveTypeViewModel()
                    {
                        TRNNO = s.TRNNO,
                        LDESC = s.LDESC,
                        LABRV = s.LABRV,
                        LWEIGHT = s.LWEIGHT,
                        STATUS = s.STATUS
                    }).FirstOrDefault<LeaveTypeViewModel>();
            }

            if (ltps == null)
            {
                return NotFound();
            }

            return Ok(ltps);
        }

        public IHttpActionResult GetAllLeaveTypes(string name)
        {
            IList<LeaveTypeViewModel> ltps = null;

            using (var ctx = new EMSEntities())
            {
                ltps = ctx.LEAVETYPEs
                    .Where(s => s.LDESC.ToLower() == name.ToLower())
                    .Select(s => new LeaveTypeViewModel()
                    {
                        TRNNO = s.TRNNO,
                        LDESC = s.LDESC,
                        LABRV = s.LABRV,
                        LWEIGHT = s.LWEIGHT,
                        STATUS = s.STATUS

                    }).ToList<LeaveTypeViewModel>();
            }

            if (ltps.Count == 0)
            {
                return NotFound();
            }

            return Ok(ltps);
        }
        public IHttpActionResult PostNewLeaveType(LeaveTypeViewModel ltp)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            using (var ctx = new EMSEntities())
            {
                int totalConunt = ctx.LEAVETYPEs.Count<LEAVETYPE>();
                ltp.TRNNO = totalConunt + 1;
                ctx.LEAVETYPEs.Add(new LEAVETYPE()
                {
                    TRNNO = (ltp.TRNNO),
                    LDESC = ltp.LDESC,
                    LABRV = ltp.LABRV,
                    LWEIGHT = ltp.LWEIGHT,
                    STATUS = ltp.STATUS
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
        public IHttpActionResult Put(LeaveTypeViewModel ltp)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            using (var ctx = new EMSEntities())
            {
                try
                {
                    var existingLeaveType = ctx.LEAVETYPEs.Where(s => s.TRNNO == ltp.TRNNO)
                                                        .FirstOrDefault<LEAVETYPE>();

                    if (existingLeaveType != null)
                    {
                        existingLeaveType.LDESC = ltp.LDESC;
                        existingLeaveType.STATUS = ltp.STATUS;
                        existingLeaveType.LABRV = ltp.LABRV;
                        existingLeaveType.LWEIGHT = ltp.LWEIGHT;

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
        public IHttpActionResult DeleteLeaveType(int id)
        {
            if (id <= 0)
                return BadRequest("Not a valid student id");

            using (var ctx = new EMSEntities())
            {
                var ltp = ctx.LEAVETYPEs
                    .Where(s => s.TRNNO == id)
                    .FirstOrDefault();
                ctx.Entry(ltp).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }

            return Ok();
        }
    }
}
