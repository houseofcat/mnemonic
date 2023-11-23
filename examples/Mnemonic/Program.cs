using Mnemonic.AhoCorasick;
using System;

Console.WriteLine("Mnemonic AhoCorasick StringReplace");

var ac = new AhoCorasickStringReplace();

ac.AddPattern("apple is red.", "apple is yellow.");
ac.AddPattern("apple is green.", "apple is blue.");

ac.BuildFailureLinks();

var input = "My apple is red.";
var output = ac.Replace(input);

Console.WriteLine("Input: " + input);
Console.WriteLine("Output: " + output);