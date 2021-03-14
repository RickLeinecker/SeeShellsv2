using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShellsV2
{
    public static class Extensions
    {
        public static T GetClassOrDefault<T>(this IDictionary<string, object> dict, string key, T defaultValue) where T : class
        {
            return dict.ContainsKey(key) ? dict[key] as T ?? defaultValue : defaultValue;
        }

        public static T GetStructOrDefault<T>(this IDictionary<string, object> dict, string key, T defaultValue) where T : struct
        {
            return dict.ContainsKey(key) ? dict[key] as T? ?? defaultValue : defaultValue;
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (T item in enumeration)
                action(item);

            return enumeration;
        }

        public static IEnumerable<IEnumerable<T>> GroupWhile<T>(this IEnumerable<T> seq, Func<T, T, bool> condition)
        {
            if (!seq.Any())
                yield break;

            T prev = seq.First();
            List<T> list = new List<T>() { prev };

            foreach (T item in seq.Skip(1))
            {
                if (condition(prev, item) == false)
                {
                    yield return list;
                    list = new List<T>();
                }
                list.Add(item);
                prev = item;
            }

            yield return list;
        }
    }
}
