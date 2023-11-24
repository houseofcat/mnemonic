# Mnemonic

A library of `.NET` tools to help put real world use cases into insteresting Data
Structures and Algorithms.  

## Mnemonic.Strings

This project is centered around efficient `string` manipulation.  

### Mnemonic.Strings.AhoCorasick

Aho Corasick is an [algorithm](https://en.wikipedia.org/wiki/Aho%E2%80%93Corasick_algorithm) for string searching.  

The first practical implemenation is a simple `naive` solution with `FailureLinks`.  

#### AhoCorasickStringReplace Class

Consider the use case of having many keyword / replacement lookups. The real world has
plenty of examples of where you might need something like this. It could be a chat filter
or lets say text substitutions in your ML model outputs. A simple solution would be to
iterate over all `string pattern/replacement` pairs, and for any user input do a find and
replace.  

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
