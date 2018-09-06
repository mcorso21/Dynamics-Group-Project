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
        //public static SelectList MortgageTermList = new SelectList(
        //    new List<SelectListItem>
        //    {
        //        new SelectListItem { Text = "10 Years", Value = "283210000" },
        //        new SelectListItem { Text = "15 Years", Value = "283210001" },
        //        new SelectListItem { Text = "30 Years", Value = "283210002" },
        //        new SelectListItem { Text = "30 Years", Value = "283210003" }
        //    }, "Value", "Text"
        //);

        //public enum TermEnum { Ten = 283210000, Fifteen = 283210001, Twenty = 283210002, Thirty = 283210003 };

        public static SelectList StatesSelectList = new SelectList(
            new List<SelectListItem>
            {
                new SelectListItem { Text = "Alabama", Value = "Alabama" },
                new SelectListItem { Text = "Alaska", Value = "Alaska" },
                new SelectListItem { Text = "Arizona", Value = "Arizona" },
                new SelectListItem { Text = "Arkansas", Value = "Arkansas" },
                new SelectListItem { Text = "California", Value = "California " },
                new SelectListItem { Text = "Colorado", Value = "Colorado" },
                new SelectListItem { Text = "Connecticut", Value = "Connecticut " },
                new SelectListItem { Text = "Delaware", Value = "Delaware" },
                new SelectListItem { Text = "District of Columbia", Value = "District of Columbia " },
                new SelectListItem { Text = "Florida", Value = "Florida" },
                new SelectListItem { Text = "Georgia", Value = "Georgia" },
                new SelectListItem { Text = "Hawaii", Value = "Hawaii" },
                new SelectListItem { Text = "Idaho", Value = "Idaho" },
                new SelectListItem { Text = "Illinois", Value = "Illinois" },
                new SelectListItem { Text = "Indiana", Value = "Indiana " },
                new SelectListItem { Text = "Iowa", Value = "Iowa" },
                new SelectListItem { Text = "Kansas", Value = "Kansas" },
                new SelectListItem { Text = "Kentucky", Value = "Kentucky" },
                new SelectListItem { Text = "Louisiana", Value = "Louisiana" },
                new SelectListItem { Text = "Maine", Value = "Maine " },
                new SelectListItem { Text = "Maryland", Value = "Maryland " },
                new SelectListItem { Text = "Massachusetts", Value = "Massachusetts " },
                new SelectListItem { Text = "Michigan", Value = "Michigan " },
                new SelectListItem { Text = "Minnesota", Value = "Minnesota" },
                new SelectListItem { Text = "Mississippi", Value = "Mississippi " },
                new SelectListItem { Text = "Missouri", Value = "Missouri " },
                new SelectListItem { Text = "Montana", Value = "Montana" },
                new SelectListItem { Text = "Nebraska", Value = "Nebraska" },
                new SelectListItem { Text = "Nevada", Value = "Nevada" },
                new SelectListItem { Text = "New Hampshire", Value = "New Hampshire" },
                new SelectListItem { Text = "New Jersey", Value = "New Jersey " },
                new SelectListItem { Text = "New Mexico", Value = "New Mexico" },
                new SelectListItem { Text = "New York", Value = "New York" },
                new SelectListItem { Text = "North Carolina", Value = "North Carolina" },
                new SelectListItem { Text = "North Dakota", Value = "North Dakota" },
                new SelectListItem { Text = "Ohio", Value = "Ohio" },
                new SelectListItem { Text = "Oklahoma", Value = "Oklahoma" },
                new SelectListItem { Text = "Oregon", Value = "Oregon" },
                new SelectListItem { Text = "Pennsylvania", Value = "Pennsylvania" },
                new SelectListItem { Text = "Rhode Island", Value = "Rhode Island " },
                new SelectListItem { Text = "South Carolina", Value = "South Carolina " },
                new SelectListItem { Text = "South Dakota", Value = "South Dakota" },
                new SelectListItem { Text = "Tennessee", Value = "Tennessee" },
                new SelectListItem { Text = "Texas", Value = "Texas" },
                new SelectListItem { Text = "Utah", Value = "Utah" },
                new SelectListItem { Text = "Vermont", Value = "Vermont" },
                new SelectListItem { Text = "Virginia", Value = "Virginia" },
                new SelectListItem { Text = "Washington", Value = "Washington" },
                new SelectListItem { Text = "West Virginia", Value = "West Virginia " },
                new SelectListItem { Text = "Wisconsin", Value = "Wisconsin" },
                new SelectListItem { Text = "Wyoming", Value = "Wyoming" }
            }, "Value", "Text"
        );

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
                    mortgageIndexModel.Payments = DataAccessLayer.DynamicsDB.GetPayments(mortgageIndexModel.Mortgages);
                }
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"MortgageController Index threw: {ex.Message}\n{ex.StackTrace}");
            }
            ViewBag.Title = $"{User.Identity.Name}";
            return View(mortgageIndexModel);
        }

        // GET: User/Create
        public ActionResult CreateCase()
        {
            List<SelectListItem> UserMortgages = new List<SelectListItem>();
            List<MortgageModel> Mortgages = new List<MortgageModel>();
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    Guid userId = new Guid(User.Identity.GetUserId());
                    var user = (from u in context.UserMapModels
                                where u.UserWebAppId == userId
                                select u).ToList()[0];

                    Mortgages = DataAccessLayer.DynamicsDB.GetMortgages(user.ClientDynamicsId);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"MortgageController CreateCase threw: {ex.Message}\n{ex.StackTrace}");
            }
            foreach(MortgageModel m in Mortgages)
            {
                UserMortgages.Add(new SelectListItem { Text = $"{m.Name} ({m.MortgageNumber})", Value = m.MortgageId.ToString() });
            }

            ViewBag.UserMortgages = new SelectList(UserMortgages, "Value", "Text");
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCase([Bind(Include = "MortgageId,Title,Description,Priority,HighPriorityReason,Type")] MortgageCaseModel caseModel)
        {
            if (ModelState.IsValid)
            {
                using (var context = new ApplicationDbContext())
                {
                    Guid userId = new Guid(User.Identity.GetUserId());
                    caseModel.ContactId = (from u in context.UserMapModels
                                           where u.UserWebAppId == userId
                                           select u).ToList()[0].ClientDynamicsId;
                }
                DataAccessLayer.DynamicsDB.CreateCase(caseModel);
                return RedirectToAction("Index");
            }

            List<SelectListItem> UserMortgages = new List<SelectListItem>();
            List<MortgageModel> Mortgages = new List<MortgageModel>();
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    Guid userId = new Guid(User.Identity.GetUserId());
                    var user = (from u in context.UserMapModels
                                where u.UserWebAppId == userId
                                select u).ToList()[0];

                    Mortgages = DataAccessLayer.DynamicsDB.GetMortgages(user.ClientDynamicsId);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"MortgageController CreateCase threw: {ex.Message}\n{ex.StackTrace}");
            }
            foreach (MortgageModel m in Mortgages)
            {
                UserMortgages.Add(new SelectListItem { Text = $"{m.Name} ({m.MortgageNumber})", Value = m.MortgageId.ToString() });
            }
            ViewBag.UserMortgages = new SelectList(UserMortgages, "Value", "Text");
            return View(caseModel);
        }

        // GET: User/Create
        public ActionResult NewMortgage()
        {
            ViewBag.States = StatesSelectList;
            //ViewBag.Terms = MortgageTermList;
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewMortgage([Bind(Include = "Name,Region,State,MortgageAmount,MortgageTermInYears,IdentityDocuments")] MortgageModel mortgageModel)
        {
            System.Diagnostics.Debug.WriteLine($"\n\n\nName={mortgageModel.Name},Region={mortgageModel.Region},State={mortgageModel.State},Amount={mortgageModel.MortgageAmount},Term={mortgageModel.MortgageTermInYears}");
            if (ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine($"Model is valid");
                using (var context = new ApplicationDbContext())
                {
                    Guid userId = new Guid(User.Identity.GetUserId());
                    mortgageModel.ContactId = (from u in context.UserMapModels
                                               where u.UserWebAppId == userId
                                               select u).ToList()[0].ClientDynamicsId;
                }
                DataAccessLayer.DynamicsDB.CreateMortgage(mortgageModel);
                return RedirectToAction("Index");
            }
            ViewBag.States = StatesSelectList;
            //ViewBag.Terms = MortgageTermList;
            return View(mortgageModel);
        }

        [HttpGet]
        public ActionResult MakePayment(Guid? id)
        {
            MortgageIndexModel mortgageIndexModel = new MortgageIndexModel();
            try
            {
                // Update payment
                DataAccessLayer.DynamicsDB.MakePayment((Guid)id);
                //// Get user data from web app SQL DB
                //using (var context = new ApplicationDbContext())
                //{
                //    Guid userId = new Guid(User.Identity.GetUserId());
                //    var user = (from u in context.UserMapModels
                //                where u.UserWebAppId == userId
                //                select u).ToList()[0];

                //    mortgageIndexModel.FirstName = user.FirstName;
                //    mortgageIndexModel.LastName = user.LastName;
                //    mortgageIndexModel.DynamicsId = user.ClientDynamicsId.ToString();
                //    mortgageIndexModel.WebAppId = user.UserWebAppId.ToString();
                //}
                //// Get contact's data from Dynamics
                //mortgageIndexModel.Cases = DataAccessLayer.DynamicsDB.GetCases(new Guid(mortgageIndexModel.DynamicsId));
                //mortgageIndexModel.Mortgages = DataAccessLayer.DynamicsDB.GetMortgages(new Guid(mortgageIndexModel.DynamicsId));
                //mortgageIndexModel.Payments = DataAccessLayer.DynamicsDB.GetPayments(mortgageIndexModel.Mortgages);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"MortgageController Index threw: {ex.Message}\n{ex.StackTrace}");
            }
            //ViewBag.Title = $"{User.Identity.Name}";
            return RedirectToAction("Index");
        }

        //// GET: User/Details/5
        //public ActionResult Details(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    UserMapModel userMapModel = db.UserMapModels.Find(id);
        //    if (userMapModel == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(userMapModel);
        //}

        //// GET: User/Edit/5
        //public ActionResult Edit(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    UserMapModel userMapModel = db.UserMapModels.Find(id);
        //    if (userMapModel == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(userMapModel);
        //}

        //// POST: User/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,UserWebAppId,ClientDynamicsId,UserDynamicsId,FirstName,LastName,SSN")] UserMapModel userMapModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(userMapModel).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(userMapModel);
        //}

        //// GET: User/Delete/5
        //public ActionResult Delete(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    UserMapModel userMapModel = db.UserMapModels.Find(id);
        //    if (userMapModel == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(userMapModel);
        //}

        //// POST: User/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(Guid id)
        //{
        //    UserMapModel userMapModel = db.UserMapModels.Find(id);
        //    db.UserMapModels.Remove(userMapModel);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
