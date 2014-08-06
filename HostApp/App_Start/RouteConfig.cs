using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcCodeRouting;
using MvcContact;

namespace HostApp {
   
   public class RouteConfig {

      public static void RegisterRoutes(RouteCollection routes) {

         routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

         routes.MapCodeRoutes(typeof(Controllers.HomeController));

         var commonConfig = new ContactConfiguration { 
            From = "noreply@example.com"
         };

         routes.MapCodeRoutes(
            baseRoute: "Contact",
            rootController: typeof(ContactController),
            settings: new CodeRoutingSettings {
               EnableEmbeddedViews = true,
               Configuration = new ContactConfiguration(commonConfig) {
                  To = "contact@example.com"
               }
            }
         );

         routes.MapCodeRoutes(
            baseRoute: "CustomContact",
            rootController: typeof(ContactController),
            settings: new CodeRoutingSettings {
               EnableEmbeddedViews = true,
               Configuration = new ContactConfiguration(commonConfig) {
                  To = "info@example.com",
                  ContactSenderResolver = () => new Models.CustomContactSender()
               }
            }
         );
      }
   }
}
