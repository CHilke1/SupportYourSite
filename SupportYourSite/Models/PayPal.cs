using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PayPal.Api;
using PayPal.Util;

//Authorization Endpoint
//https://www.sandbox.paypal.com/webapps/auth/protocol/openidconnect/v1/authorize

namespace SupportYourSite.Models
{
    public class PayPalPayment
    {
        public Payment PayWithCreditCard(Donation donation, CreditCard c, APIContext apiContext)
        {
            //create and item for which you are taking payment
            //if you need to add more items in the list
            //Then you will need to create multiple item objects or use some loop to instantiate object
            decimal amt = donation.Amount;

            Item item = new Item();
            item.name = "Donation";
            item.currency = "USD";
            item.price = amt.ToString();
            item.quantity = "1";
            item.sku = "sku";

            //Now make a List of Item and add the above item to it
            //you can create as many items as you want and add to this list
            List<Item> items = new List<Item>();
            items.Add(item);
            ItemList itemList = new ItemList();
            itemList.items = items;

            //Address for the payment
            Address billingAddress = new Address();
            billingAddress.city = donation.City;
            billingAddress.country_code = donation.Country;
            billingAddress.line1 = donation.Address1;
            billingAddress.postal_code = donation.Zipcode;
            billingAddress.state = donation.State;

            //Now Create an object of credit card and add above details to it
            //Please replace your credit card details over here which you got from paypal
            PayPal.Api.CreditCard creditCard = new PayPal.Api.CreditCard();

            creditCard.billing_address = billingAddress;
            creditCard.cvv2 = c.CVV.ToLower();  //card cvv2 number
            creditCard.expire_month = Convert.ToInt16(c.ExpirationMonth); //card expire date
            creditCard.expire_year = Convert.ToInt16(c.ExpirationYear); //card expire year
            creditCard.first_name = donation.FirstName;
            creditCard.last_name = donation.LastName;
            creditCard.number = c.CreditCardNumber; //enter your credit card number here
            string cardtype = c.CardType.ToString();
            creditCard.type = cardtype.ToLower(); //credit card type here paypal allows 4 types

            // Specify details of your payment amount.
            Details details = new Details();
            details.shipping = "0";
            details.subtotal = item.price;
            details.tax = "0";

            // Specify your total payment amount and assign the details object
            Amount amnt = new Amount();
            amnt.currency = "USD";
            // Total = shipping tax + subtotal.
            amnt.total = details.subtotal;
            amnt.details = details;

            // Now make a transaction object and assign the Amount object
            Transaction tran = new Transaction();
            tran.amount = amnt;
            tran.description = "creating a direct payment with credit card";
            tran.item_list = itemList;

            // Now, we have to make a list of transaction and add the transactions object
            // to this list. You can create one or more object as per your requirements

            List<Transaction> transactions = new List<Transaction>();
            transactions.Add(tran);

            // Now we need to specify the FundingInstrument of the Payer
            // for credit card payments, set the CreditCard which we made above

            FundingInstrument fundInstrument = new FundingInstrument();
            fundInstrument.credit_card = creditCard;

            // The Payment creation API requires a list of FundingIntrument

            List<FundingInstrument> fundingInstrumentList = new List<FundingInstrument>();
            fundingInstrumentList.Add(fundInstrument);

            // Now create Payer object and assign the fundinginstrument list to the object
            Payer payer = new Payer();
            payer.funding_instruments = fundingInstrumentList;
            payer.payment_method = "credit_card";

            // finally create the payment object and assign the payer object & transaction list to it
            Payment payment = new Payment();
            payment.intent = "sale";
            payment.payer = payer;
            payment.transactions = transactions;


            //getting context from the paypal
            //basically we are sending the clientID and clientSecret key in this function
            //to the get the context from the paypal API to make the payment
            //for which we have created the object above.

            //Basically, apiContext object has a accesstoken which is sent by the paypal
            //to authenticate the payment to facilitator account.
            //An access token could be an alphanumeric string

            //Create is a Payment class function which actually sends the payment details
            //to the paypal API for the payment. The function is passed with the ApiContext
            //which we received above.

            Payment createdPayment = payment.Create(apiContext);
            return createdPayment;
        }

        public Payment PayWithPayPal(Donation donation, string redirectUrlSuccess, string redirectUrlFailure, APIContext apiContext)
        {
            // ###Items
            // Items within a transaction.
            decimal amt = donation.Amount; // e.g. $100.00
            Item item = new Item();
            item.name = "Donation";
            item.currency = "USD";
            item.price = amt.ToString();
            item.quantity = "1"; // price * quantity must equal amount
            item.sku = "sku";

            List<Item> items = new List<Item>();
            items.Add(item);
            ItemList itemList = new ItemList();
            itemList.items = items;

            // ###Payer
            // A resource representing a Payer that funds a payment
            // Payment Method
            // as `paypal`
            Payer payer = new Payer();
            payer.payment_method = "paypal";
            Random rndm = new Random();
            var guid = Convert.ToString(rndm.Next(100000));

            // Configure Redirect Urls here with RedirectUrls object
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrlFailure + "guid=" + guid,
                return_url = redirectUrlSuccess + "guid=" + guid
            };

            // ###Details
            // Let's you specify details of a payment amount.
            Details details = new Details();
            details.tax = "0";
            details.shipping = "0";
            details.subtotal = item.price; // tax + shipping + subtotal must = amount

            // ###Amount
            // Let's you specify a payment amount.
            Amount amnt = new Amount();
            amnt.currency = "USD";
            // Total must be equal to sum of shipping, tax and subtotal.
            amnt.total = details.subtotal;
            amnt.details = details;

            // ###Transaction
            // A transaction defines the contract of a
            // payment - what is the payment for and who
            // is fulfilling it. 
            List<Transaction> transactionList = new List<Transaction>();
            Transaction tran = new Transaction();
            tran.description = "Donation";
            tran.amount = amnt;
            tran.item_list = itemList;
            // The Payment creation API requires a list of
            // Transaction; add the created `Transaction`
            // to a List
            transactionList.Add(tran);

            // ###Payment
            // A Payment Resource; create one using
            // the above types and intent as `sale` or `authorize`
            Payment payment = new Payment();
            payment.intent = "sale";
            payment.payer = payer;
            payment.transactions = transactionList;
            payment.redirect_urls = redirUrls;

            return payment.Create(apiContext);
        }
    }
}