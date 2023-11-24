using Mnemonic.Strings.AhoCorasick;
using Mnemonic.Strings.General;
using Mnemonic.Strings.Regex;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static Mnemonic.Utilities.Extensions.StopwatchExtensions;

Console.WriteLine("Mnemonic StringReplace Examples");

var ac = new AhoCorasickStringReplace();
var sr = new StringReplace();
var regex = new RegexReplace();

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

sr.AddPatterns(patterns);
regex.AddPatterns(patterns);

AhoStringReplace();
StringReplace();
RegexReplace();

Console.ReadKey();

void AhoStringReplace()
{
    var input = "My apple is red green.";
    var sw = Stopwatch.StartNew();
    var output = ac.Replace(input);
    sw.Stop();

    Console.WriteLine("AhoCorasickStringReplace");
    Console.WriteLine($"Input: {input}");
    Console.WriteLine($"Output: {output}");
    Console.WriteLine($"Elapsed: {sw.ElapsedMicroseconds():000} us");
}

void StringReplace()
{
    var input = "My apple is red green.";
    var sw = Stopwatch.StartNew();
    var output = sr.Replace(input);
    sw.Stop();

    Console.WriteLine("StringReplace");
    Console.WriteLine($"Input: {input}");
    Console.WriteLine($"Output: {output}");
    Console.WriteLine($"Elapsed: {sw.ElapsedMicroseconds():000} us");
}

void RegexReplace()
{
    var input = "My apple is red green.";
    var sw = Stopwatch.StartNew();
    var output = regex.Replace(input);
    sw.Stop();

    Console.WriteLine($"Input: {input}");
    Console.WriteLine("RegexReplace");
    Console.WriteLine($"Output: {output}");
    Console.WriteLine($"Elapsed: {sw.ElapsedMicroseconds():000} us");
}
