using System.Collections.Generic;
using System.Linq;

namespace ListExtensions
{
    public static class ExcludeIncludeListExtensions
    {

        //ToDO: покрыть тестами
        public static ExcludeIncludeList<T> IntersectAll<T>(this List<ExcludeIncludeList<T>> lists)
        {
            return lists.Skip(1).Aggregate(lists.First(), (current, list) => current.Intersect(list));
        }

        //ToDO: покрыть тестами
        public static ExcludeIncludeList<T> ConcatAll<T>(this List<ExcludeIncludeList<T>> lists)
        {
            return lists.Skip(1).Aggregate(lists.First(), (current, list) => current.Concat(list));
        }
    }
}