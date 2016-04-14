using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SupportYourSite.Models;

namespace SupportYourSite.Controllers
{
    public class Websites1Controller : Controller
    {
        private SiteContext db = new SiteContext();

        // GET: Websites1
        public ActionResult Index()
        {
            var website = db.Website.Include(w => w.SiteOwner);
            return View(website.ToList());
        }

        // GET: Websites1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Website website = db.Website.Find(id);
            if (website == null)
            {
                return HttpNotFound();
            }
            return View(website);
        }

        // GET: Websites1/Create
        public ActionResult Create()
        {
            ViewBag.WebsiteID = new SelectList(db.SiteOwners, "WebsiteID", "OwnerName");
            return View();
        }

        // POST: Websites1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WebsiteID,Name,Type,URL,iTunes,RSS,Description,Image,SiteOwnerID")] Website website)
        {
            if (ModelState.IsValid)
            {
                db.Website.Add(website);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.WebsiteID = new SelectList(db.SiteOwners, "WebsiteID", "OwnerName", website.WebsiteID);
            return View(website);
        }

        // GET: Websites1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Website website = db.Website.Find(id);
            if (website == null)
            {
                return HttpNotFound();
            }
            ViewBag.WebsiteID = new SelectList(db.SiteOwners, "WebsiteID", "OwnerName", website.WebsiteID);
            return View(website);
        }

        // POST: Websites1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "WebsiteID,Name,Type,URL,iTunes,RSS,Description,Image,SiteOwnerID")] Website website)
        {
            if (ModelState.IsValid)
            {
                db.Entry(website).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.WebsiteID = new SelectList(db.SiteOwners, "WebsiteID", "OwnerName", website.WebsiteID);
            return View(website);
        }

        // GET: Websites1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Website website = db.Website.Find(id);
            if (website == null)
            {
                return HttpNotFound();
            }
            return View(website);
        }

        // POST: Websites1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Website website = db.Website.Find(id);
            db.Website.Remove(website);
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
