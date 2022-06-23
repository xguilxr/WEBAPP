using System;
using System.Data;
using Bat.PortalDeCargas.Domain.Enums;
using Dapper;

namespace Bat.PortalDeCargas.Infrastructure.Repositories.TypeMapper
{
    public class LogActionEnumerationTypeMapper : SqlMapper.TypeHandler<LogAction>
    {
        public override LogAction Parse(object value) => (LogAction)(int)value;

        public override void SetValue(IDbDataParameter parameter, LogAction value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            parameter.Value = (int)value;
        }
    }
}
