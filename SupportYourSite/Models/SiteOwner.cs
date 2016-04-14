using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SupportYourSite.Models
{
    public class SiteOwner
    {
        [Key, ForeignKey("Website")]
        public int WebsiteID { get; set; }
        // Owner info
        [MaxLength(100), MinLength(2)]
        public string OwnerName { get; set; }
        [Display(Name = "Email address")]
        public string OwnerEmail { get; set; }
        public string PayPalInfo { get; set; }
        public string OwnerStatement { get; set; }
        public string OwnerBiogrpahy { get; set; }
        public virtual Website Website { get; set; }

    }
}