using Cysharp.Text;
using Mnemonic.Strings.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using static Mnemonic.Utilities.Extensions.DictionaryExtensions;

namespace Mnemonic.Strings.AhoCorasick;

public sealed class AhoCorasickStringReplace : IStringReplace
{
    private readonly TrieNode _root = new TrieNode { IsRoot = true };
    private bool _isBuilt = false;

    private static readonly string _mustAddFirstError1 = $"You can only invoke {nameof(AddPatterns)} before invoking {nameof(BuildFailureLinks)}.";
    private static readonly string _mustAddFirstError2 = $"You can only invoke {nameof(AddPattern)} before invoking {nameof(BuildFailureLinks)}.";

    public void AddPatterns(Dictionary<string, string> patterns)
    {
        if (_isBuilt) throw new InvalidOperationException(_mustAddFirstError1);

        foreach (var pattern in patterns)
        {
            AddPatternCore(pattern.Key, pattern.Value);
        }
    }

    public void AddPattern(string pattern, string replacement)
    {
        if (_isBuilt) throw new InvalidOperationException(_mustAddFirstError2);

        AddPatternCore(pattern, replacement);
    }

    private void AddPatternCore(ReadOnlySpan<char> pattern, string replacement)
    {
        var node = _root;

        foreach (char c in pattern)
        {
            node.Children.AddIfNotExists(c, new TrieNode { Key = c });
            node = node.Children[c];
        }

        node.IsEndOfPattern = true;
        node.Replacement = replacement;
    }

    public void BuildFailureLinks()
    {
        if (_isBuilt) return;

        var queue = new Queue<TrieNode>();

        foreach (var child in _root.Children.Values)
        {
            child.FailureLinkNode = _root;
            queue.Enqueue(child);
        }

        while (queue.Count > 0)
        {
            BuildFailureLink(queue);
        }

        _isBuilt = true;
    }

    private static readonly string _mustBuildFirstError = $"You must invoke {nameof(BuildFailureLinks)} before attempting to invoke {nameof(Replace)}.";

    public string Replace(ReadOnlySpan<char> input)
    {
        if (!_isBuilt) throw new InvalidOperationException(_mustBuildFirstError);
        if (input.Length == 0) return default;

        using var result = ZString.CreateUtf8StringBuilder();
        var currentNode = _root;
        var i = 0;
        var partialMatchIndexStart = -1;

        while (i < input.Length)
        {
            var currentChar = input[i];
            DebugView("Iteration", i, currentChar, currentNode.Key, currentNode.Replacement);

            if (currentNode.Children.TryGetValue(currentChar, out var nextNode))
            {
                DebugView("PartialMatchStarted", i, currentChar, currentNode.Key, currentNode.Replacement);
                currentNode = nextNode;
                if (partialMatchIndexStart == 0)
                {
                    partialMatchIndexStart = i;
                }
                i++;
            }
            else if (currentNode.IsRoot)
            {
                DebugView("NoMatchFound", i, currentChar, currentNode.Key, currentNode.Replacement);
                result.Append(currentChar);
                partialMatchIndexStart = -1;
                i++;
            }
            else
            {
                DebugView("FailureLink", i, currentChar, currentNode.Key, currentNode.Replacement);
                result.Append(input[partialMatchIndexStart..i]);
                currentNode = currentNode.FailureLinkNode;
                partialMatchIndexStart = -1;
            }

            if (currentNode.IsEndOfPattern)
            {
                DebugView("EndOfPatternFound", i, currentChar, currentNode.Key, currentNode.Replacement);
                result.Append(currentNode.Replacement);
                currentNode = _root;
                partialMatchIndexStart = -1;
            }

        }

        return result.ToString();
    }

    private void BuildFailureLink(Queue<TrieNode> queue)
    {
        var currentNode = queue.Dequeue();

        foreach (var kvp in currentNode.Children)
        {
            var currentKey = kvp.Key;
            var currentChildNode = kvp.Value;
            var currentFailureLinkeNode = currentNode.FailureLinkNode;

            while (currentFailureLinkeNode != null
                && !currentFailureLinkeNode.Children.ContainsKey(currentKey))
            {
                currentFailureLinkeNode = currentFailureLinkeNode.FailureLinkNode;
            }

            if (currentFailureLinkeNode == null)
            {
                currentChildNode.FailureLinkNode = _root;
            }
            else
            {
                currentChildNode.FailureLinkNode = currentFailureLinkeNode.Children[currentKey];
            }

            if (currentChildNode.FailureLinkNode.IsEndOfPattern)
            {
                currentChildNode.IsEndOfPattern = true;
                currentChildNode.Replacement = currentNode.Replacement;
            }

            queue.Enqueue(currentChildNode);
        }
    }

    private static readonly string _inputDetails = "InputValue: [{0}:{1}]";
    private static readonly string _nodeDetails = "Node: [Key:{0}, Replacement:{1}]";

    [Conditional("DEBUG")]
    private static void DebugView(string action, params object[] args)
    {
        Console.WriteLine($"{action} - {_inputDetails}", args[0], args[1]);
        Console.WriteLine($"{action} - {_nodeDetails}", args[2], args[3]);
    }
}
