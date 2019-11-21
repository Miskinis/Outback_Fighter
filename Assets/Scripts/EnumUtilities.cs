using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

public static class EnumUtilities
{
    public static class Enumerable
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasAll<T>(IEnumerable<T> to, IEnumerable<T> against)
        {
            return to.All(against.Contains);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasAny<T>(IEnumerable<T> to, IEnumerable<T> against)
        {
            return to.Any(against.Contains);
        }
    }

    public static class FlagUtilities
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasAll(int to, int against)
        {
            return (against & to) == to;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasAny(int to, int against)
        {
            return (against & to) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Combine(int to, int against)
        {
            return to | against;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Remove(int to, int against)
        {
            return to & ~against;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Toggle(ref int to, int against)
        {
            return to ^ against;
        }
    }
}