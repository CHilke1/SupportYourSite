using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SupportYourSite.Models
{
    public enum CardType
    {
        MasterCard,
        Visa,
    }

    public class CreditCard
    {
        //Credit Card Info - not stored
        [CreditCard]
        public string CreditCardNumber { get; set; }

        public CardType CardType { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM}", ApplyFormatInEditMode = true)]
        public string ExpirationMonth { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy}", ApplyFormatInEditMode = true)]
        public string ExpirationYear { get; set; }

        [MaxLength(3), MinLength(1)]
        public string CVV { get; set; }
    }
}