using System;
using System.Collections.Generic;
using System.Text;

namespace Bat.PortalDeCargas.Domain.DTO
{
    public class ResultUploadDTO
    {
        public int AmountLoaded { get; set; }
        public int AmountErro { get; set; }
        public int AmountCorrect { get; set; }
    }
}
