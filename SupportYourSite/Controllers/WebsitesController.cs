using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SupportYourSite.Models;
using System.Xml;
using System.Text;
using System.Xml.Linq;
using System.ServiceModel.Syndication;

namespace SupportYourSite.Controllers
{
    public class WebsitesController : Controller
    {
        private SiteContext db = new SiteContext();

        // GET: Websites
        public ActionResult Index(int? Categories, int? type)
        {
            ViewBag.Categories = new SelectList(db.Category.ToList(), "CategoryID", "CategoryName");

            var website = db.Website.Include(w => w.SiteOwner).Include(w => w.Categories);

            if (Categories != null) { website = website.Where(c => c.Categories.Any(o => o.CategoryID == Categories)); }
            if (type != null) { website = website.Where(w => (int)w.Type == type); }
            var websiteList = new List<WebsiteIndexViewModel>();
            foreach (Website w in website)
            {
                var websiteIndexViewModel = new WebsiteIndexViewModel();
                websiteIndexViewModel.WebsiteId = w.WebsiteID;
                websiteIndexViewModel.Name = w.Name;
                foreach (Category c in w.Categories)
                {
                    websiteIndexViewModel.Categories.Add(c.CategoryName);
                }
                websiteIndexViewModel.Siteowner = w.SiteOwner.OwnerName;
                websiteIndexViewModel.URL = w.URL;
                websiteIndexViewModel.Description = w.Description;
                websiteIndexViewModel.Type = w.Type.ToString();
                websiteIndexViewModel.Image = w.Image;
                websiteList.Add(websiteIndexViewModel);
            }
            return View(websiteList);
        }
        
        // GET: Websites/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Website website = (from s in db.Website.Include("SiteOwner").Include("Comments")
                            where s.WebsiteID == id
                            select s).FirstOrDefault<Website>();
            //Website website = db.Website.Find(id);
            if (website == null)
            {
                return HttpNotFound();
            }

            List<Donation> donations = new List<Donation>();
            donations = (from d in db.Donation
                      where d.Website.WebsiteID == id
                      select d).ToList();
            //retrieve rss data
            string myRSS = website.RSS;
            XmlDocument doc = new XmlDocument();          
            try
            {
                doc.Load(myRSS);
                website.Description = doc.SelectSingleNode("//channel/description").InnerText;
                website.Image = doc.SelectSingleNode("//channel/image/url").InnerText;
            }
            catch
            {
                if (String.IsNullOrEmpty(website.Description))
                {
                    website.Description = "No Description Provided.";
                }
                website.Image = String.Empty;
            }

            var websiteViewModel = new WebsiteViewModel();
            websiteViewModel.website = website;
            websiteViewModel.donations = donations;

            ViewBag.Website = website.WebsiteID;
            return View(websiteViewModel);
        }

        // GET: Websites/Create
        public ActionResult Create()
        {
            ViewBag.WebsiteID = new SelectList(db.SiteOwners, "WebsiteID", "OwnerName");

            CreateWebsiteViewModel createViewModel = new CreateWebsiteViewModel();
            createViewModel.categories = db.Category.ToList();
            return View(createViewModel);
        }

        // POST: Websites/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateWebsiteViewModel websiteVM, string[] selectedCourses)
        {
            Website website = websiteVM.website;
            SiteOwner siteOwner = websiteVM.siteOwner;
            website.SiteOwner = siteOwner;
            siteOwner.Website = website;

            if (selectedCourses != null)
            {
                website.Categories = new List<Category>();
                foreach (var course in selectedCourses)
                {
                    var courseToAdd = db.Category.Find(int.Parse(course));
                    website.Categories.Add(courseToAdd);
                }
            }

            if (ModelState.IsValid)
            {  
                db.Website.Add(website);
                db.SiteOwners.Add(siteOwner);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.WebsiteID = new SelectList(db.SiteOwners, "WebsiteID", "OwnerName", website.WebsiteID);
            return View(website);
        }

        // GET: Websites/Edit/5
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
            PopulateAssignedCategoriesData(website);
            ViewBag.WebsiteID = new SelectList(db.SiteOwners, "WebsiteID", "OwnerName", website.WebsiteID);
            return View(website);
        }

        // POST: Websites/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "WebsiteID,Name,Type,URL,iTunes,RSS,Description,SiteOwnerID")] Website website)
        {
            if (ModelState.IsValid)
            {
                db.Entry(website).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.WebsiteID = new SelectList(db.SiteOwners, "WebsiteID", "OwnerName", website.WebsiteID);
            return View(website);
        }*/

        // POST: Websites/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Website website, string[] selectedCategories)
        {
            //if (ModelState.IsValid)
            //{
            //   return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            var websiteToUpdate = db.Website.Include(i => i.Categories).Include(i => i.SiteOwner).Where(i => i.WebsiteID == website.WebsiteID).Single();
            websiteToUpdate.SiteOwnerID = website.SiteOwnerID;
            UpdateInstructorCourses(selectedCategories, websiteToUpdate);
            db.Entry(websiteToUpdate).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
            //ViewBag.WebsiteID = new SelectList(db.SiteOwners, "WebsiteID", "OwnerName", website.WebsiteID);
            //return View(website);
        }

        private void UpdateInstructorCourses(string[] selectedCategories, Website websiteToUpdate)
        {
            if (selectedCategories == null)
            {
                websiteToUpdate.Categories = new List<Category>();
                return;
            }
            var selectedCoursesHS = new HashSet<string>(selectedCategories);
            var instructorCourses = new HashSet<int>(websiteToUpdate.Categories.Select(c => c.CategoryID));
            foreach (var category in db.Category)
            {
                if (selectedCoursesHS.Contains(category.CategoryID.ToString()))
                {
                    if (!instructorCourses.Contains(category.CategoryID))
                    {
                        websiteToUpdate.Categories.Add(category);
                    }
                }
                else
                {
                    if (instructorCourses.Contains(category.CategoryID))
                    {
                        websiteToUpdate.Categories.Remove(category);
                    }
                }
            }
        }

        // GET: Websites/Delete/5
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

        // POST: Websites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Website website = db.Website.Find(id);
            SiteOwner siteOwner = db.SiteOwners.Find(id);
            var comments = db.Comment.Where(c => c.WebsiteID == id);
            var donations = db.Donation.Where(c => c.WebsiteID == id);

            //ICollection<Comment> comments = (from Comment in db.Comment
            //                                where Comment.WebsiteID == id
            //                                select Comment).ToList();
            //ICollection<Donation> donations = (from Donation in db.Donation
             //                                where Donation.WebsiteID == id
            //                                 select Donation).ToList();

            db.SiteOwners.Remove(siteOwner);
            db.Website.Remove(website);
            db.Comment.RemoveRange(comments);
            db.Donation.RemoveRange(donations);

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Websites/Create
        [HttpGet]
        public ActionResult SiteContent(int? id)
        {
            RSSFeed rss = new RSSFeed();
            Website website = db.Website.Find(id);
            if (website == null)
            {
                return HttpNotFound();
            }

            XmlDocument rssXmlDoc = new XmlDocument();

            // Load the RSS file from the RSS URL
            rssXmlDoc.Load(website.RSS);

            // Parse the Items in the RSS file
            XmlNodeList rssNodes = rssXmlDoc.SelectNodes("rss/channel/item");

            // Iterate through the items in the RSS file
            foreach (XmlNode rssNode in rssNodes)
            {

                XmlNode rssSubNode = rssNode.SelectSingleNode("title");
                string title = rssSubNode != null ? rssSubNode.InnerText : "";


                rssSubNode = rssNode.SelectSingleNode("link");
                string link = rssSubNode != null ? rssSubNode.InnerText : "";


                rssSubNode = rssNode.SelectSingleNode("description");
                string description = rssSubNode != null ? rssSubNode.InnerText : "";

                Entry entry = new Entry
                {
                    title = title,
                    link = new HtmlString(link),
                    description = description,
                };

                rss.feeditems.Add(entry);
            }

            return View(rss);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private void PopulateCategoriesDropDownList(object selectedCategory = null) {
            var categoryQuery = from d in db.Category
                                   orderby d.CategoryName
                                   select d;
            ViewBag.CategoryID = new SelectList(categoryQuery, "CategoryID", "Name", selectedCategory);
        }

        // GET: Websites
        public ActionResult Index_old(string sortOrder, string searchString, int? Categories)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.TypeSortParm = sortOrder == "Type" ? "type_desc" : "Type";
            ViewBag.Categories = new SelectList(db.Category.ToList(), "CategoryID", "CategoryName");

            var website = db.Website.Include(w => w.SiteOwner);

            if (!String.IsNullOrEmpty(searchString)) { website = website.Where(s => s.Name.ToUpper().Contains(searchString.ToUpper()) || s.Description.ToUpper().Contains(searchString.ToUpper())); }
            if (Categories != null) { website = website.Where(c => c.Categories.Any(o => o.CategoryID == Categories)); }

            switch (sortOrder)
            {
                case "name_desc":
                    website = website.OrderByDescending(s => s.Name);
                    break;
                case "Type":
                    website = website.OrderBy(s => s.Type);
                    break;
                case "date_desc":
                    website = website.OrderByDescending(s => s.Type);
                    break;
                default:
                    website = website.OrderBy(s => s.Name);
                    break;
            }

            return View(website.ToList());
        }
        private void PopulateAssignedCategoriesData(Website website)
        {
            var allCategories = db.Category;
            var websiteCategories = new HashSet<int>(website.Categories.Select(c => c.CategoryID));
            var viewModel = new List<AssignedCategoriesData>();
            foreach (var category in allCategories)
            {
                viewModel.Add(new AssignedCategoriesData
                {
                    CategoryID = category.CategoryID,
                    Name = category.CategoryName,
                    Assigned = websiteCategories.Contains(category.CategoryID)
                });
            }
            ViewBag.Categories = viewModel;
        }
    }
}
