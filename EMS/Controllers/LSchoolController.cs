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
    [RoutePrefix("api")]
    public class LSchoolController : ApiController
    {
        [HttpGet]
        [Route("lschool")]
        public IHttpActionResult GetAllLs()
        {
            IList<LSchoolViewModel> lss = null;

            using (var ctx = new EMSEntities())
            {
                lss = ctx.LSCHOOLMSTs
                            .Select(s => new LSchoolViewModel()
                            {
                                TRNNO = s.TRNNO,
                                CLASS_TRNNO = s.CLASS_TRNNO,
                                LDATE = s.LDATE
                            }).ToList<LSchoolViewModel>();
            }

            if (lss.Count == 0)
            {
                return NotFound();
            }

            return Ok(lss);
        }
        public IHttpActionResult GetLsById(int id)
        {
            LSchoolViewModel lss = null;

            using (var ctx = new EMSEntities())
            {
                lss = ctx.LSCHOOLMSTs
                    .Where(s => s.TRNNO == id)
                    .Select(s => new LSchoolViewModel()
                    {
                        TRNNO = s.TRNNO,
                        CLASS_TRNNO = s.CLASS_TRNNO,
                        LDATE = s.LDATE
                    }).FirstOrDefault<LSchoolViewModel>();
            }

            if (lss == null)
            {
                return NotFound();
            }

            return Ok(lss);
        }

        [HttpGet]
        [Route("lschool/detail")]
        public IHttpActionResult GetAllLsDetail()
        {
            IList<LSchoolDtlViewModel> lsdtl = null;

            using (var ctx = new EMSEntities())
            {
                lsdtl = ctx.LSCHOOLDTLs
                            .Select(s => new LSchoolDtlViewModel()
                            {
                                TRNNO = s.TRNNO,
                                EM_TRNNO = s.EM_TRNNO,
                                SR = s.SR
                            }).ToList<LSchoolDtlViewModel>();
            }

            if (lsdtl.Count == 0)
            {
                return NotFound();
            }

            return Ok(lsdtl);
        }
        [HttpPost]
        [Route("lschool")]
        public IHttpActionResult PostNewLs(LSchoolViewModel bp)
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
                    _trnno = Convert.ToInt32(ctx.LSCHOOLMSTs.OrderByDescending(t => t.TRNNO).FirstOrDefault().TRNNO);
                    _trnno = _trnno + 1;
                    //_trnno = Convert.ToInt32(ctx.EMs.OrderByDescending(t => t.TRNNO).First().ToString());
                }
                else
                {
                    _trnno = Convert.ToInt32(bp.TRNNO) + 1;
                }
               // int totalConunt = ctx.LSCHOOLMSTs.Count<LSCHOOLMST>();
                bp.TRNNO = _trnno;
                ctx.LSCHOOLMSTs.Add(new LSCHOOLMST()
                {
                    TRNNO = bp.TRNNO,
                    CLASS_TRNNO = bp.CLASS_TRNNO,
                    LDATE = bp.LDATE
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
        public IHttpActionResult Put(LSchoolViewModel bp)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            using (var ctx = new EMSEntities())
            {
                try
                {
                    var existingLs = ctx.LSCHOOLMSTs.Where(s => s.TRNNO == bp.TRNNO)
                                                        .FirstOrDefault<LSCHOOLMST>();

                    if (existingLs != null)
                    {
                        existingLs.CLASS_TRNNO = bp.CLASS_TRNNO;
                        existingLs.LDATE = bp.LDATE;

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
        public IHttpActionResult DeleteLs(int id)
        {
            if (id <= 0)
                return BadRequest("Not a valid student id");

            using (var ctx = new EMSEntities())
            {
                var bp = ctx.LSCHOOLMSTs
                    .Where(s => s.TRNNO == id)
                    .FirstOrDefault();
                ctx.Entry(bp).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }

            return Ok();
        }
    }
}
