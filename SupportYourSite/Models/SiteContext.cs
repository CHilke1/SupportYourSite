using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SupportYourSite.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Data.Entity.ModelConfiguration.Conventions;
    public class SiteContext : DbContext
    {
        public SiteContext() : base("DefaultConnection")
        {
        }

        public DbSet<Comment> Comment { get; set; }
        public DbSet<Donation> Donation { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Website> Website { get; set; }
        //public DbSet<SiteOwner> SiteOwner { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public System.Data.Entity.DbSet<SupportYourSite.Models.SiteOwner> SiteOwners { get; set; }
    }
}