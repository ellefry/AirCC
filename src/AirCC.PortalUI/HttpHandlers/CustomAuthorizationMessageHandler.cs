using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AirCC.PortalUI.HttpHandlers
{
    public class CustomAuthorizationMessageHandler : DelegatingHandler
    {
        private readonly NavigationManager navManager;
        public CustomAuthorizationMessageHandler(NavigationManager navManager, HttpMessageHandler innerHandler=null)
        {
            InnerHandler = InnerHandler = innerHandler ?? new HttpClientHandler();
            this.navManager = navManager;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);
            var status = response.StatusCode;
            if (status == HttpStatusCode.Unauthorized) //throw new ApplicationException(status.ToString());
            {
                navManager.NavigateTo("login");
            }
            return response;
        }
    }

}
