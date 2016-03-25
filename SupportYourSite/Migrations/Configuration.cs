namespace SupportYourSite.Migrations
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SupportYourSite.Models.SiteContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SupportYourSite.Models.SiteContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            context.Website.AddOrUpdate(
                  p => p.Name,
                  new Website
                  {
                      Name = "Tank Riot",
                      Type = SupportYourSite.Models.Type.Podcast,
                      URL = "http://www.tankriot.com",
                      iTunes = "https://itunes.apple.com/us/podcast/tank-riot/id84359875",
                      RSS = "http://www.tankriot.com/rss.xml",
                      SiteOwner = new SiteOwner { OwnerName = "Viktor, Tor, Sputnik", OwnerStatement = "", OwnerEmail = "feedback@tankriot.com", PayPalInfo = "" },
                      Description = "",
                      Categories = new List<Category>
                      {
                          new Category() { CategoryName = "Pop Culture" },
                          new Category() { CategoryName = "Miscellaneous" },
                      },
                      Comments = new List<Comment>
                      {
                          new Comment() { CommentEmail = "chad.hilke@gmail.com", CommentName = "Chad", CommentText = "So much fun!!", DatePosted = DateTime.Now },
                      }
                  },
                  new Website
                  {
                      Name = "Tangentially Speaking",
                      Type = SupportYourSite.Models.Type.Podcast,
                      URL = "http://chrisryanphd.com/tangentially-speaking/",
                      iTunes = "https://itunes.apple.com/us/podcast/tangentially-speaking-dr./id566908883",
                      RSS = "http://feeds.feedburner.com/TangentiallySpeaking-ChristopherRyanPhd",
                      SiteOwner = new SiteOwner { OwnerName = "Christopher Ryan", OwnerStatement = "", OwnerEmail = "chrisryanphd@gmail.com", PayPalInfo = "" },
                      Description = "",
                      Categories = new List<Category>
                      {
                        new Category() { CategoryName = "Interview" },
                        new Category() { CategoryName = "Travel" }
                      },
                      Comments = new List<Comment>
                      {
                          new Comment() { CommentEmail = "chad.hilke@gmail.com", CommentName = "Fu Sheng", CommentText = "I love this podcast!!", DatePosted = DateTime.Now },
                      }
                  },
                  new Website
                  {
                      Name = "The C-Realm",
                      Type = SupportYourSite.Models.Type.Podcast,
                      URL = "http://http://c-realm.com/",
                      iTunes = "https://itunes.apple.com/us/podcast/c-realm-podcast/id497263927",
                      RSS = "http://c-realm.com/feed/podcast/",
                      SiteOwner = new SiteOwner { OwnerName = "KMO", OwnerStatement = "", OwnerEmail = "kayemmo@c-realm.com", PayPalInfo = "" },
                      Description = "",
                      Categories = new List<Category>
                      {
                        new Category() { CategoryName = "Futurism" },
                        new Category() { CategoryName = "Interview" }
                      },
                      Comments = new List<Comment>
                      {
                          new Comment() { CommentEmail = "chad.hilke@gmail.com", CommentName = "Chad", CommentText = "Really makes me think.", DatePosted = DateTime.Now },
                          new Comment() { CommentEmail = "testuser@gmail.com", CommentName = "Lars", CommentText = "You should donate...", DatePosted = DateTime.Now },
                      }
                  }
              );
        }
    }
}
