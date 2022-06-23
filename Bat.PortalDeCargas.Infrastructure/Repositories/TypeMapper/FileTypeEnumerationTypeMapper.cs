using System;
using System.Data;
using Bat.PortalDeCargas.Domain.Enums;
using Dapper;

namespace Bat.PortalDeCargas.Infrastructure.Repositories.TypeMapper
{
    public class FileTypeEnumerationTypeMapper : SqlMapper.TypeHandler<FileType>
    {
        public override FileType Parse(object value) => (FileType)(int)value;

        public override void SetValue(IDbDataParameter parameter, FileType value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            parameter.Value = (int)value;
        }
    }
}
