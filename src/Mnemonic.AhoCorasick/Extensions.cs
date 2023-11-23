using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Mnemonic.AhoCorasick;

public static class Extensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AddIfNotExists<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key) where TValue : new()
    {
        ref var valOrNew = ref CollectionsMarshal.GetValueRefOrAddDefault(dictionary, key, out var existed);
        if (!existed) { valOrNew = new(); }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AddIfNotExists<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
    {
        ref var valOrNew = ref CollectionsMarshal.GetValueRefOrAddDefault(dictionary, key, out var existed);
        if (!existed) { valOrNew = value; }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetStableHashCode(this string str)
    {
        unchecked
        {
            var hash1 = 5381;
            var hash2 = hash1;

            for (var i = 0; i < str.Length && str[i] != '\0'; i += 2)
            {
                hash1 = ((hash1 << 5) + hash1) ^ str[i];
                if (i == str.Length - 1 || str[i + 1] == '\0')
                { break; }
                hash2 = ((hash2 << 5) + hash2) ^ str[i + 1];
            }

            return hash1 + (hash2 * 1566083941);
        }
    }
}
