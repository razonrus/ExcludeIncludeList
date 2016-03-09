using System.Collections.Generic;
using ListExtensions;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class FilterTests
    {
        [Test]
        public void ConcatTest()
        {
            Assert.AreEqual(
                Filter.Exclude(3),
                Filter.Exclude(1, 2, 3).Concat(Filter.Exclude(3, 4, 5))
                );

            Assert.AreEqual(
                Filter.Include(1, 2, 3, 4, 5),
                Filter.Include(1, 2, 3).Concat(Filter.Include(3, 4, 5))
                );

            Assert.AreEqual(
                Filter.Exclude(1, 2),
                Filter.Exclude(1, 2, 3).Concat(Filter.Include(3, 4, 5))
                );

            Assert.AreEqual(
                Filter.Exclude(4, 5),
                Filter.Include(1, 2, 3).Concat(Filter.Exclude(3, 4, 5))
                );


            Assert.AreEqual(
                Filter<int>.CreateAcceptAllList(),
                Filter<int>.CreateAcceptAllList().Concat(Filter.Exclude(3, 4, 5))
                );

            Assert.AreEqual(
                Filter<int>.CreateAcceptAllList(),
                Filter<int>.CreateAcceptAllList().Concat(Filter.Include(3, 4, 5))
                );


            Assert.AreEqual(
                Filter.Include(3, 4, 5),
                Filter<int>.Include().Concat(Filter.Include(3, 4, 5))
                );

            Assert.AreEqual(
                Filter.Exclude(3, 4, 5),
                Filter<int>.Include().Concat(Filter.Exclude(3, 4, 5))
                );
        }

        [Test]
        public void EqualsTest()
        {
            Assert.AreEqual(
                Filter.Exclude(1, 2, 3),
                Filter.Exclude(3, 1, 2, 3)
                );

            Assert.AreEqual(
                Filter.Include(1, 2, 3),
                Filter.Include(3, 1, 2, 3)
                );

            Assert.AreNotEqual(
                Filter.Exclude(1, 2, 3),
                Filter.Include(3, 1, 2, 3)
                );

            Assert.AreNotEqual(
                Filter.Exclude(1, 2, 3),
                Filter.Include(1, 2, 3)
                );


            Assert.AreNotEqual(
                Filter.Exclude(1, 2, 3),
                Filter.Exclude(1, 2, 3, 4)
                );

            Assert.AreNotEqual(
                Filter.Include(1, 2, 3),
                Filter.Include(1, 2, 3, 4)
                );
        }

        [Test]
        public void IntersectTest()
        {
            Assert.AreEqual(
                Filter.Exclude(1, 2, 3, 4, 5),
                Filter.Exclude(1, 2, 3).Intersect(Filter.Exclude(3, 4, 5))
                );

            Assert.AreEqual(
                Filter.Include(3),
                Filter.Include(1, 2, 3).Intersect(Filter.Include(3, 4, 5))
                );
            Assert.AreEqual(
                Filter.Include(3),
                Filter.Include(1, 2, 3).Intersect(new List<int> { 3, 4, 5 })
                );

            Assert.AreEqual(
                Filter.Include(4, 5),
                Filter.Exclude(1, 2, 3).Intersect(Filter.Include(3, 4, 5))
                );
            Assert.AreEqual(
                Filter.Include(4, 5),
                Filter.Exclude(1, 2, 3).Intersect(new List<int> { 3, 4, 5 })
                );

            Assert.AreEqual(
                Filter.Include(1, 2),
                Filter.Include(1, 2, 3).Intersect(Filter.Exclude(3, 4, 5))
                );


            Assert.AreEqual(
                Filter.Exclude(3, 4, 5),
                Filter<int>.CreateAcceptAllList().Intersect(Filter.Exclude(3, 4, 5))
                );

            Assert.AreEqual(
                Filter.Include(3, 4, 5),
                Filter<int>.CreateAcceptAllList().Intersect(Filter.Include(3, 4, 5))
                );


            Assert.AreEqual(
                Filter<int>.Include(),
                Filter<int>.Include().Intersect(Filter.Include(3, 4, 5))
                );

            Assert.AreEqual(
                Filter<int>.Include(),
                Filter<int>.Include().Intersect(Filter.Exclude(3, 4, 5))
                );
        }

        [Test]
        public void GetAllItemsTest()
        {
            CollectionAssert.AreEqual(new List<int> { 4, 5 }, Filter.Exclude(1, 2, 3).GetAllItems(1, 2, 3, 4, 5));
            CollectionAssert.AreEqual(new List<int> { 1, 2, 3 }, Filter.Include(1, 2, 3).GetAllItems(1, 2, 3, 4, 5));
        }

        [Test]
        public void ContainsTest()
        {
            Assert.True(Filter.Include(4, 5).Contains(5));
            Assert.False(Filter.Include(4, 5).Contains(7));

            Assert.False(Filter.Exclude(4, 5).Contains(5));
            Assert.True(Filter.Exclude(4, 5).Contains(7));
        }

        [Test]
        public void SubstractTest()
        {
            Assert.AreEqual(
                Filter.Include(1, 3),
                Filter.Include(1, 2, 3).Substract(2)
                );

            Assert.AreEqual(
                Filter.Include(1, 2, 3),
                Filter.Include(1, 2, 3).Substract(4)
                );

            Assert.AreEqual(
                Filter.Exclude(1, 2, 3, 4),
                Filter.Exclude(1, 2, 3).Substract(4)
                );

            Assert.AreEqual(
                Filter.Exclude(1, 2, 3),
                Filter.Exclude(1, 2, 3).Substract(2)
                );


            Assert.AreEqual(
                Filter.Exclude(1),
                Filter<int>.CreateAcceptAllList().Substract(1)
                );
        }

        [Test]
        public void AnyTest()
        {
            Assert.True(Filter.Exclude<int>().Any());
            Assert.False(Filter.Include<int>().Any());

            Assert.True(Filter.Exclude(1).Any());
            Assert.True(Filter.Include(1).Any());

            Assert.False(Filter.Include(1).Substract(1).Any());
            Assert.False(Filter.Include(1).Intersect(Filter.Include(2)).Any());

            Assert.True(Filter<int>.CreateAcceptAllList().Any());
        }

        [Test]
        public void AcceptAllTest()
        {
            Assert.True(Filter.Exclude<int>().AcceptAll());
            Assert.False(Filter.Include<int>().AcceptAll());

            Assert.False(Filter.Exclude(1).AcceptAll());
            Assert.False(Filter.Include(1).AcceptAll());

            Assert.False(Filter.Include(1).Substract(1).AcceptAll());
            Assert.True(Filter.Include(1).Concat(Filter.Exclude(1)).AcceptAll());

            Assert.True(Filter<int>.CreateAcceptAllList().AcceptAll());
        }

        [Test]
        public void CloneTest()
        {
            var exclude = Filter.Exclude(1);
            Assert.AreEqual(exclude, exclude.Clone());
            Assert.AreNotSame(exclude, exclude.Clone());

            var include = Filter.Include(1);
            Assert.AreEqual(include, include.Clone());
            Assert.AreNotSame(include, include.Clone());
        }

        [Test]
        public void SelectTest()
        {
            Assert.AreEqual(Filter.Exclude(2, 5), Filter.Exclude(1, 4).Select(x => x + 1));
            Assert.AreEqual(Filter.Include(2, 5), Filter.Include(1, 4).Select(x => x + 1));
        }
    }
}