using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;

namespace FamtasticPublicWebsite.Business
{
	public class EmailHelper
	{
		public static void SendUserMessage(MailAddress emailAddress, MailMessage message, string sendOption)
		{
			var regex = new Regex(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");
			var emailClean = regex.IsMatch(emailAddress.Address);

			if (emailClean)
			{
				var smtpServerUrl = ConfigurationManager.AppSettings.Get("SmtpServerUrl");
				var smtpServerPort = ConfigurationManager.AppSettings.Get("SmtpServerPort");

				MailAddress emailFrom = new MailAddress(emailAddress.Address);
				MailAddress emailTo;
				if (sendOption == "hr")
				{
					emailTo = new MailAddress(ConfigurationManager.AppSettings.Get("ToHrAddress"));
				}
				else
				{
					emailTo = new MailAddress(ConfigurationManager.AppSettings.Get("ToSalesAddress"));
				}
				
				var password = ConfigurationManager.AppSettings.Get("Password");
				var smtp = new SmtpClient(smtpServerUrl);
				smtp.Port = Convert.ToInt32(smtpServerPort, CultureInfo.InvariantCulture);
				smtp.Credentials = new NetworkCredential(emailTo.Address, password);
				
				var emailMessage = new MailMessage
				{
					From = emailFrom,
					Subject = message.Subject,
					To = { emailTo },
					Body = message.Body,
					IsBodyHtml = true
				};

				smtp.Send(emailMessage);
			}
		}
	}
}