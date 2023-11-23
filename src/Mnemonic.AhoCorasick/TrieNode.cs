using System.Collections.Generic;
using System.Diagnostics;

namespace Mnemonic.AhoCorasick;

[DebuggerDisplay("{Replacement}", Name = "{Key}")]
public sealed record TrieNode
{
    public char Key { get; set; }
    public Dictionary<char, TrieNode> Children { get; } = new Dictionary<char, TrieNode>();
    public TrieNode FailureLinkNode { get; set; }
    public bool IsEndOfPattern { get; set; }
    public string Replacement { get; set; }
}
