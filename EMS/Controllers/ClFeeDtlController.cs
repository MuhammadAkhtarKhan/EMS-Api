using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EMS.Models;

namespace EMS.Controllers
{
    public class ClFeeDtlController : ApiController
    {
        public IHttpActionResult GetAllTotalMarkDtl()
        {
            IList<ClFeedtlViewModel> tmdtls = null;

            using (var ctx = new EMSEntities())
            {
                tmdtls = ctx.CLFEEDTLs
                            .Select(s => new ClFeedtlViewModel()
                            {
                                TRNNO = s.TRNNO,
                                SR = s.SR,
                                 FEE = s.FEE,
                                ADT = s.ADT
                            }).ToList<ClFeedtlViewModel>();
            }

            if (tmdtls.Count == 0)
            {
                return NotFound();
            }

            return Ok(tmdtls);
        }
        public IHttpActionResult GetTotalMarkDtlById(int id)
        {
            //IList<ClFeedtlViewModel> tmdtls = null;
            IList<ClFeedtlViewModel> tmdtls = null;

            using (var ctx = new EMSEntities())
            {
                //tmdtls = ctx.MARKTOTALDTLs
                //    .Where(s => s.TRNNO == id)
                //    .Select(s => new ClFeedtlViewModel()
                //    {
                //        TRNNO = s.TRNNO,
                //        SR = s.SR,
                //        FEE = s.FEE,
                //        ADT = s.ADT
                //    }).ToList<ClFeedtlViewModel>();
                tmdtls = ctx.Database.SqlQuery<ClFeedtlViewModel>("SELECT TOP 1[TRNNO] ,[SR] ,[FEE] ,[ADT] FROM[EMS].[EMS].[CLFEEDTL] where TRNNO = @id order by SR desc; ", new SqlParameter("@id", id)).ToList<ClFeedtlViewModel>();

                //tmdtls = ctx.Database.SqlQuery<TotalMarkDtlSubjectViewModel>("SELECT s.TRNNO , s.SNAME, s.SCODE, mkdtl.TRNNO as MKTOTAL_TRNNO, mkdtl.SR, mkdtl.ADT FROM EMS.MARKTOTALDTL mkdtl INNER JOIN EMS.SUBJECT s ON s.TRNNO = mkdtl.FEE WHERE mkdtl.TRNNO = @id", new SqlParameter("@id", id)).ToList<TotalMarkDtlSubjectViewModel>();
                
    }

            if (tmdtls == null)
            {
                return NotFound();
            }

            return Ok(tmdtls);
        }
    }
}
