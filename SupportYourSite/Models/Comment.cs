using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SupportYourSite.Models
{
    public class Comment
    {
        public int CommentID { get; set; }
        public string CommentText { get; set; }
        public string CommentName { get; set; }
        public string CommentEmail { get; set; }
        public DateTime DatePosted { get; set; }

        // Navigation property
        public int WebsiteID { get; set; }
        public virtual Website Website { get; set; }
    }
}