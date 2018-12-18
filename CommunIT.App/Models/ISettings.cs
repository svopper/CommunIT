using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunIT.App.Models
{
    public interface ISettings
    {
        Uri BackendUrl { get; }
        IReadOnlyCollection<string> Scopes { get; }
        string ClientId { get; }
        string TenantId { get; }
    }
}
