using System.Text.RegularExpressions;

namespace AoC;

internal static partial class Utils
{

    [GeneratedRegex(@"mul\((\d{1,3}),(\d{1,3})\)", RegexOptions.Multiline)]
    internal static partial Regex Day3aRegex();

    [GeneratedRegex(@"(do\(\))|(don't\(\))", RegexOptions.Multiline)]
    internal static partial Regex Day3bRegex();
}