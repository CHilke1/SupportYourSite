using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SupportYourSite.Models
{
    public class CreateWebsiteViewModel
    {
        public CreateWebsiteViewModel()
        {
            var website = new Website();
            var siteOwner = new SiteOwner();
            var categories = new List<Category>();
        }
        public SiteOwner siteOwner { get; set; }
        public Website website { get; set; }
        public ICollection<Category> categories { get; set; }
    }
}