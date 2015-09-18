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
        private readonly IRoomsService roomsService;
        private readonly IPagesService pagesService;
        private readonly IAttractionsService attractionsService;
        private readonly IImagesService imagesService;

        public HomeController()
        {
            this.data = new UoWData();
            this.roomsService = new RoomsService(this.data);
            this.attractionsService = new AttractionsService(this.data);
            this.pagesService = new PagesService(this.data);
            this.imagesService = new ImagesService(this.data);
        }

        public ActionResult Index()
        {
            HomeViewModel model = new HomeViewModel();
            model.Attractions = this.attractionsService.GetAttractions().Take(2);
            model.RoomFeatures = this.roomsService.GetRoomFeatures().Take(4);
            model.FeaturedRooms = this.roomsService.GetRooms(null).Take(3);
            int featuredCustomPageId = int.Parse(ConfigurationManager.AppSettings["FeaturedCustomPageId"]);
            model.FeaturedCustomPage = this.pagesService.GetFeaturedCustomPage(featuredCustomPageId);

            if (this.data.Images.All().Any())
            {
                model.GalleryImages = this.imagesService.GetRandomRoomImages();
            }
            

            return View(model);
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