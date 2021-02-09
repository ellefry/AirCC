using System;
using System.Collections.Generic;
using System.Text;

namespace AirCCClient.Web.Apis
{
    public interface IUpdateAuthorizationService
    {
        bool Validate(string accessToken);
    }
}
