using System;
using System.Collections.Generic;
using System.Text;
using AirCC.Client;
using BCI.Extensions.IdentityClient.Token;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace AirCCClient.Web.Apis
{
    public class UpdateAuthorizationService : IUpdateAuthorizationService
    {
        private readonly IJwtTokenHandler jwtTokenHandler;
        private readonly AirCCConfigOptions airCcConfigOptions;

        public UpdateAuthorizationService(IJwtTokenHandler jwtTokenHandler, AirCCConfigOptions airCcConfigOptions)
        {
            this.jwtTokenHandler = jwtTokenHandler;
            this.airCcConfigOptions = airCcConfigOptions;
        }

        public bool Validate([NotNull]string accessToken)
        {
            try
            {
                var tokenParameters = new TokenParameters
                {
                    SigningKey = airCcConfigOptions.ApplicationSecret,
                    Issuer = airCcConfigOptions.ApplicationId,
                    Audience = "AirCC"
                };
                jwtTokenHandler.ValidateJwtToken(accessToken, tokenParameters);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
