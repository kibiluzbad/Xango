using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Blog.UI.Infra.AutoMapper;
using Xango.Data.NHibernate.Configuration;
using Xango.Mvc.Extensions;

namespace Blog.UI
{
    public class MvcApplication : HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "PostsPaged",
                "posts/{itemsPerPage}/{pageNumber}",
                new { controller = "Posts", action = "Index" },
                new { itemsPerPage = "\\d+", pageNumber = "\\d+", httpMethod = new HttpMethodConstraint("GET") }
                );
            
            routes.Root("Posts");

            routes.MapRoute(
                "PostPaginated", // Route name
                "Posts/{itemsPerPage}/{pageNumber}", // URL with parameters
                new {controller = "Posts", action = "Index"},
                new
                    {
                        itemsPerPage = @"\d+",
                        pageNumber = @"\d+",
                        httpMethod = new HttpMethodConstraint("GET")
                    });

            routes.MapRoute(
                "CommentPost", // Route name
                "posts/{slug}/comment", // URL with parameters
                new {controller = "Posts", action = "Comment"}, // Parameter defaults
                new {slug = @"[\w\-]+"}
                );

            routes.MapRoute(
                "RecentPosts", // Route name
                "RecentPosts", // URL with parameters
                new {controller = "Posts", action = "Last"},
                new {httpMethod = new HttpMethodConstraint("GET")}
                );

            routes.Resource("Posts");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new {controller = "Home", action = "Index", id = UrlParameter.Optional} // Parameter defaults
                );
        }

        protected void Application_Start()
        {
            
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            AutoMapperConfiguration.Configure();
        }
    }
}