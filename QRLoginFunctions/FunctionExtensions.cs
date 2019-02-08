using System;
using System.Collections.Generic;

namespace QRLoginFunctions
{
    internal static class FunctionExtensions
    {
        public static T FirstOrDefault<T>(
            this IEnumerable<KeyValuePair<string, string>> enumerable, 
            string key,
            IEqualityComparer<string> comparer)
        {
            foreach (var kvp in enumerable)
            {
                if (comparer.Equals(kvp.Key, key))
                {
                    try
                    {
                        T result = (T) Convert.ChangeType(kvp.Value, typeof(T));
                        return result;
                    }
                    catch
                    {
                    }

                    break;
                }
            }

            return default(T);
        }
    }
}
