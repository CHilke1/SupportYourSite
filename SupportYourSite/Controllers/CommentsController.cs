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
    public class CommentsController : Controller
    {
        private SiteContext db = new SiteContext();

        // GET: Comments
        public ActionResult Index()
        {
            var comment = db.Comment.Include(c => c.Website);
            return View(comment.ToList());
        }

        // GET: Comments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comment.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Comments/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //ViewBag.WebsiteID = new SelectList(db.Website, "WebsiteID", "Name");
            ViewBag.WebsiteID = id;
            var sitename = (from Website in db.Website
                                            where Website.WebsiteID == id
                                            select Website.Name).FirstOrDefault();
            ViewBag.WebsiteName = sitename.ToString();
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CommentID,CommentText,CommentName,CommentEmail,DatePosted,WebsiteID")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                DateTime now = DateTime.Now;
                comment.DatePosted = now;
                db.Comment.Add(comment);
                db.SaveChanges();
                return RedirectToAction("../Websites/Details/" + comment.WebsiteID);
            }

            ViewBag.WebsiteID = new SelectList(db.Website, "WebsiteID", "Name", comment.WebsiteID);
            return View(comment);
        }

        // GET: Comments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comment.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.WebsiteID = new SelectList(db.Website, "WebsiteID", "Name", comment.WebsiteID);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CommentID,CommentText,CommentName,CommentEmail,DatePosted,WebsiteID")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.WebsiteID = new SelectList(db.Website, "WebsiteID", "Name", comment.WebsiteID);
            return View(comment);
        }

        // GET: Comments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comment.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comment.Find(id);
            db.Comment.Remove(comment);
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
