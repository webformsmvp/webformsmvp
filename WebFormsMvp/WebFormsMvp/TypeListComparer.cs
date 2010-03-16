using System;
using System.Collections.Generic;
using System.Linq;

namespace WebFormsMvp
{
    internal class TypeListComparer<T> : IEqualityComparer<IEnumerable<T>>
        where T : class
    {
        public bool Equals(IEnumerable<T> x, IEnumerable<T> y)
        {
            if (x == null) throw new ArgumentNullException("x");
            if (y == null) throw new ArgumentNullException("y");

            var objectsInX = x.ToList();
            var objectsInY = y.ToList();

            if (objectsInX.Count() != objectsInY.Count())
                return false;

            foreach (var objectInY in objectsInY)
            {
                if (!objectsInX.Contains(objectInY))
                    return false;

                objectsInX.Remove(objectInY);
            }

            return objectsInX.Empty();
        }

        public int GetHashCode(IEnumerable<T> obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");

            var result = obj
                .Aggregate<T, int?>(null, (current, o) =>
                    current == null ? o.GetHashCode() : current | o.GetHashCode());

            return result ?? 0;
        }
    }
}