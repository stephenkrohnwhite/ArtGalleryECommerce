using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtGallery_ECommerce.Utils
{
    public static class PrivateAPIKeys
    {
        private static string stripeAPISK = "sk_test_ZBkSBCi6RvtmMyb2pzL22DgQ";
        public static string StripeAPISK
        {
            get
            {
                return stripeAPISK;
            }
        }
        private static string mailgunAPI = "7efe8d73-8e94d86a";
        public static string MailgunAPI
        {
            get
            {
                return mailgunAPI;
            }
        }
    }
}