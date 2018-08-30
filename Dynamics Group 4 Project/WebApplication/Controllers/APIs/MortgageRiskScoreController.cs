using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Results;
using DataAccessLayer.Models;

namespace WebApplication.Controllers.APIs
{
    [EnableCors("*", "*", "*")]
    public class MortgageRiskScoreController: ApiController
    {
        public static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();     //logger.Info(e.Message);
        private MortgageCreditCheck creditCheck = new MortgageCreditCheck();

        [HttpGet]
        public JsonResult<MortgageCreditCheck> GetApr()
        {
            return Json<MortgageCreditCheck>(new MortgageCreditCheck()
            {
                RiskScore = creditCheck.GetRiskScore()
            });
        }
    }
}
