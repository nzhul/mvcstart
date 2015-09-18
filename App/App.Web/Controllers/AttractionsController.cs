using App.Data;
using App.Data.Service;
using App.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Web.Controllers
{
    public class AttractionsController : BaseController
    {
        private readonly IUoWData data;
        private readonly IAttractionsService attractionsService;

        public AttractionsController()
        {
            this.data = new UoWData();
            this.attractionsService = new AttractionsService(this.data);
        }

        [HttpGet]
        public ActionResult Index()
        {
            IEnumerable<AttractionViewModel> model = new List<AttractionViewModel>();
            model = this.attractionsService.GetAttractions();
            return View(model);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            AttractionViewModel model = this.attractionsService.GetAttractionById(id);
            return View(model);
        }
    }
}