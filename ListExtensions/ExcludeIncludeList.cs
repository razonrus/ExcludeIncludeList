using System;
using System.Collections.Generic;
using System.Linq;

namespace ListExtensions
{
    public class ExcludeIncludeList<T>
    {
        public static ExcludeIncludeList<T> CreateAcceptAllList()
        {
            return new ExcludeIncludeList<T>(true, new List<T>());
        }

        public ExcludeIncludeList(bool allBut, List<T> items)
        {
            AllBut = allBut;
            this.items = items;
        }

        private bool AllBut { get; set; }

        private readonly List<T> items;

        public List<T> GetAllItems(IEnumerable<T> allItemsSet)
        {
            return allItemsSet.Where(Contains).ToList();
        }

        public ExcludeIncludeList<T> Intersect(IEnumerable<T> second)
        {
            return Intersect(new ExcludeIncludeList<T>(false, second.ToList()));
        }

        //TODO: покрыть тестами
        public ExcludeIncludeList<T> Intersect(ExcludeIncludeList<T> second)
        {
            if (AllBut && second.AllBut)
                return new ExcludeIncludeList<T>(true, items.Concat(second.items).Distinct().ToList());

            if (AllBut == false && second.AllBut == false)
                return new ExcludeIncludeList<T>(false, items.Intersect(second.items).ToList());

            if (AllBut && second.AllBut == false)
                return Concat(second.items, items);

            return Concat(items, second.items);
        }

        public ExcludeIncludeList<T> Concat(ExcludeIncludeList<T> second)
        {
            if (AllBut && second.AllBut)
                return new ExcludeIncludeList<T>(true, items.Intersect(second.items).ToList());

            if (AllBut == false && second.AllBut == false)
                return new ExcludeIncludeList<T>(false, items.Concat(second.items).Distinct().ToList());

            if (AllBut && second.AllBut == false)
                return new ExcludeIncludeList<T>(true, items.Except(second.items).ToList());

            return new ExcludeIncludeList<T>(true, second.items.Except(items).ToList());
        }

        private static ExcludeIncludeList<T> Concat(List<T> includeItems, List<T> excludeItems)
        {
            return new ExcludeIncludeList<T>(false, includeItems.Except(excludeItems).ToList());
        }

        public bool Contains(T item)
        {
            return items.Contains(item) != AllBut;
        }

        public ExcludeIncludeList<T> Substract(T item)
        {
            if (AllBut)
            {
                if (items.Contains(item) == false)
                    items.Add(item);
            }
            else
            {
                if (items.Contains(item))
                    items.Remove(item);
            }

            return this;
        }

        public bool Any()
        {
            return AllBut || items.Any();
        }

        public ExcludeIncludeList<T> Clone()
        {
            return new ExcludeIncludeList<T>(AllBut, items.ToList());
        }

        public bool AcceptAll()
        {
            return AllBut && items.Any() == false;
        }

        public ExcludeIncludeList<TOut> Select<TOut>(Func<T, TOut> func)
        {
            return new ExcludeIncludeList<TOut>(AllBut, items.Select(func).Distinct().ToList());
        }
    }
}