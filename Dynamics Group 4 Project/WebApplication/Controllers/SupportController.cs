using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataAccessLayer.Models;
using WebApplication.Models;
using Microsoft.AspNet.Identity;

namespace WebApplication.Controllers
{
    public class SupportController : Controller
    {
        public ActionResult Article(string article)
        {
            String f = HttpContext.Server.MapPath("~/Views/Support/" + article + ".cshtml");
            if (System.IO.File.Exists(f))
                ViewBag.ArticleContent = article;
            else
                ViewBag.ArticleContent = "_Error";

            return View();
        }
    }
}
