using System;
using System.Collections.Generic;
using System.Text;

namespace Bat.PortalDeCargas.Domain.DTO
{
    public class ResultDTO
    {
        public bool IndSucesso { get; set; }
        public IEnumerable<string>  Erros { get; set; }
    }

    public class ResultDTO<T>: ResultDTO
    {
        public T Model { get; set; }
    }
}
