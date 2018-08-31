using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataAccessLayer.Models;
using System.Web.Http.Cors;
using System.Web.Http.Results;

namespace WebApplication.Controllers.APIs
{
    [EnableCors("*", "*", "*")]
    public class MortgageCaseController : ApiController
    {
        public static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();     //logger.Info(e.Message);

        [HttpGet]
        public JsonResult<IEnumerable<MortgageCaseModel>> Get()
        {
            //return Json<IEnumerable<MortgageCaseModel>>(MessageSqlDbAccess.GetAll());

            /*
            
             */
            return null;
        }

        [HttpGet]
        public JsonResult<MortgageCaseModel> Get(Guid Id)
        {
            //return Json<MortgageCaseModel>(MessageSqlDbAccess.GetById(Id));
            return null;
        }

        [HttpPost]
        public IHttpActionResult Post(MortgageCaseModel m)
        {
            //if (m != null)
            //{
            //    try
            //    {
            //        m.Id = Guid.NewGuid();
            //        m.WasRead = false;

            //        MessageSqlDbAccess.Add(m);
            //        logger.Info($"MessageController.Post successfully added {m.Print()}");
            //        return Ok();
            //    }
            //    catch (Exception ex)
            //    {
            //        logger.Info($"MessageController.Post threw error {ex.Message} trying to add {m.Print()}");
            //        return BadRequest();
            //    }
            //}
            //else
            //{
            //    logger.Info($"MessageController.Post failed to add {m.Print()}");
            //    return BadRequest();
            //}

            return Ok();
        }

        [HttpPut]
        public IHttpActionResult Put(Guid Id, [FromBody] MortgageCaseModel m)
        {
            //if (m != null)
            //{
            //    try
            //    {
            //        MortgageCaseModel original = MessageSqlDbAccess.GetById(Id);

            //        if (m.WasRead != null) original.WasRead = m.WasRead;
            //        if (m.FullName != null) original.FullName = m.FullName;
            //        if (m.Email != null) original.Email = m.Email;
            //        if (m.Message != null) original.Message = m.Message;

            //        MessageSqlDbAccess.Update(original);
            //        logger.Info($"MessageController.Put successfully edited {m.Print()}");
            //        return Ok();
            //    }
            //    catch (Exception ex)
            //    {
            //        logger.Info($"MessageController.Put threw error {ex.Message} trying to edit {m.Print()}");
            //        return BadRequest();
            //    }
            //}
            //else
            //{
            //    logger.Info($"MessageController.Put failed to edit {m.Print()}");
            //    return BadRequest();
            //}

            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult Delete(Guid Id)
        {
            //try
            //{
            //    MessageSqlDbAccess.Delete(Id);
            //    logger.Info($"MessageController.Delete successfully deleted id {Id}");
            //    return Ok();
            //}
            //catch (Exception ex)
            //{
            //    logger.Info($"MessageController.Delete threw error {ex.Message} trying to delete {Id}");
            //    return BadRequest();
            //}

            return Ok();
        }
    }
}

/*


namespace WebAPI.Controllers
{
    
    public class MessageController : ApiController
    {
        public static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();     //logger.Info(e.Message);

        [HttpGet]
        public JsonResult<IEnumerable<MortgageCaseModel>> Get()
        {
            return Json<IEnumerable<MortgageCaseModel>>(MessageSqlDbAccess.GetAll());
        }

        [HttpGet]
        public JsonResult<MortgageCaseModel> Get(Guid Id)
        {
            return Json<MortgageCaseModel>(MessageSqlDbAccess.GetById(Id));
        }

        [HttpPost]
        public IHttpActionResult Post(MortgageCaseModel m)
        {
            if (m != null)
            {
                try
                {
                    m.Id = Guid.NewGuid();
                    m.WasRead = false;

                    MessageSqlDbAccess.Add(m);
                    logger.Info($"MessageController.Post successfully added {m.Print()}");
                    return Ok();
                }
                catch (Exception ex)
                {
                    logger.Info($"MessageController.Post threw error {ex.Message} trying to add {m.Print()}");
                    return BadRequest();
                }
            }
            else
            {
                logger.Info($"MessageController.Post failed to add {m.Print()}");
                return BadRequest();
            }
        }

        [HttpPut]
        public IHttpActionResult Put(Guid Id, [FromBody] MortgageCaseModel m)
        {
            if (m != null)
            {
                try
                {
                    MortgageCaseModel original = MessageSqlDbAccess.GetById(Id);

                    if (m.WasRead != null) original.WasRead = m.WasRead;
                    if (m.FullName != null) original.FullName = m.FullName;
                    if (m.Email != null) original.Email = m.Email;
                    if (m.Message != null) original.Message = m.Message;

                    MessageSqlDbAccess.Update(original);
                    logger.Info($"MessageController.Put successfully edited {m.Print()}");
                    return Ok();
                }
                catch (Exception ex)
                {
                    logger.Info($"MessageController.Put threw error {ex.Message} trying to edit {m.Print()}");
                    return BadRequest();
                }
            }
            else
            {
                logger.Info($"MessageController.Put failed to edit {m.Print()}");
                return BadRequest();
            }
        }

        [HttpDelete]
        public IHttpActionResult Delete(Guid Id)
        {
            try
            {
                MessageSqlDbAccess.Delete(Id);
                logger.Info($"MessageController.Delete successfully deleted id {Id}");
                return Ok();
            }
            catch (Exception ex)
            {
                logger.Info($"MessageController.Delete threw error {ex.Message} trying to delete {Id}");
                return BadRequest();
            }
        }
    }
}

 */
