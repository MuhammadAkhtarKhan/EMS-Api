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
    public class SectionsController : ApiController
    {
        [HttpGet]
        [Route("sections")]
        public IHttpActionResult GetAllSections()
        {
            IList<SectionViewModel> secs = null;

            using (var ctx = new EMSEntities())
            {
                secs = ctx.SECMSTs
                            .Select(s => new SectionViewModel()
                            {
                                TRNNO = s.TRNNO,
                                CLASS_TRNNO = s.CLASS_TRNNO                                
                            }).ToList<SectionViewModel>();
            }

            if (secs.Count == 0)
            {
                return NotFound();
            }

            return Ok(secs);
        }

        [HttpGet]
        [Route("sections/details")]
        public IHttpActionResult GetAllSectionsDetail()
        {
            IList<SectionDetailViewModel> secs = null;

            using (var ctx = new EMSEntities())
            {
                secs = ctx.SECDTLs
                            .Select(s => new SectionDetailViewModel()
                            {
                                TRNNO = s.TRNNO,
                                SR = s.SR,
                                SNAME=s.SNAME,
                                STATUS=s.STATUS
                            }).ToList<SectionDetailViewModel>();
            }

            if (secs.Count == 0)
            {
                return NotFound();
            }

            return Ok(secs);
        }
        public IHttpActionResult GetSectionById(int id)
        {
            SectionViewModel secs = null;

            using (var ctx = new EMSEntities())
            {
                secs = ctx.SECMSTs
                    .Where(s => s.TRNNO == id)
                    .Select(s => new SectionViewModel()
                    {
                        TRNNO = s.TRNNO,
                        CLASS_TRNNO = s.CLASS_TRNNO                       
                    }).FirstOrDefault<SectionViewModel>();
            }

            if (secs == null)
            {
                return NotFound();
            }

            return Ok(secs);
        }

        //public IHttpActionResult GetAllSections(string name)
        //{
        //    IList<SectionViewModel> secs = null;

        //    using (var ctx = new EMSEntities())
        //    {
        //        secs = ctx.SECMSTs
        //            .Where(s => s.CLASS_TRNNO.ToLower() == name.ToLower())
        //            .Select(s => new SectionViewModel()
        //            {
        //                TRNNO = s.TRNNO,
        //                CLASS_TRNNO = s.CLASS_TRNNO,
        //                STATUS = s.STATUS

        //            }).ToList<SectionViewModel>();
        //    }

        //    if (secs.Count == 0)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(secs);
        //}
        public IHttpActionResult PostNewSection(SectionViewModel sec)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            using (var ctx = new EMSEntities())
            {
                int _trnno;
                int _sr = 1;
                //db.Users.OrderByDescending(u => u.UserId).FirstOrDefault();
                if (sec.TRNNO == 0)
                {
                    _trnno = Convert.ToInt32(ctx.SECMSTs.OrderByDescending(t => t.TRNNO).FirstOrDefault().TRNNO);
                    _trnno = _trnno + 1;
                    //_trnno = Convert.ToInt32(ctx.EMs.OrderByDescending(t => t.TRNNO).First().ToString());
                }
                else
                {
                    _trnno = Convert.ToInt32(sec.TRNNO) + 1;
                }
                ctx.SECMSTs.Add(new SECMST()
                {
                    TRNNO = sec.TRNNO,
                    CLASS_TRNNO = sec.CLASS_TRNNO                   
                });

                foreach (var secdtl in sec.SECDTLs)
                {
                    var detailSection = new SECDTL
                    {
                        //BudgetId = Here i need to get id of the Budget table that i inserted before
                        TRNNO = _trnno,
                        SR = _sr,
                        SNAME = secdtl.SNAME,
                        STATUS = secdtl.STATUS,
                    };
                    ctx.SECDTLs.Add(detailSection);
                    _sr++;
                }
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
        public IHttpActionResult Put(SectionViewModel sec)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            using (var ctx = new EMSEntities())
            {
                try
                {
                    var existingSection = ctx.SECMSTs.Where(s => s.TRNNO == sec.TRNNO)
                                                        .FirstOrDefault<SECMST>();

                    if (existingSection != null)
                    {
                        existingSection.CLASS_TRNNO = sec.CLASS_TRNNO;
                        existingSection.TRNNO = sec.TRNNO;
                        foreach (var secdtl in sec.SECDTLs)
                        {
                            var detailSection = new SECDTL
                            {
                                //BudgetId = Here i need to get id of the Budget table that i inserted before
                                TRNNO = sec.TRNNO,
                                SR = secdtl.SR,
                                SNAME = secdtl.SNAME,
                                STATUS = secdtl.STATUS,
                            };
                            ctx.SECDTLs.Add(detailSection);                            
                        }
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
        public IHttpActionResult DeleteSection(int id)
        {
            if (id <= 0)
                return BadRequest("Not a valid student id");

            using (var ctx = new EMSEntities())
            {
                var sec = ctx.SECMSTs
                    .Where(s => s.TRNNO == id)
                    .FirstOrDefault();
                var secdtl = ctx.SECDTLs.Where(d => d.TRNNO == id).FirstOrDefault();
                ctx.Entry(secdtl).State = System.Data.Entity.EntityState.Deleted;

                ctx.Entry(sec).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }

            return Ok();
        }
    }
}
