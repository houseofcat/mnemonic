using Mnemonic.Utilities.Helpers;
using System;
using System.Text.RegularExpressions;

namespace Mnemonic.Strings.Regex;

public sealed record RegexPair
{
    public System.Text.RegularExpressions.Regex Regex { get; private set; }
    public string Replacement { get; private set; }
    public int Count { get; private set; }

    public RegexPair(string pattern, string replacement, int count = 1, RegexOptions? options = null)
    {
        Regex = new System.Text.RegularExpressions.Regex(
            RegexHelpers.BuildRegexSubstringPattern(pattern),
            options ?? RegexOptions.Singleline);

        Replacement = replacement;
        Count = count;
    }

    public bool IsMatch(ReadOnlySpan<char> input)
    {
        return Regex.IsMatch(input);
    }

    public string Replace(string input)
    {
        return Regex.Replace(input, Replacement, Count);
    }
}
