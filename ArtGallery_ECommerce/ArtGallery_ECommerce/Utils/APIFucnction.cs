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
        public static void Send(string Subject, string To, string Body)
        {
            RestClient client = new RestClient();
            client.BaseUrl = new Uri("https://api.mailgun.net/v2");
            client.Authenticator = new HttpBasicAuthenticator("api", PrivateAPIKeys.MailgunAPI);
            RestRequest request = new RestRequest();
            request.Resource = "sandboxd9f10ded9b9c4aad924d6213d5c8263b.mailgun.org";
            request.AddParameter("from", "Painted Plunders LLC - paintedplunders.com");
            request.AddParameter("to", To);
            request.AddParameter("subject", Subject);
            request.AddParameter("text", Body);
            request.Method = Method.POST;
            client.Execute(request);
        }
    }
}