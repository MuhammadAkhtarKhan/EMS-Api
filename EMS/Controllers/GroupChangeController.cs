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
    public class GroupChangeController : ApiController
    {
        public IHttpActionResult GetAllGrp()
        {
            IList<GroupChangeViewModel> gps = null;

            using (var ctx = new EMSEntities())
            {
                gps = ctx.GRPCHANGEs
                            .Select(s => new GroupChangeViewModel()
                            {
                                TRNNO = s.TRNNO,
                                CHDATE = s.CHDATE,
                                EM_TRNNO = s.EM_TRNNO,
                                GRPMST_TRNNO=s.GRPMST_TRNNO
                            }).ToList<GroupChangeViewModel>();
            }

            if (gps.Count == 0)
            {
                return NotFound();
            }

            return Ok(gps);
        }
        public IHttpActionResult GetGrpById(int id)
        {
            GroupChangeViewModel gps = null;

            using (var ctx = new EMSEntities())
            {
                gps = ctx.GRPCHANGEs
                    .Where(s => s.TRNNO == id)
                    .Select(s => new GroupChangeViewModel()
                    {
                        TRNNO = s.TRNNO,
                        CHDATE = s.CHDATE,
                        EM_TRNNO = s.EM_TRNNO,
                        GRPMST_TRNNO = s.GRPMST_TRNNO
                    }).FirstOrDefault<GroupChangeViewModel>();
            }

            if (gps == null)
            {
                return NotFound();
            }

            return Ok(gps);
        }

        //public IHttpActionResult GetAllGrps(string name)
        //{
        //    IList<GroupChangeViewModel> gps = null;

        //    using (var ctx = new EMSEntities())
        //    {
        //        gps = ctx.GRPCHANGEs
        //            .Where(s => s.CHDATE.ToLower() == name.ToLower())
        //            .Select(s => new GroupChangeViewModel()
        //            {
        //                TRNNO = s.TRNNO,
        //                CHDATE = s.CHDATE,
        //                EM_TRNNO = s.EM_TRNNO,
        //                GRPMST_TRNNO = s.GRPMST_TRNNO

        //            }).ToList<GroupChangeViewModel>();
        //    }

        //    if (gps.Count == 0)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(gps);
        //}
        public IHttpActionResult PostNewGrp(GroupChangeViewModel bp)
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
                    _trnno = Convert.ToInt32(ctx.GRPCHANGEs.OrderByDescending(t => t.TRNNO).FirstOrDefault().TRNNO);
                    _trnno = _trnno + 1;
                    //_trnno = Convert.ToInt32(ctx.EMs.OrderByDescending(t => t.TRNNO).First().ToString());
                }
                else
                {
                    _trnno = Convert.ToInt32(bp.TRNNO) + 1;
                }
               // int totalConunt = ctx.GRPCHANGEs.Count<GRPCHANGE>();
                //bp.TRNNO = _trnno;
                ctx.GRPCHANGEs.Add(new GRPCHANGE()
                {
                    TRNNO = _trnno,
                    CHDATE = bp.CHDATE,
                    EM_TRNNO = bp.EM_TRNNO,
                    GRPMST_TRNNO = bp.GRPMST_TRNNO
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
        public IHttpActionResult Put(GroupChangeViewModel bp)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            using (var ctx = new EMSEntities())
            {
                try
                {
                    var existingGrp = ctx.GRPCHANGEs.Where(s => s.TRNNO == bp.TRNNO)
                                                        .FirstOrDefault<GRPCHANGE>();

                    if (existingGrp != null)
                    {
                        existingGrp.CHDATE = bp.CHDATE;
                        existingGrp.EM_TRNNO = bp.EM_TRNNO;
                        existingGrp.GRPMST_TRNNO = bp.GRPMST_TRNNO;
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
    }
}
