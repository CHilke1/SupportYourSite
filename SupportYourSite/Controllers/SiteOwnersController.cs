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
    public class SiteOwnersController : Controller
    {
        private SiteContext db = new SiteContext();

        // GET: SiteOwners
        public ActionResult Index()
        {
            var siteOwners = db.SiteOwners.Include(s => s.Website);
            return View(siteOwners.ToList());
        }

        // GET: SiteOwners/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SiteOwner siteOwner = db.SiteOwners.Find(id);
            if (siteOwner == null)
            {
                return HttpNotFound();
            }
            return View(siteOwner);
        }

        // GET: SiteOwners/Create
        public ActionResult Create()
        {
            ViewBag.WebsiteID = new SelectList(db.Website, "WebsiteID", "Name");
            return View();
        }

        // POST: SiteOwners/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WebsiteID,OwnerName,OwnerEmail,PayPalInfo,OwnerStatement,OwnerBiogrpahy")] SiteOwner siteOwner)
        {
            if (ModelState.IsValid)
            {
                db.SiteOwners.Add(siteOwner);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.WebsiteID = new SelectList(db.Website, "WebsiteID", "Name", siteOwner.WebsiteID);
            return View(siteOwner);
        }

        // GET: SiteOwners/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SiteOwner siteOwner = db.SiteOwners.Find(id);
            if (siteOwner == null)
            {
                return HttpNotFound();
            }
            ViewBag.WebsiteID = new SelectList(db.Website, "WebsiteID", "Name", siteOwner.WebsiteID);
            return View(siteOwner);
        }

        // POST: SiteOwners/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "WebsiteID,OwnerName,OwnerEmail,PayPalInfo,OwnerStatement,OwnerBiogrpahy")] SiteOwner siteOwner)
        {
            if (ModelState.IsValid)
            {
                db.Entry(siteOwner).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.WebsiteID = new SelectList(db.Website, "WebsiteID", "Name", siteOwner.WebsiteID);
            return View(siteOwner);
        }

        // GET: SiteOwners/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SiteOwner siteOwner = db.SiteOwners.Find(id);
            if (siteOwner == null)
            {
                return HttpNotFound();
            }
            return View(siteOwner);
        }

        // POST: SiteOwners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SiteOwner siteOwner = db.SiteOwners.Find(id);
            db.SiteOwners.Remove(siteOwner);
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
