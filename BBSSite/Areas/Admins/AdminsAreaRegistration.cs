using System.Web.Mvc;

namespace BBSSite.Areas.Admins
{
    public class AdminsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admins";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                name: "Admins_default",
                url: "Admins/{controller}/{action}/{id}",
                defaults: new { controller = "Users", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "BBSSite.Areas.Admins.Controllers" }
            );

            context.MapRoute(
                name: "Admins_default1",
                url: "Admins/{controller}/{action}/{limit}/{offset}/{txt_search}",
                defaults: new { controller = "Users", action = "UserList", limit = UrlParameter.Optional, offset = UrlParameter.Optional, txt_search = UrlParameter.Optional },
                namespaces: new string[] { "BBSSite.Areas.Admins.Controllers" }
            );
        }
    }
}