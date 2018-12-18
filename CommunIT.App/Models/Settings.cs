using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunIT.App.Models
{
    public class Settings : ISettings
    {
        public Uri BackendUrl => new Uri("https://localhost:44399/");

        public IReadOnlyCollection<string> Scopes => new[] { "6103c652-7ca8-4653-9685-141ea51e6d7e/user_impersonation" }; // API client ID

        //public IReadOnlyCollection<string> Scopes => new[] { "2c6f6059-a61a-4bb6-8c77-adab10973cd6/user_impersonation" }; // API client ID

        public string ClientId => "2c6f6059-a61a-4bb6-8c77-adab10973cd6"; // App client ID
        

        public string TenantId => "bea229b6-7a08-4086-b44c-71f57f716bdb";

        public string Authority => $"https://login.microsoftonline.com/{TenantId}/v2.0/";
    }
}
