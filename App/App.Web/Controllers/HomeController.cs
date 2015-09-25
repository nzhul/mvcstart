using App.Data;
using App.Data.Service;
using App.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace App.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IUoWData data;
        private readonly IItemsService roomsService;
        private readonly IPagesService pagesService;
        private readonly IArticlesService attractionsService;
        private readonly IImagesService imagesService;

        public HomeController()
        {
            this.data = new UoWData();
            this.roomsService = new ItemsService(this.data);
            this.attractionsService = new ArticlesService(this.data);
            this.pagesService = new PagesService(this.data);
            this.imagesService = new ImagesService(this.data);
        }

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Contact()
        {
            var model = new ContactFormInputModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Contact(ContactFormInputModel contactData)
        {
            if (ModelState.IsValid)
            {
                string sender = ConfigurationManager.AppSettings["emailSender"];
                string receiver = ConfigurationManager.AppSettings["emailReceiver"];

                MailMessage mailMessage = new MailMessage(sender, receiver);
                mailMessage.IsBodyHtml = true;
                mailMessage.Subject = "Запитване (контактна форма): ";
                mailMessage.Body = "Имена: " + contactData.Name + "<br/>" +
                                   "Email: " + contactData.Email + "<br/>" +
                                   "Телефон: " + contactData.Phone + "<br/><br/>" +
                                   "Запитване: <br/>" + contactData.Content;

                SmtpClient smtpClient = new SmtpClient();

                // The settings are in web.config file
                smtpClient.Send(mailMessage);

                return Content(@"<div class='alert alert-dismissable alert-success'>
                            <button type='button' class='close' data-dismiss='alert'>×</button>
                            <strong>Съобщението беше изпратено успешно!</strong>
                        </div>");
            }
            else
            {
                return View("Contact", contactData);
            }
        }
    }
}