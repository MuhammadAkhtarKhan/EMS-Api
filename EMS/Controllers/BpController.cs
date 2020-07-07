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
    [Authorize]
    public class BpController : ApiController
    {
        [HttpGet]
        [Route("api/bp")]
        public IHttpActionResult GetAllBp()
        {
            IList<BpViewModel> bps = null;

            using (var ctx = new EMSEntities())
            {
                bps = ctx.BPs
                            .Select(s => new BpViewModel()
                            {
                                TRNNO=s.TRNNO,
                                BPNAME = s.BPNAME,
                                STATUS = s.STATUS
                            }).ToList<BpViewModel>();
            }

            if (bps.Count == 0)
            {
                return NotFound();
            }

            return Ok(bps);
        }
        public IHttpActionResult GetBpById(int id)
        {
            BpViewModel  bps = null;

            using (var ctx = new EMSEntities())
            {
                bps = ctx.BPs
                    .Where(s => s.TRNNO == id)
                    .Select(s => new BpViewModel()
                    {
                        TRNNO = s.TRNNO,
                        BPNAME = s.BPNAME,
                        STATUS = s.STATUS
                    }).FirstOrDefault<BpViewModel>();
            }

            if (bps == null)
            {
                return NotFound();
            }

            return Ok(bps);
        }

        public IHttpActionResult GetAllBps(string name)
        {
            IList<BpViewModel> bps = null;

            using (var ctx = new EMSEntities())
            {
                bps = ctx.BPs
                    .Where(s => s.BPNAME.ToLower() == name.ToLower())
                    .Select(s => new BpViewModel()
                    {
                        TRNNO = s.TRNNO,
                        BPNAME = s.BPNAME,
                        STATUS = s.STATUS
                        
                    }).ToList<BpViewModel>();
            }

            if (bps.Count == 0)
            {
                return NotFound();
            }

            return Ok(bps);
        }
        public IHttpActionResult PostNewBp(BpViewModel bp)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            using (var ctx = new EMSEntities())
            {
                int _trnno;
                //int _sr = 1;
                //db.Users.OrderByDescending(u => u.UserId).FirstOrDefault();
                if (bp.TRNNO == 0)
                {
                    _trnno = Convert.ToInt32(ctx.BPs.OrderByDescending(t => t.TRNNO).FirstOrDefault().TRNNO);
                    _trnno = _trnno + 1;
                    //_trnno = Convert.ToInt32(ctx.EMs.OrderByDescending(t => t.TRNNO).First().ToString());
                }
                else
                {
                    _trnno = Convert.ToInt32(bp.TRNNO) + 1;
                }
               // int totalConunt= ctx.BPs.Count<BP>();
                bp.TRNNO = _trnno;
                ctx.BPs.Add(new BP()
                {
                    TRNNO = bp.TRNNO,
                    BPNAME = bp.BPNAME,
                    STATUS = bp.STATUS
                });
                try { 
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
        public IHttpActionResult Put(BpViewModel bp)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            using (var ctx = new EMSEntities())
            {
                try
                {
                    var existingBp = ctx.BPs.Where(s => s.TRNNO == bp.TRNNO)
                                                        .FirstOrDefault<BP>();

                    if (existingBp != null)
                    {
                        existingBp.BPNAME = bp.BPNAME;
                        existingBp.STATUS = bp.STATUS;

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
                var bp = ctx.BPs
                    .Where(s => s.TRNNO == id)
                    .FirstOrDefault();
                ctx.Entry(bp).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }

            return Ok();
        }
    }
}
