﻿using EMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EMS.Controllers
{
    [RoutePrefix("api")]
    public class MarksController : ApiController
    {
        [HttpGet]
        [Route("marksmst")]
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
                                MDT = s.MDT,
                                EXAM_TRNNO = s.EXAM_TRNNO,
                                GRPMST_TRNNO = s.GRPMST_TRNNO
                            }).ToList<MarksViewModel>();
            }

            if (mkss.Count == 0)
            {
                return NotFound();
            }

            return Ok(mkss);
        }

        [HttpGet]
        [Route("marksdtl")]
        public IHttpActionResult GetAllMarksdtl()
        {
            IList<MarksDtlViewModel> mkdtl = null;

            using (var ctx = new EMSEntities())
            {
                mkdtl = ctx.MARKSDTLs
                            .Select(s => new MarksDtlViewModel()
                            {
                                TRNNO = s.TRNNO,
                                SR = s.SR,
                                EM_TRNNO = s.EM_TRNNO
                            }).ToList<MarksDtlViewModel>();
            }

            if (mkdtl.Count == 0)
            {
                return NotFound();
            }

            return Ok(mkdtl);
        }
        [HttpGet]
        [Route("marksdtl1")]
        public IHttpActionResult GetAllMarksdtl1()
        {
            IList<MarksDtl1ViewModel> mkdtl = null;

            using (var ctx = new EMSEntities())
            {
                mkdtl = ctx.MARKSDTL1
                            .Select(s => new MarksDtl1ViewModel()
                            {
                                TRNNO = s.TRNNO,
                                SR = s.SR,
                                SR1 = s.SR1,
                                GMARKS = s.GMARKS,
                                MARKTOTALDTL_TRNNO = s.MARKTOTALDTL_TRNNO,
                                MARKTOTALDTL_SR = s.MARKTOTALDTL_SR
                            }).ToList<MarksDtl1ViewModel>();
            }

            if (mkdtl.Count == 0)
            {
                return NotFound();
            }

            return Ok(mkdtl);
        }
        [HttpGet]
        [Route("marksmst/{id}")]
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
        [HttpPost]
        [Route("marks")]
        public IHttpActionResult PostNewMarks(MarksViewModel mks)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            using (var ctx = new EMSEntities())
            {
                int _trnno;
                int _sr1 = 1;
                int _sr = 1;
                //int _oldTrnno = 0;
              var  _oldTrnno = ctx.MARKSMSTs.Where(s => s.MDT == mks.MDT && s.EXAM_TRNNO == mks.EXAM_TRNNO && s.CLASS_TRNNO == mks.CLASS_TRNNO && s.GRPMST_TRNNO == mks.GRPMST_TRNNO && s.EM_TRNNO == mks.EM_TRNNO).FirstOrDefault();
                if (_oldTrnno != null)
                {
                   _trnno = Convert.ToInt32( _oldTrnno.TRNNO);
                }
                else
                {
                    _trnno = Convert.ToInt32(ctx.MARKSMSTs.OrderByDescending(t => t.TRNNO).FirstOrDefault().TRNNO);
                    _trnno = _trnno + 1;
                }
               
                    mks.TRNNO = _trnno;
                    ctx.MARKSMSTs.Add(new MARKSMST()
                    {
                        TRNNO = mks.TRNNO,
                        MDT = mks.MDT,
                        EXAM_TRNNO = mks.EXAM_TRNNO,
                        CLASS_TRNNO = mks.CLASS_TRNNO,
                        GRPMST_TRNNO = mks.GRPMST_TRNNO,
                        EM_TRNNO = mks.EM_TRNNO,
                        
                    });
                    foreach (var dtls in mks.MARKSDTLs)
                    {
                        var markdetail = new MARKSDTL
                        {
                            TRNNO = _trnno,
                            SR = _sr,
                            EM_TRNNO = dtls.EM_TRNNO
                        };
                        ctx.MARKSDTLs.Add(markdetail);
                        
                    foreach (var dt1 in dtls.MARKSDTL1)
                    {
                        var markdtl1 = new MARKSDTL1
                        {
                            TRNNO = _trnno,
                            SR = _sr,
                            SR1 = dt1.SR1,
                            GMARKS = dt1.GMARKS,
                            MARKTOTALDTL_TRNNO = dt1.MARKTOTALDTL_TRNNO,
                            MARKTOTALDTL_SR = dt1.MARKTOTALDTL_SR
                        };
                        ctx.MARKSDTL1.Add(markdtl1);
                        _sr1++;
                    }
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
