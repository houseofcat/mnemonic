using Mnemonic.Strings.Interfaces;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Mnemonic.Strings.Regex;

public sealed class RegexReplace : IStringReplace
{
    private readonly List<RegexPair> _patterns = new List<RegexPair>();

    public void AddPatterns(Dictionary<string, string> patterns)
    {
        foreach (var pattern in patterns)
        {
            AddPatternCore(pattern.Key, pattern.Value);
        }
    }

    public void AddPattern(string pattern, string replacement)
    {
        AddPatternCore(pattern, replacement);
    }

    public string Replace(string input)
    {
        foreach (var pattern in CollectionsMarshal.AsSpan(_patterns))
        {
            input = pattern.Replace(input);
        }

        return input;
    }

    public string ReplaceOnFirstMatchOnly(string input)
    {
        foreach (var pattern in CollectionsMarshal.AsSpan(_patterns))
        {
            if (pattern.IsMatch(input))
            {
                return pattern.Replace(input);
            }
        }

        return input;
    }

    private void AddPatternCore(string pattern, string replacement)
    {
        _patterns.Add(new RegexPair(pattern, replacement));
    }
}
