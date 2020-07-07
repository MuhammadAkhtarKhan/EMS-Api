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
    public class GroupController : ApiController
    {
        public IHttpActionResult GetAllGroup()
        {
            IList<GroupViewModel> GRPMSTs = null;

            using (var ctx = new EMSEntities())
            {
                GRPMSTs = ctx.GRPMSTs
                            .Select(s => new GroupViewModel()
                            {
                                TRNNO = s.TRNNO,
                                GRPNAME = s.GRPNAME,
                                DT = s.DT
                            }).ToList<GroupViewModel>();
            }

            if (GRPMSTs.Count == 0)
            {
                return NotFound();
            }

            return Ok(GRPMSTs);
        }
        public IHttpActionResult GetGroupById(int id)
        {
            GroupViewModel GRPMSTs = null;

            using (var ctx = new EMSEntities())
            {
                GRPMSTs = ctx.GRPMSTs
                    .Where(s => s.TRNNO == id)
                    .Select(s => new GroupViewModel()
                    {
                        TRNNO = s.TRNNO,
                        GRPNAME = s.GRPNAME,
                        DT = s. DT
                    }).FirstOrDefault<GroupViewModel>();
            }

            if (GRPMSTs == null)
            {
                return NotFound();
            }

            return Ok(GRPMSTs);
        }

        public IHttpActionResult GetAllGRPMSTs(string name)
        {
            IList<GroupViewModel> GRPMSTs = null;

            using (var ctx = new EMSEntities())
            {
                GRPMSTs = ctx.GRPMSTs
                    .Where(s => s.GRPNAME.ToLower() == name.ToLower())
                    .Select(s => new GroupViewModel()
                    {
                        TRNNO = s.TRNNO,
                        GRPNAME = s.GRPNAME,
                         DT = s. DT

                    }).ToList<GroupViewModel>();
            }

            if (GRPMSTs.Count == 0)
            {
                return NotFound();
            }

            return Ok(GRPMSTs);
        }
        public IHttpActionResult PostNewGroup(GroupViewModel gp)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            using (var ctx = new EMSEntities())
            {
                int _trnno;
                if (gp.TRNNO == 0)
                {
                    _trnno = Convert.ToInt32(ctx.GRPDTLs.OrderByDescending(t => t.TRNNO).FirstOrDefault().TRNNO);
                    _trnno = _trnno + 1;
                    //_trnno = Convert.ToInt32(ctx.EMs.OrderByDescending(t => t.TRNNO).First().ToString());
                }
                else
                {
                    _trnno = Convert.ToInt32(gp.TRNNO) + 1;
                }
                // int totalConunt = ctx.MARKTOTALs.Count<MARKTOTAL>();
                gp.TRNNO = _trnno;
                //int totalConunt = ctx.GRPMSTs.Count<GRPMST>();               
                ctx.GRPMSTs.Add(new GRPMST()
                {
                    TRNNO = gp.TRNNO,
                    GRPNAME = gp.GRPNAME,
                     DT = gp. DT,
                     COMPDTL_TRNNO=gp.COMPDTL_TRNNO
                });
                foreach (var dtls in gp.GRPDTLs)
                {
                    var grpdtail = new GRPDTL
                    {
                        //BudgetId = Here i need to get id of the Budget table that i inserted before
                        TRNNO = _trnno,
                        SUBJECT_TRNNO=dtls.TRNNO
                    };
                    ctx.GRPDTLs.Add(grpdtail);                    
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
        public IHttpActionResult Put(GroupViewModel Group)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            using (var ctx = new EMSEntities())
            {
                try
                {
                    var existingGroup = ctx.GRPMSTs.Where(s => s.TRNNO == Group.TRNNO)
                                                        .FirstOrDefault<GRPMST>();

                    if (existingGroup != null)
                    {
                        existingGroup.GRPNAME = Group.GRPNAME;
                        existingGroup. DT = Group. DT;

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
        public IHttpActionResult DeleteGroup(int id)
        {
            if (id <= 0)
                return BadRequest("Not a valid student id");

            using (var ctx = new EMSEntities())
            {
                var Group = ctx.GRPMSTs
                    .Where(s => s.TRNNO == id)
                    .FirstOrDefault();
                ctx.Entry(Group).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }

            return Ok();
        }
    }
}
