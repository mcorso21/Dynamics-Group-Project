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
    public class MortgageIndexModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DynamicsId { get; set; }
        public string WebAppId { get; set; }
        public List<MortgageModel> Mortgages { get; set; }
        public List<MortgageCaseModel> Cases { get; set; }
        public List<MortgagePaymentRecordModel> Payments { get; set; }
    }

    [Authorize]
    public class MortgageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: User
        public ActionResult Index()
        {
            //return View(db.UserMapModels.ToList());
            MortgageIndexModel mortgageIndexModel = new MortgageIndexModel();
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    Guid userId = new Guid(User.Identity.GetUserId());
                    var user = (from u in context.UserMapModels
                                where u.UserWebAppId == userId
                                select u).ToList()[0];

                    mortgageIndexModel.FirstName =  user.FirstName;
                    mortgageIndexModel.LastName =   user.LastName;
                    mortgageIndexModel.DynamicsId = user.ClientDynamicsId.ToString();
                    mortgageIndexModel.WebAppId =   user.UserWebAppId.ToString();
                    mortgageIndexModel.Cases = DataAccessLayer.DynamicsDB.GetCases(user.ClientDynamicsId);
                    mortgageIndexModel.Mortgages = DataAccessLayer.DynamicsDB.GetMortgages(user.ClientDynamicsId);
                }
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"MortgageController Index threw: {ex.Message}\n{ex.StackTrace}");
            }
            ViewBag.Title = $"{User.Identity.Name}";
            return View(mortgageIndexModel);
        }

        // GET: User/Details/5
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

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserWebAppId,ClientDynamicsId,UserDynamicsId,FirstName,LastName,SSN")] UserMapModel userMapModel)
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

        // GET: User/Edit/5
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

        // POST: User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserWebAppId,ClientDynamicsId,UserDynamicsId,FirstName,LastName,SSN")] UserMapModel userMapModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userMapModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userMapModel);
        }

        // GET: User/Delete/5
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

        // POST: User/Delete/5
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
