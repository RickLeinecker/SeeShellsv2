using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SeeShellsV2
{
    public static class Extensions
    {
        public static T Swap<T>(this T a, ref T b)
        {
            T temp = b;
            b = a;
            return temp;
        }

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

        public static object GetDeepPropertyValue(this object instance, string path)
        {
            var pp = path.Split('.');
            Type t = instance.GetType();
            foreach (var prop in pp)
            {
                PropertyInfo propInfo = t.GetProperty(prop);
                if (propInfo != null)
                {
                    instance = propInfo.GetValue(instance, null);
                    t = propInfo.PropertyType;
                }
                else throw new ArgumentException("Properties path is not correct");
            }
            return instance;
        }

        /// <summary>
        /// Iterate over WPF Visual Trees
        /// </summary>
        /// <param name="parent">the parent Visual</param>
        /// <param name="recurse">enable to recursively iterate over the tree</param>
        /// <returns>WPF Visual Tree enumerable</returns>
        public static IEnumerable<DependencyObject> GetChildren(this DependencyObject parent, bool recurse = true)
        {
            if (parent != null)
            {
                foreach (var child in LogicalTreeHelper.GetChildren(parent))
                {
                    if (child is DependencyObject d)
                    {
                        yield return d;

                        if (recurse)
                        {
                            foreach (var grandChild in d.GetChildren(true))
                            {
                                yield return grandChild;
                            }
                        }
                    }
                }
            }
        }
    }
}
