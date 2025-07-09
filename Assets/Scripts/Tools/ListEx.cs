using System;
using System.Collections.Generic;

namespace Scorewarrior
{
    public static class ListEx
    {
        public static int FindIndexNonAlloc<T, TV>(this List<T> list, TV item, Func<T, TV, bool> match)
        {
            for (int i = 0, i_max = list.Count; i < i_max; i++)
            {
                if(match.Invoke(list[i], item)) return i;
            }
            
            return -1;
        }
    };
}
