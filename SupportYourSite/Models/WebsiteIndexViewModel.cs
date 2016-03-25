using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SupportYourSite.Models
{
    public class WebsiteIndexViewModel
    {
        public WebsiteIndexViewModel() {
            Categories = new HashSet<String>();
        }


        public int WebsiteId { get; set; }


        public string Name { get; set; }


        public string Type { get; set; }

        public string Description { get; set; }

        public string URL { get; set; }

        public string Image { get; set; }

        public string Siteowner { get; set; }

        public ICollection<String> Categories { get; set; }
    }
}