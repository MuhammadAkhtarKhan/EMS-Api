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
    public class EmController : ApiController
    {
        public IHttpActionResult GetAllEm()
        {
            IList<EmViewModel> ems = null;

            using (var ctx = new EMSEntities())
            {
                ems = ctx.EMs
                            .Select(s => new EmViewModel()
                            {
                                TRNNO = s.TRNNO,
                                EMP_ID=s.EMP_ID,
                                 EMP_NAME = s.EMP_NAME,
                                CST_TRNNO = s.CST_TRNNO,
                                EMP_F_NAME = s.EMP_F_NAME,
                                DOB=s.DOB,
                                BP_TRNNO=s.BP_TRNNO,
                                ETYPE=s.ETYPE,
                                CL_TRNNO=s.CST_TRNNO,
                                CL_TRNNO1=s.CL_TRNNO1,
                                CERTIFICATE=s.CERTIFICATE,
                                PSCHDT=s.PSCHDT,
                                EMAIL=s.EMAIL,
                                ADT=s.ADT,
                                FTYPE=s.FTYPE,
                                FEE=s.FEE,
                                EMAILST=s.EMAILST,
                                GRPMST_TRNNO=s.GRPMST_TRNNO,
                                CNIC=s.CNIC,
                                SECMST_TRNNO=s.SECMST_TRNNO,
                                SECDTL_SR=s.SECDTL_SR,
                                SEX=s.SEX,
                                BSAL=s.BSAL,
                                FCNIC=s.FCNIC,
                                ADIMNO=s.ADIMNO,
                                STYPE=s.STYPE                               
                            }).ToList<EmViewModel>();
            }

            if (ems.Count == 0)
            {
                return NotFound();
            }

            return Ok(ems);
        }
        public IHttpActionResult GetEmById(int id)
        {
            EmViewModel ems = null;

            using (var ctx = new EMSEntities())
            {
                ems = ctx.EMs
                    .Where(s => s.TRNNO == id)
                    .Select(s => new EmViewModel()
                    {
                        TRNNO = s.TRNNO,
                        EMP_NAME = s.EMP_NAME,
                        EMP_F_NAME = s.EMP_F_NAME
                    }).FirstOrDefault<EmViewModel>();
            }

            if (ems == null)
            {
                return NotFound();
            }

            return Ok(ems);
        }

        public IHttpActionResult GetAllEms(string name)
        {
            IList<EmViewModel> ems = null;

            using (var ctx = new EMSEntities())
            {
                ems = ctx.EMs
                    .Where(s => s.EMP_NAME.ToLower() == name.ToLower())
                    .Select(s => new EmViewModel()
                    {
                        TRNNO = s.TRNNO,
                        EMP_NAME = s.EMP_NAME,
                        EMP_F_NAME = s.EMP_F_NAME

                    }).ToList<EmViewModel>();
            }

            if (ems.Count == 0)
            {
                return NotFound();
            }

            return Ok(ems);
        }
        public IHttpActionResult PostNewEm(EmViewModel em)
        {
             
            //foreach (var detail in newBudget.BudgetDetails)
            //{
            //    var detailBudget = new BudgetDetail
            //    {
            //        //BudgetId = Here i need to get id of the Budget table that i inserted before
            //        ProductId = detail.ProductId,
            //        Price = detail.Price,
            //        Quantity = detail.Quantity,
            //        Iva = detail.Iva,
            //        Total = detail.Total
            //    };
            //    _context.BudgetDetails.Add(detailBudget);
            //}
            //_context.SaveChanges();
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            using (var ctx = new EMSEntities())
            {
                int _trnno;
                int _sr = 1;
                //db.Users.OrderByDescending(u => u.UserId).FirstOrDefault();
                if (em.TRNNO == 0)
                {
                     _trnno = Convert.ToInt32(ctx.EMs.OrderByDescending(t => t.TRNNO).FirstOrDefault().TRNNO);
                    _trnno = _trnno + 1;
                    //_trnno = Convert.ToInt32(ctx.EMs.OrderByDescending(t => t.TRNNO).First().ToString());
                }
                else
                {
                     _trnno = Convert.ToInt32(em.TRNNO) + 1;
                }              
                
               // em.TRNNO = totalConunt + 1;
                ctx.EMs.Add(new EM()
                {
                    TRNNO = _trnno,
                    EMP_ID=em.EMP_ID,
                    EMP_NAME = em.EMP_NAME,
                    DOB = em.DOB,
                    DOJ=em.DOJ,
                    CST_TRNNO=em.CST_TRNNO,
                    EMP_F_NAME = em.EMP_F_NAME,
                    COMP_TRNNO=em.COMP_TRNNO,
                    BP_TRNNO = em.BP_TRNNO,
                    ETYPE=em.ETYPE,
                    CL_TRNNO=em.CL_TRNNO,
                    CL_TRNNO1=em.CL_TRNNO1,
                    CERTIFICATE=em.CERTIFICATE,
                    PSCHDT=em.PSCHDT,
                    EMAIL=em.EMAIL,
                    ADT = em.ADT,
                    FTYPE=em.FTYPE,
                    FEE=em.FEE,
                    EMAILST=em.EMAILST,
                    GRPMST_TRNNO=em.GRPMST_TRNNO,
                    CNIC=em.CNIC,
                    SECMST_TRNNO=em.SECMST_TRNNO,
                    SECDTL_SR=em.SECDTL_SR,
                    SEX=em.SEX,
                    FCNIC=em.FCNIC,
                    ADIMNO =em.ADIMNO,
                    STYPE=em.STYPE,
                    BSAL =em.BSAL,                    

                });
                foreach ( var address in em.EMDTLs)
                {
                    var detailAddress = new EMDTL
                    {
                        //BudgetId = Here i need to get id of the Budget table that i inserted before
                        TRNNO = _trnno,
                        SR = _sr,
                        ADDDESC = address.ADDDESC,
                        ADDTYPE = address.ADDTYPE,                        
                    };
                    ctx.EMDTLs.Add(detailAddress);
                    _sr++;
                }
                _sr = 1;
                foreach (var ph in em.EMDTL1)
                {
                    var detailPhones = new EMDTL1
                    {
                        //BudgetId = Here i need to get id of the Budget table that i inserted before
                        TRNNO = _trnno,
                        SR = _sr,
                         PHNO = ph.PHNO,
                        PHTYPE = ph.PHTYPE,
                        RELTION=ph.RELTION
                    };
                    ctx.EMDTL1.Add(detailPhones);
                    _sr++;
                }
                _sr = 1;
                foreach (var edu in em.EMDTL2)
                {
                    var detailEducation = new EMDTL2
                    {
                        //BudgetId = Here i need to get id of the Budget table that i inserted before
                        TRNNO = _trnno,
                        SR = _sr,                         
                         EDU_TRNNO = edu.EDU_TRNNO,
                        PTYPE = edu.PTYPE
                    };
                    ctx.EMDTL2.Add(detailEducation);
                    _sr++;
                }
                _sr = 1;

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
        public IHttpActionResult Put(EmViewModel em)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            using (var ctx = new EMSEntities())
            {
                try
                {
                    var existingEm = ctx.EMs.Where(s => s.TRNNO == em.TRNNO)
                                                        .FirstOrDefault<EM>();

                    if (existingEm != null)
                    {
                        existingEm.EMP_NAME = em.EMP_NAME;
                        existingEm.EMP_F_NAME = em.EMP_F_NAME;

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
        public IHttpActionResult DeleteEm(int id)
        {
            if (id <= 0)
                return BadRequest("Not a valid student id");

            using (var ctx = new EMSEntities())
            {
                var em = ctx.EMs
                    .Where(s => s.TRNNO == id)
                    .FirstOrDefault();
                ctx.Entry(em).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }

            return Ok();
        }
    }
}
