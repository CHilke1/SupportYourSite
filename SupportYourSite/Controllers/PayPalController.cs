using PayPal.Api;
using SupportYourSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SupportYourSite.Controllers
{
    public class PayPalController : Controller
    {
        private SiteContext db = new SiteContext();

        // GET: PayPal - Credit Card
        public ActionResult PayWithCreditCard(int? id)
        {
            DonationViewModel donationViewModel = new DonationViewModel();
            Website website = db.Website.Find(id);
            donationViewModel.website = website;
            return View(donationViewModel);
        }

        // GET: PayPal - PayPal
        public ActionResult PayWithPayPal(int? id)
        {
            DonationViewModel donationViewModel = new DonationViewModel();
            Website website = db.Website.Find(id);
            donationViewModel.website = website;
            return View(donationViewModel);
        }

        // GET: PayPal - Credit Card
        public ActionResult SuccessView()
        {
            int id = Convert.ToInt16(Request.Params["WebsiteID"]);
            Website website = db.Website.Find(id);
            return View(website);
            //return View();
        }

        // GET: PayPal - PayPal
        public ActionResult CreditCardSuccessView(int? id)
        {
            Website website = db.Website.Find(id);
            return View(website);
            //return View();
        }

        public ActionResult FailureView()
        {
            return View();
        }


        // GET: PayPal
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PaymentWithPayPal()
        {
            return View();
        }

        // GET: PayPal/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PayPal/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PayPal/Create
        [HttpPost]
        public ActionResult PayWithPayPal(DonationViewModel donationViewModel)
        {
            PayPal.Api.Payment createdPayment = null;
            if (ModelState.IsValid)
            {
                PayPal.Api.Payment payment;
                Donation donation = donationViewModel.donation;
                Website website = donationViewModel.website;
                DateTime now = DateTime.Now;
                donation.Date = now;
                donation.WebsiteID = website.WebsiteID;

                try
                {
                    // Get a reference to the config
                    var config = ConfigManager.Instance.GetProperties();

                    // Use OAuthTokenCredential to request an access token from PayPal
                    var accessToken = new OAuthTokenCredential(config).GetAccessToken();

                    var apiContext = new APIContext(accessToken);
                    apiContext.Config = ConfigManager.Instance.GetProperties();

                    // Define any HTTP headers to be used in HTTP requests made with this APIContext object
                    if (apiContext.HTTPHeaders == null)
                    {
                        apiContext.HTTPHeaders = new Dictionary<string, string>();
                    }
                    apiContext.HTTPHeaders["website"] = website.Name;
                    apiContext.HTTPHeaders["amount"] = donation.Amount.ToString();

                    string payerId = Request.Params["PayerID"];

                    if (string.IsNullOrEmpty(payerId))
                    {
                        string baseURISuccess = Request.Url.Scheme + "://" + Request.Url.Authority + "/Paypal/SuccessView?WebsiteID=" + website.WebsiteID + "&";
                        string baseURIFailure = Request.Url.Scheme + "://" + Request.Url.Authority + "/Paypal/FailureView?WebsiteID=" + website.WebsiteID + "&";
                        var guid = Convert.ToString((new Random()).Next(100000));
                        
                        PayPalPayment paypal = new PayPalPayment();
                        createdPayment = paypal.PayWithPayPal(donation, baseURISuccess, baseURIFailure, apiContext);

                        string paypalRedirectUrl = null;
                        var links = createdPayment.links.GetEnumerator();
                        while (links.MoveNext())
                        {
                            Links lnk = links.Current;

                            if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                            {
                                //saving the payapalredirect URL to which user will be redirected for payment
                                paypalRedirectUrl = lnk.href;
                            }
                        }

                        // saving the paymentID in the key guid
                        db.Donation.Add(donation);
                        db.SaveChanges();
                        Session.Add(guid, createdPayment.id);
                        return Redirect(paypalRedirectUrl);
                    }
                    else
                    {
                        var guid = Request.Params["guid"];

                        var paymentExecution = new PaymentExecution();
                        paymentExecution.payer_id = payerId;

                        payment = new Payment() { id = Session[guid] as string };
                        var executedPayment = payment.Execute(apiContext, paymentExecution);

                        if (executedPayment.state.ToLower() != "approved")
                        {
                            return View("FailureView");
                        }
                        db.Donation.Add(donation);
                        db.SaveChanges();
                        return View("SuccessView", executedPayment);
                    }
                }
                catch(Exception e)
                {
                    string error = e.ToString();
                    return View("FailureView");
                }
            }
            return View();
        }

        // POST: PayPal/Create
        [HttpPost]
        public ActionResult PayWithCreditCard(DonationViewModel donationViewModel)
        {
            if (ModelState.IsValid)
            {
                Donation donation = donationViewModel.donation;
                Website website = donationViewModel.website;
                
                Models.CreditCard creditCard = donationViewModel.creditcard;
                PayPal.Api.Payment createdPayment = null;
                PayPalPayment paypal = new PayPalPayment();
                try
                {
                    // Get a reference to the config
                    var config = ConfigManager.Instance.GetProperties();

                    // Use OAuthTokenCredential to request an access token from PayPal
                    var accessToken = new OAuthTokenCredential(config).GetAccessToken();

                    var apiContext = new APIContext(accessToken);
                    apiContext.Config = ConfigManager.Instance.GetProperties();

                    // Define any HTTP headers to be used in HTTP requests made with this APIContext object
                    //if (apiContext.HTTPHeaders == null)
                    //{
                    //    apiContext.HTTPHeaders = new Dictionary<string, string>();
                    //}
                    //apiContext.HTTPHeaders["app-test"] = "supportyoursite-test";

                    // return created payment
                    createdPayment = paypal.PayWithCreditCard(donation, creditCard, apiContext);

                    // check if approved
                    if (createdPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                }
                catch (Exception e)
                {
                    string error = e.ToString();
                    return View("FailureView");
                }

                donation.WebsiteID = website.WebsiteID;
                DateTime now = DateTime.Now;
                donation.Date = now;
                db.Donation.Add(donation);         
                db.SaveChanges();
                return View("CreditCardSuccessView", createdPayment);
            }
            return View();
        }

        // GET: PayPal/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PayPal/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: PayPal/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PayPal/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        /*private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            this.payment = new Payment() { id = paymentId };
            return this.payment.Execute(apiContext, paymentExecution);
        }*/
    }
}
