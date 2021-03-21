using System;
using System.Collections.Generic;

namespace RomenoCompany
{
    public static class LinqExtention
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
                action(item);
        }
    }
}
