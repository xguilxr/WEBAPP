using System.Collections.Generic;
using System.Linq;

namespace Bat.PortalDeCargas.Domain.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool IsEmpty<T>(this IEnumerable<T> enumerable) => !enumerable.Any();
    }
}
