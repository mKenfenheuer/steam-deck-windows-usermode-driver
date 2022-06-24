using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWICD_Lib
{
    internal static class Extensions
    {
        public static bool EqualsWithValues<TKey, TValue>(this Dictionary<TKey, TValue> obj1, Dictionary<TKey, TValue>obj2)
        {
            bool equal = false;
            if (obj1.Count == obj2.Count) // Require equal count.
            {
                equal = true;
                foreach (var pair in obj1)
                {
                    TValue value;
                    if (obj2.TryGetValue(pair.Key, out value))
                    {
                        // Require value be equal.
                        if (!value.Equals(pair.Value))
                        {
                            equal = false;
                            break;
                        }
                    }
                    else
                    {
                        // Require key be present.
                        equal = false;
                        break;
                    }
                }
            }

            return equal;
        }
    }
}
