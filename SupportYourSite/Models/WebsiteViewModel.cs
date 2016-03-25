using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SupportYourSite.Models
{
    public class WebsiteViewModel
    {
        public WebsiteViewModel()
        {
            website = new Website();
            var donations = new List<Donation>();
        }
        public Website website { get; set; }

        public List<Donation> donations { get; set; }
    }
}