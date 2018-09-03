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

namespace WebApplication.Controllers.Mortgage
{
    public class UserMapModelsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: UserMapModels
        public ActionResult Index()
        {
            return View(db.UserMapModels.ToList());
        }

        // GET: UserMapModels/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserMapModel userMapModel = db.UserMapModels.Find(id);
            if (userMapModel == null)
            {
                return HttpNotFound();
            }
            return View(userMapModel);
        }

        // GET: UserMapModels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserMapModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserWebAppId,ClientDynamicsId,UserDynamicsId")] UserMapModel userMapModel)
        {
            if (ModelState.IsValid)
            {
                userMapModel.Id = Guid.NewGuid();
                db.UserMapModels.Add(userMapModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userMapModel);
        }

        // GET: UserMapModels/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserMapModel userMapModel = db.UserMapModels.Find(id);
            if (userMapModel == null)
            {
                return HttpNotFound();
            }
            return View(userMapModel);
        }

        // POST: UserMapModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserWebAppId,ClientDynamicsId,UserDynamicsId")] UserMapModel userMapModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userMapModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userMapModel);
        }

        // GET: UserMapModels/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserMapModel userMapModel = db.UserMapModels.Find(id);
            if (userMapModel == null)
            {
                return HttpNotFound();
            }
            return View(userMapModel);
        }

        // POST: UserMapModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            UserMapModel userMapModel = db.UserMapModels.Find(id);
            db.UserMapModels.Remove(userMapModel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
