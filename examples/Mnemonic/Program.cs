using Mnemonic.AhoCorasick;
using System;
using System.Diagnostics;
using static Mnemonic.Utilities.Extensions.StopwatchExtensions;

Console.WriteLine("Mnemonic AhoCorasick StringReplace");

var ac = new AhoCorasickStringReplace();

ac.AddPattern("apple is red", "apple is yellow");
ac.AddPattern("apple is teal", "apple is blue");
ac.AddPattern("apple is puple", "apple is violet");
ac.AddPattern("apple is yellowish", "apple is brown");
ac.AddPattern("apple is bolt", "apple is belt");
ac.AddPattern("apple is orange", "apple is blue");
ac.AddPattern("apple is rod", "apple is red");
ac.AddPattern("apple is green", "apple is blue");
ac.AddPattern("pear is green", "pear is blue");
ac.AddPattern("cheese is green", "cheese has mold");

ac.BuildFailureLinks();

var sw = Stopwatch.StartNew();
var input = "My apple is red green.";
var output = ac.Replace(input);
sw.Stop();

Console.WriteLine($"Input: {input}");
Console.WriteLine($"Output: {output}");
Console.WriteLine($"Elapsed: {sw.ElapsedMicroseconds():000} us");

Console.ReadKey();