using System;
using System.Collections.Generic;
using System.Linq;

namespace ListExtensions
{
    public class Filter<T>
    {
        public static Filter<T> Include(params T[] items)
        {
            return new Filter<T>(false, items);
        }

        public static Filter<T> Exclude(params T[] items)
        {
            return new Filter<T>(true, items);
        }

        public static Filter<T> CreateAcceptAllList()
        {
            return new Filter<T>(true, new List<T>());
        }


        public static Filter<T> CreateEmpty()
        {
            return new Filter<T>(false, new List<T>());
        }

        private Filter(bool allBut, params T[] items)
            : this(allBut, (IEnumerable<T>)items)
        {

        }

        private Filter(bool allBut, IEnumerable<T> items)
        {
            AllBut = allBut;
            this.items = items.Distinct().ToList();
        }

        private bool AllBut { get; set; }

        private readonly List<T> items;

        public List<T> GetAllItems(params T[] allItemsSet)
        {
            return GetAllItems(allItemsSet.AsEnumerable());
        }

        public List<T> GetAllItems(IEnumerable<T> allItemsSet)
        {
            return allItemsSet.Where(Contains).ToList();
        }

        public Filter<T> Intersect(IEnumerable<T> second)
        {
            return Intersect(new Filter<T>(false, second.ToList()));
        }

        public Filter<T> Intersect(Filter<T> second)
        {
            if (AllBut && second.AllBut)
                return new Filter<T>(true, items.Concat(second.items));

            if (AllBut == false && second.AllBut == false)
                return new Filter<T>(false, items.Intersect(second.items).ToList());

            if (AllBut && second.AllBut == false)
                return Concat(second.items, items);

            return Concat(items, second.items);
        }

        public Filter<T> Concat(Filter<T> second)
        {
            if (AllBut && second.AllBut)
                return new Filter<T>(true, items.Intersect(second.items).ToList());

            if (AllBut == false && second.AllBut == false)
                return new Filter<T>(false, items.Concat(second.items));

            if (AllBut && second.AllBut == false)
                return new Filter<T>(true, items.Except(second.items).ToList());

            return new Filter<T>(true, second.items.Except(items).ToList());
        }

        private static Filter<T> Concat(List<T> includeItems, List<T> excludeItems)
        {
            return new Filter<T>(false, includeItems.Except(excludeItems).ToList());
        }

        public bool Contains(T item)
        {
            return items.Contains(item) != AllBut;
        }

        public Filter<T> Substract(T item)
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

        public Filter<T> Clone()
        {
            return new Filter<T>(AllBut, items.ToList());
        }

        public bool AcceptAll()
        {
            return AllBut && items.Any() == false;
        }

        public Filter<TOut> Select<TOut>(Func<T, TOut> func)
        {
            return new Filter<TOut>(AllBut, items.Select(func));
        }

        private bool Equals(Filter<T> other)
        {
            return AllBut == other.AllBut
                   && items.Count == other.items.Count
                   && items.Except(other.items).Any() == false;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Filter<T>)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((items != null ? items.GetHashCode() : 0) * 397) ^ AllBut.GetHashCode();
            }
        }
    }

    public static class Filter
    {
        public static Filter<T> Include<T>(params T[] items)
        {
            return Filter<T>.Include(items);
        }

        public static Filter<T> Exclude<T>(params T[] items)
        {
            return Filter<T>.Exclude(items);
        }
    }
}