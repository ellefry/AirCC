using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AirCC.PortalUI.HttpHandlers
{
    public class CustomAuthorizationMessageHandler : DelegatingHandler
    {
        public CustomAuthorizationMessageHandler(HttpMessageHandler innerHandler=null)
        {
            InnerHandler = InnerHandler = innerHandler ?? new HttpClientHandler(); ;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if(request.RequestUri.ToString().Contains("Login?ReturnUrl"))
                throw new ApplicationException(request.RequestUri.ToString());
            var response = await base.SendAsync(request, cancellationToken);
            var status = response.StatusCode;
            if (status == HttpStatusCode.Redirect) throw new ApplicationException(status.ToString());
            Console.WriteLine(status);
            return response;
        }
    }

    public class ExampleHttpHandler : DelegatingHandler
    {
        public ExampleHttpHandler(HttpMessageHandler innerHandler)
        {
            //the last (inner) handler in the pipeline should be a "real" handler.
            //To make a HTTP request, create a HttpClientHandler instance.
            InnerHandler = innerHandler ?? new HttpClientHandler();
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            //add any logic here
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
