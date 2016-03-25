using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SupportYourSite.Models
{
    public class DonationViewModel
    {
        public DonationViewModel()
        {
            website = new Website();
            donation = new Donation();
            creditcard = new CreditCard();
        }
        public Website website { get; set; }

        public Donation donation { get; set; }

        public CreditCard creditcard { get; set; }
    }
}