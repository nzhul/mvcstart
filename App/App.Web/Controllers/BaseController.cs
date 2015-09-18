using App.Data;
using App.Data.Service;
using App.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Web.Controllers
{
    public class BaseController : Controller
    {
        private readonly IUoWData data;
        private readonly IRoomsService roomsService;
        private readonly IAttractionsService attractionsService;
        private readonly IPagesService pagesService;

        public BaseController()
        {
            this.data = new UoWData();
            this.roomsService = new RoomsService(this.data);
            this.attractionsService = new AttractionsService(this.data);
            this.pagesService = new PagesService(this.data);

            LayoutModel model = new LayoutModel();
            model.RoomCategories = this.roomsService.GetRoomCategories();
            model.Attractions = this.attractionsService.GetAttractions().Take(5);
            model.Pages = this.pagesService.GetPages();

            ViewBag.LayoutModel = model;
        }
    }
}