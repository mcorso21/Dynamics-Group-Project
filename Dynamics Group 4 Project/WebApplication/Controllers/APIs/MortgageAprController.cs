using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Results;
using DataAccessLayer.Models;

namespace WebApplication.Controllers.APIs
{
    [EnableCors("*", "*", "*")]
    public class MortgageAprController : ApiController
    {
        public static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();     //logger.Info(e.Message);
        private MortgageBaseApr baseApr = new MortgageBaseApr();

        [HttpGet]
        public JsonResult<MortgageBaseApr> GetApr()
        {
            return Json<MortgageBaseApr>(new MortgageBaseApr()
            {
                Date = baseApr.GetLastModified(),
                Apr = baseApr.GetRate()
            });
        }
    }
}
