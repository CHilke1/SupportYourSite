using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SupportYourSite.Models
{
    public class EditWebsiteViewModel
    {
        public EditWebsiteViewModel()
        {
            var website = new Website();
            var siteOwner = new SiteOwner();
            var categories = new List<Category>();
        }
        public bool isCategory;
        public SiteOwner siteOwner { get; set; }
        public Website website { get; set; }
        public ICollection<Category> categories { get; set; }
    }
}