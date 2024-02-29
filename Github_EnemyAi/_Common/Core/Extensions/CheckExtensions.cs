using System.Collections.Generic;

namespace _Common.Core.Extensions {
    public static class CheckExtensions {
        public static bool IsNullOrEmpty<T>(this ICollection<T> list) {
            return list == null || list.Count <= 0;
        }
    }
}