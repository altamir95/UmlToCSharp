using System;
using System.IO;
using System.Text.RegularExpressions;
using UmlToCSharp.Pattern;

namespace UmlToCSharp.Utils
{
    public static class RegexUtil
    {
        public static string GetInnerGroupFromUml(string input, string pattern, string groupName)
        {
            return Regex.Match(input, pattern).Groups[groupName].Value;
        }

        public static string GetInnerGroupFromUml(string input, PatternItem[] patternItems, string groupName)
        {
            return Regex.Match(input, string.Join(string.Empty, patternItems.Select(r => r.Pattern))).Groups[groupName].Value;
        }

        public static string GetInnerGroupFromUml(string separator, string input, PatternItem[] patternItems, string groupName)
        {
            return string.Join(
                            separator: separator,
                            values: Regex.Matches(
                                            input: input,
                                            pattern: string.Join(
                                                        separator: string.Empty,
                                                        values: patternItems.Select(r => r.Pattern)))
                            .Select(f => f.Groups[groupName].Value));
        }

        public static string GetInnerGroupFromUml(string separator, string input, string pattern, string groupName)
        {
            return string.Join(
                            separator: separator,
                            values: Regex.Matches(
                                            input: input,
                                            pattern: pattern)
                            .Select(f => f.Groups[groupName].Value));
        }
    }
}