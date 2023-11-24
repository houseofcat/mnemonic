using System.Collections.Generic;

namespace Mnemonic.Strings.Interfaces;

public interface IStringReplace
{
    void AddPatterns(Dictionary<string, string> patterns);
    void AddPattern(string pattern, string replacement);
}
