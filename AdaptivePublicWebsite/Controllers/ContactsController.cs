using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using FamtasticPublicWebsite.DataAccess.EntityFramework;
using FamtasticPublicWebsite.Business;
using System.Diagnostics;

namespace FamtasticAdminWebsite.Controllers
{
    public class ContactsController : Controller
    {
        private FamtasticPublicEntities db = new FamtasticPublicEntities();

		[Route("Create")]
		[AcceptVerbs(HttpVerbs.Post)]
		[HttpPost, ValidateInput(false)]
		//[ValidateAntiForgeryToken]
		public JsonResult Create([Bind(Exclude = "Subject,CreateDate")] Contact contact, string sendOption)
        {
            if (ModelState.IsValid)
            {
				contact.CreateDate = DateTime.Now;
				contact.Subject = "Website Contact from " + contact.Name;
                db.Contacts.Add(contact);
                db.SaveChanges();

				//Send an email on training form submit
				try
				{
					MailAddress mailAddress = new MailAddress(contact.Email);
					MailMessage message = new MailMessage();
					message.IsBodyHtml = true;
					message.Subject = contact.Subject;
					message.Body = contact.Message;
					EmailHelper.SendUserMessage(mailAddress, message, sendOption);
				}
				catch (Exception ex)
				{
					using (EventLog eventLog = new EventLog("Application"))
					{
						eventLog.Source = "Public Website";
						eventLog.WriteEntry("Error sending email in Contacts " + ex.InnerException + " Stack Trace: " + ex.StackTrace, EventLogEntryType.Error);
					}
				}

				var jsonResult = Json(contact);
				return jsonResult;
			}

			return Json(null);
		}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
