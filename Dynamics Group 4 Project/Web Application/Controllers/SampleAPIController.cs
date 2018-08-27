/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Models.ContactMe;
using BusinessLogic;
using DataAccess;
using System.Web.Http.Cors;
using System.Web.Http.Results;

namespace WebAPI.Controllers
{
    [EnableCors("*", "*", "*")]
    public class MessageController : ApiController
    {
        public static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();     //logger.Info(e.Message);

        [HttpGet]
        public JsonResult<IEnumerable<MessageModel>> Get()
        {
            return Json<IEnumerable<MessageModel>>(MessageSqlDbAccess.GetAll());
        }

        [HttpGet]
        public JsonResult<MessageModel> Get(Guid Id)
        {
            return Json<MessageModel>(MessageSqlDbAccess.GetById(Id));
        }

        [HttpPost]
        public IHttpActionResult Post(MessageModel m)
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
        public IHttpActionResult Put(Guid Id, [FromBody] MessageModel m)
        {
            if (m != null)
            {
                try
                {
                    MessageModel original = MessageSqlDbAccess.GetById(Id);

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