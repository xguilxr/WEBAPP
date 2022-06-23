using System;
using System.Collections.Generic;
using System.Text;

namespace Bat.PortalDeCargas.Domain.Configuration
{
    public interface IAppConfiguration
    {        
        int ClientsPerPageFilter { get; }
        string Secret { get; }
        
        string BlobConnectionString { get; }
        string BlobUri { get; set; }
    }
}
