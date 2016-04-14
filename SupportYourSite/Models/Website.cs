using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SupportYourSite.Models
{
    public enum Type
    {
        Blog,
        Podcast,
        Artist,
        Other,
    }
    public class Website
    {
        public Website()
        {
            Categories = new HashSet<Category>();
            Comments = new HashSet<Comment>();
        }
        public int WebsiteID { get; set; }
        public string Name { get; set; }
        public Type Type { get; set; }
        public string URL { get; set; }
        public string iTunes { get; set; }
        public string RSS { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int SiteOwnerID { get; set; }
        public SiteOwner SiteOwner { get; set; }

        // Navigation properties

        // Topics
        public virtual ICollection<Category> Categories { get; set; }

        // Comments
        public virtual ICollection<Comment> Comments { get; set; }
    }
}