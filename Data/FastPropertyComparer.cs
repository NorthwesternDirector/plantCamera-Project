using System;
using System.Collections.Generic;
using System.Linq;

namespace plantCamera.Data
{
    public class FastPropertyComparer<T, V> : IEqualityComparer<T>
    {
        private Func<T, V> keySelector;  

        public FastPropertyComparer(Func<T, V> keySelector)
        {
            this.keySelector = keySelector;
        }  

        public bool Equals(T x, T y)
        {
            return EqualityComparer<V>.Default.Equals(keySelector(x), keySelector(y));
        }  

        public int GetHashCode(T obj)
        {
            return EqualityComparer<V>.Default.GetHashCode(keySelector(obj));
        }
    }


    public static class DistinctExtensions
    {
        public static IEnumerable<T> Distinct<T, V>(this IEnumerable<T> source,
            Func<T, V> keySelector)
        {
            return source.Distinct(new FastPropertyComparer<T, V>(keySelector));
        }
    }
}