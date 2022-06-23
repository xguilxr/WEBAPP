using System;
using System.Collections.Generic;
using System.Text;

namespace Bat.PortalDeCargas.Domain.Configuration
{
    public class AppConfiguration:IAppConfiguration
    {
       public int ClientsPerPageFilter { get; set; }
       public string Secret { get; set; }
       public string BlobConnectionString { get; set; }
       public string BlobUri { get; set; }
    }
}
