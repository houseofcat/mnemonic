using System.Text.RegularExpressions;

namespace Mnemonic.Utilities.Helpers;

public static class RegexHelpers
{
    public static string BuildRegexSubstringPattern(string input)
    {
        return Regex.Escape(input);
    }

    private static readonly string _exactMatchTemplate = "^{0}$";

    public static string BuildRegexExactPhrasePattern(string input)
    {
        return string.Format(_exactMatchTemplate, Regex.Escape(input));
    }
}
