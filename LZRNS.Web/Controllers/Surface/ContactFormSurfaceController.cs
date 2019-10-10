using System;
using Umbraco.Web.Mvc;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;
using Umbraco.Web;
using LZRNS.Models.DocumentTypes.Nodes.Items;

namespace LZRNS.Web.Controllers.Surface
{
	public class ContactFormSurfaceController : SurfaceController
	{
		public string GetViewPath(string name)
		{
			return $"/Views/Partials/Items/{name}.cshtml";
		}
		[HttpGet]
		public ActionResult RenderForm()
		{
			ContactFormViewModel model = new ContactFormViewModel();
			return PartialView(GetViewPath("_ContactUsForm"), model);
		}
		[HttpPost]
		public ActionResult RenderForm(ContactFormViewModel model)
		{
			return PartialView(GetViewPath("_ContactUsForm"), model);
		}

		[HttpPost]
		public ActionResult SubmitForm(ContactFormViewModel model)
		{
			bool success = false;
			if (ModelState.IsValid)
			{
				success = SendEmail(model);
			}
			return PartialView(GetViewPath(success ? "_ContactUsSuccessForm" : "_Error"));
		}

		public bool SendEmail(ContactFormViewModel model)
		{
			try
			{
				var colonisContactUs = Umbraco.TypedContentAtXPath("//AboutUs");
				
				string from = model.Email;
				string to = colonisContactUs.First().GetPropertyValue("emailAddress").ToString();
				string subject = "Poruka od " + model.Name;
				string body = "Posetilac " + model.Name + " (" + from + ") vam je poslao poruku:\n\n\"" + model.Message + "\"";

				MailMessage mail = new MailMessage();
				mail.To.Add(new MailAddress((string)to, from));
				mail.From = new MailAddress((string)to, from);
				mail.Body = body;
				mail.Subject = subject;

				SmtpClient client = new SmtpClient();

				client.Send(mail);

				return true;
			}
			catch (Exception ex)
			{
				Logger.Error(typeof(ContactFormSurfaceController), "An exception occured while sending mail", ex);

				return false;
			}
		}

	}
}