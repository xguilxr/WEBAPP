using System;
using System.Collections.Generic;
using System.Text;

namespace Bat.PortalDeCargas.Domain.DTO
{
    public class RowValidateDTO
    {
        public string Value { get; set; }
        public IList<string> Erros { get; set; }

        public RowValidateDTO(string value, IList<string> Erros)
        {
            this.Value = value;
            this.Erros = Erros;
        }
    }
}
