using Mnemonic.Strings.Interfaces;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Mnemonic.Strings.General;

public class StringReplace : IStringReplace
{
    private readonly List<StringReplacePair> _patterns = new List<StringReplacePair>();

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
        foreach (var pair in CollectionsMarshal.AsSpan(_patterns))
        {
            input = input.Replace(pair.Pattern, pair.Replacement);
        }

        return input;
    }

    private void AddPatternCore(string pattern, string replacement)
    {
        _patterns.Add(
            new StringReplacePair
            {
                Pattern = pattern,
                Replacement = replacement
            });
    }
}
