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
    public class MarksController : ApiController
    {
        public IHttpActionResult GetAllMarks()
        {
            IList<MarksViewModel> mkss = null;

            using (var ctx = new EMSEntities())
            {
                mkss = ctx.MARKSMSTs
                            .Select(s => new MarksViewModel()
                            {
                                TRNNO = s.TRNNO,
                                CLASS_TRNNO = s.CLASS_TRNNO,
                                EM_TRNNO = s.EM_TRNNO,
                                MDT=s.MDT,
                                 EXAM_TRNNO=s.EXAM_TRNNO,
                                 GRPMST_TRNNO=s.GRPMST_TRNNO
                            }).ToList<MarksViewModel>();
            }

            if (mkss.Count == 0)
            {
                return NotFound();
            }

            return Ok(mkss);
        }
        public IHttpActionResult GetMarksById(int id)
        {
            MarksViewModel mkss = null;

            using (var ctx = new EMSEntities())
            {
                mkss = ctx.MARKSMSTs
                    .Where(s => s.TRNNO == id)
                    .Select(s => new MarksViewModel()
                    {
                        TRNNO = s.TRNNO,
                        CLASS_TRNNO = s.CLASS_TRNNO,
                         EM_TRNNO = s.EM_TRNNO
                    }).FirstOrDefault<MarksViewModel>();
            }

            if (mkss == null)
            {
                return NotFound();
            }

            return Ok(mkss);
        }

        //public IHttpActionResult GetAllMarkss(string name)
        //{
        //    //IList<MarksViewModel> mkss = null;

        //    //using (var ctx = new EMSEntities())
        //    //{
        //    //    mkss = ctx.MARKSMSTs
        //    //        .Where(s => s..ToLower() == name.ToLower())
        //    //        .Select(s => new MarksViewModel()
        //    //        {
        //    //            TRNNO = s.TRNNO,
        //    //            CLASS_TRNNO = s.CLASS_TRNNO,
        //    //            STATUS = s.STATUS

        //    //        }).ToList<MarksViewModel>();
        //    //}

        //    //if (mkss.Count == 0)
        //    //{
        //    //    return NotFound();
        //    //}

        //    //return Ok(mkss);
        //}
        public IHttpActionResult PostNewMarks(MarksViewModel mks)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            using (var ctx = new EMSEntities())
            {
                int totalConunt = ctx.MARKSMSTs.Count<MARKSMST>();
                mks.TRNNO = totalConunt + 1;
                ctx.MARKSMSTs.Add(new MARKSMST()
                {
                    TRNNO = mks.TRNNO,
                    CLASS_TRNNO = mks.CLASS_TRNNO,
                    EM_TRNNO = mks.EM_TRNNO
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
        public IHttpActionResult Put(MarksViewModel mks)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            using (var ctx = new EMSEntities())
            {
                try
                {
                    var existingMarks = ctx.MARKSMSTs.Where(s => s.TRNNO == mks.TRNNO)
                                                        .FirstOrDefault<MARKSMST>();

                    if (existingMarks != null)
                    {
                        existingMarks.CLASS_TRNNO = mks.CLASS_TRNNO;
                        existingMarks.EM_TRNNO = mks.EM_TRNNO;

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
        public IHttpActionResult DeleteMarks(int id)
        {
            if (id <= 0)
                return BadRequest("Not a valid student id");

            using (var ctx = new EMSEntities())
            {
                var mks = ctx.MARKSMSTs
                    .Where(s => s.TRNNO == id)
                    .FirstOrDefault();
                ctx.Entry(mks).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }

            return Ok();
        }
    }
}
