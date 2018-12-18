using System.Collections.Generic;
using System.Linq;

namespace CommunIT.Shared.Portable.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            if (source is null)
                return true;
            if (!source.Any())
                return true;

            return false;
        }

        public static bool IsNullOrEmpty<T>(this IQueryable<T> source)
        {
            if (source is null)
                return true;
            if (!source.Any())
                return true;

            return false;
        }

        public static IEnumerable<T> DefaultIfNull<T>(this IEnumerable<T> source)
        {
            if (source is null)
                return new List<T>();

            return source;
        }
    }
}
