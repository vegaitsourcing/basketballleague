using Umbraco.Web.Mvc;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
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
                var toAddress = colonisContactUs.First().GetPropertyValue("emailAddress");

                MailMessage messeage = new MailMessage();
                SmtpClient client = new SmtpClient();

                string fromAddress = model.Email;
                messeage.Subject = $"Enquiry from {model.Name} - {model.Email}";
                messeage.Body = "The Get In Touch form was submitted on the Contact Us page:" + "\n\n" + "Name: " + model.Name + "\n" + "Email: " + model.Email + "\n" + "Text: " + model.Message;
                messeage.To.Add(new MailAddress((string)toAddress, fromAddress));
                messeage.From = new MailAddress((string)toAddress, fromAddress);
                client.Send(messeage);
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

    }
}