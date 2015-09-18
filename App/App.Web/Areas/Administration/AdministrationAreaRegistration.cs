using System.Web.Mvc;

namespace App.Web.Areas.Administration
{
    public class AdministrationAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Administration";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Administration_default",
                "Administration/{controller}/{action}/{id}",
                defaults: new { controller = "Reservations", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}