using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication.Controllers.APIs
{
    public class AprController : ApiController
    {
        public IHttpActionResult GetApr()
        {
            //var product = products.FirstOrDefault((p) => p.Id == id);
            //if (product == null)
            //{
            //    return NotFound();
            //}
            //return Ok(product);

            return Ok(42.1);
        }
    }
}
