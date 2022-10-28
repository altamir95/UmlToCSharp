using System.Text.RegularExpressions;
using UmlToCSharp.Pattern;

namespace UmlToCSharp.Parsers
{
    internal class EnumParser
    {
        private const string _template = "public enum uml_name\n    {\n        uml_enum_values\n    }";

        public Regex Regex = new (pattern: @"enum\s([^<\s]+)\s(<([^{<>]+)>\s)?{\n([^\n{}]+\n){0,}}");

        public string EnumStr { get; set; }

        public EnumParser(string enumStr)
        {
            EnumStr = enumStr;
        }


        protected string GetInnerGroupFromUml(string groupName)
        {
            return Regex.Match(EnumStr, PatternParts.ToString(PatternParts.enumPatternParts)).Groups[groupName].Value;
        }

        protected List<string> GetListInnerGroupFromUml(string groupName)
        {
            return Regex.Matches(EnumStr, PatternParts.ToString(PatternParts.enumPatternParts)).Select(f => f.Groups[groupName].Value).ToList();
        }

        public override string ToString() =>
            _template
            .Replace("uml_name", GetInnerGroupFromUml("object_name"))
            .Replace("uml_enum_values", string.Join(",\n", GetListInnerGroupFromUml("enum_item")));
    }
}