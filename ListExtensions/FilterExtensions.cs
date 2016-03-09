using System.Collections.Generic;
using System.Linq;

namespace ListExtensions
{
    public static class FilterExtensions
    {
        public static Filter<T> IntersectAll<T>(this List<Filter<T>> lists)
        {
            return lists.Skip(1).Aggregate(lists.First(), (current, list) => current.Intersect(list));
        }

        public static Filter<T> ConcatAll<T>(this List<Filter<T>> lists)
        {
            if (lists.Any() == false)
                return Filter<T>.CreateEmpty();

            return lists.Skip(1).Aggregate(lists.First(), (current, list) => current.Concat(list));
        }
    }
}