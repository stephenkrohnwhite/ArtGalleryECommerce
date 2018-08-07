using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtGallery_ECommerce.Utils
{
    public static class APIFucnction
    {

        public static IRestResponse SendSimpleMessage(string subject, string to, string body)
        {
            RestClient client = new RestClient();
            client.BaseUrl = new Uri("https://api.mailgun.net/v3");
            client.Authenticator =
                new HttpBasicAuthenticator("api",
                                            PrivateAPIKeys.MailgunAPI);
            RestRequest request = new RestRequest();
            request.AddParameter("domain", "sandboxd9f10ded9b9c4aad924d6213d5c8263b.mailgun.org", ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from", "mailgun@sandboxd9f10ded9b9c4aad924d6213d5c8263b.mailgun.org");
            request.AddParameter("to", to);
            request.AddParameter("subject", subject);
            request.AddParameter("text", body);
            request.Method = Method.POST;
            return client.Execute(request);
        }
    }
}