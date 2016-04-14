using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SupportYourSite.Models
{
    public class AssignedCategoriesData
    {
        public int CategoryID { get; set; }
        public string Name { get; set; }
        public bool Assigned { get; set; }
    }
}