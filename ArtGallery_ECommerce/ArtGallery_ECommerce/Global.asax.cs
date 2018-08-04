using ArtGallery_ECommerce.Utils;
using Stripe;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ArtGallery_ECommerce
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Need to access apikey from ignored file
            // PrivateAPIKeys sk = new PrivateAPIKeys();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            StripeConfiguration.SetApiKey(ConfigurationManager.AppSettings["sk_test_ZBkSBCi6RvtmMyb2pzL22DgQ"]);
        }
    }
}
