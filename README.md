# Mnemonic

A library of `.NET` tools to help put real world use cases into insteresting Data
Structures and Algorithms.  

## Mnemonic.Strings

This project is centered around efficient `string` manipulation.  

### Mnemonic.Strings.AhoCorasick

Aho-Corasick is an [algorithm](https://en.wikipedia.org/wiki/Aho%E2%80%93Corasick_algorithm) for string searching.  

This is a first practical implemenation with `FailureLinks` to find and substitute in values.  

#### AhoCorasickStringReplace Class

Consider the use case of having many keyword / replacement lookups. The real world has
plenty of examples of where you might need something like this. It could be a chat swear
filter or perhaps text substitutions in your ML model outputs. A simple solution would be
to iterate over all `string pattern/replacement` pairs, and for any user input do a find
and replace.  

This solution to the problem does it different by using a node tree and looking at each 
character one by one with an early exit. Also on early exit detect if there is another
branch in the TrieNode tree to descend down to. The only twist added here is that instead
of just being an `IsFound()` impelmentation, the leaf nodes branches (end of branches),
indicate it is the end of a found pattern and a `string replacement` is an additional
property on the node to be used for substitution.  

The ideal scenario is you have a large list (larger the better) of strings that can
be cached and used for constant re-evaluation without over-taxing garbage collection. 
Surprisingly, now in NET6+, `string.Replace()` is quite efficient so you should
always test which approach is actually more efficient for your use case.  

String allocations are based on using `StringBuilder`.

Performance/Complexity don't really matter too much until you get to a very large
pattern replacement pairs but should be something akin to O(m+n) where m is
input length + number of matches.  

What can be interesting is that the working memory can be quite smaller than having
to storing every pattern since it only adds non-existing chars creating only unique
branches.  

#### Simplified Over View
```plaintext
Inputs: 
apple red, apple blue
apple blue, apple red

      
[\0] -> [a] -> [p] -> [p] -> [l] -> [e] -> [' '] -> [r] -> [e] -> [d] 
                                                 -> [b] -> [l] -> [u] -> [e]
```

Give the input `apple red` and this simplified tree, you would start at `root/null`,
and iterate over each node linearly because we are matching characters. We hit the
space (`' '`). We now have two pathways to keep traversing downards. If the next
letter is `'r'` then we keep going down that path until we hit hopefully hit `'d'`.
Then hitting the node containing `'d'` we see that pattern completed and thus we
need to use this node's `replacement` property. That value is currently `apple blue`
as the replacement.  

Input Examples
```plaintext
\0 -> \0
cat -> cat
apple -> apple
apple re -> apple re
apple red -> apple blue
apple redblue -> apple blueblue
apple redapplered -> apple blueapple blue
red apple -> red apple
apple blue apple red apple blue -> apple red apple blue apple red
```

#### Setup

```csharp
// The values we want to find and substitute in.
var patterns = new Dictionary<string, string>
{
    { "apple is red", "apple is yellow" },
    { "apple is teal", "apple is blue" },
    { "apple is puple", "apple is violet" },
    { "apple is yellowish", "apple is brown" },
    { "apple is bolt", "apple is belt" },
    { "apple is orange", "apple is blue" },
    { "apple is rod", "apple is red" },
    { "apple is green", "apple is blue" },
    { "pear is green", "pear is blue" },
    { "cheese is green", "cheese has mold" }
};

// Class holds the TrieNodes and relationships.
var ac = new AhoCorasickStringReplace();

// Flexibility adding (string, string) combinations.
ac.AddPatterns(patterns);


// Indicate to the class that no more new patterns will
// be added and to generate all the failure links that
// will allow neighbor branch hopping when out of leaves
// but partial pattern still exists.
ac.BuildFailureLinks();
```

#### Usage

```csharp
var output = ac.Replace(input);
```