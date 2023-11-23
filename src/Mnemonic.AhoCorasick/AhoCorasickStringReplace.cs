using System;
using System.Collections.Generic;
using System.Text;

namespace Mnemonic.AhoCorasick;

public sealed class AhoCorasickStringReplace
{
    private readonly TrieNode _root = new TrieNode();
    private bool _isBuilt = false;

    private static readonly string _mustAddFirstError = $"You can only invoke {nameof(AddPattern)} before invoking {nameof(BuildFailureLinks)}.";

    public void AddPattern(ReadOnlySpan<char> pattern, string replacement)
    {
        if (_isBuilt) throw new InvalidOperationException(_mustBuildFirstError);

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

        var result = new StringBuilder();
        var currentNode = _root;
        var i = 0;

        while (i < input.Length)
        {
            if (currentNode.Children.TryGetValue(input[i], out var nextNode))
            {
                currentNode = nextNode;
                i++;
            }
            else if (currentNode == _root)
            {
                result.Append(input[i]);
                i++;
            }
            else
            {
                currentNode = currentNode.FailureLinkNode;
            }

            if (currentNode.IsEndOfPattern)
            {
                result.Append(currentNode.Replacement);
                currentNode = _root;
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
            }

            queue.Enqueue(currentChildNode);
        }
    }
}
