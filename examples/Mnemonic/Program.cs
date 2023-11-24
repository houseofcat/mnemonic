using Mnemonic.AhoCorasick;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static Mnemonic.Utilities.Extensions.StopwatchExtensions;

Console.WriteLine("Mnemonic AhoCorasick StringReplace");

var ac = new AhoCorasickStringReplace();
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

ac.AddPatterns(patterns);
ac.BuildFailureLinks();

AhoStringReplace();
StringReplace(patterns);

Console.ReadKey();

void AhoStringReplace()
{
    var input = "My apple is red green.";
    var sw = Stopwatch.StartNew();
    var output = ac.Replace(input);
    sw.Stop();

    Console.WriteLine($"AhoStringReplace");
    Console.WriteLine($"Input: {input}");
    Console.WriteLine($"Output: {output}");
    Console.WriteLine($"Elapsed: {sw.ElapsedMicroseconds():000} us");
}

void StringReplace(Dictionary<string, string> patterns)
{
    Console.WriteLine($"StringReplace");

    var input = "My apple is red green.";
    Console.WriteLine($"Input: {input}");

    var sw = Stopwatch.StartNew();

    foreach (var kvp in patterns)
    {
        input = input.Replace(kvp.Key, kvp.Value);
    }
    sw.Stop();

    Console.WriteLine($"Output: {input}");
    Console.WriteLine($"Elapsed: {sw.ElapsedMicroseconds():000} us");
}