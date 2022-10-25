using System;
using System.Text.RegularExpressions;

namespace UmlToCSharp
{
    public class UmlEnum
    {
        private const string _template = "public enum uml_name\n    {\n        uml_enum_values\n    }";

        public Regex Regex = new Regex(pattern: @"enum\s([^<\s]+)\s(<([^{<>]+)>\s)?{\n([^\n{}]+\n){0,}}");


        public string EnumStr { get; set; }

        public string Name { get; set; }

        public List<string> Indexes { get; set; }

        public UmlEnum(string enumStr)
        {
            EnumStr = enumStr;
            SetName();
            SetIndexes();
        }

        private void SetName()
        {
            Name = Regex.Match(input: EnumStr, pattern: new Regex(@"enum\\s([^\\s]+)").ToString()).Groups[1].Value;
        }

        private void SetIndexes()
        {
            Indexes = Regex.Matches(input: EnumStr, pattern: new Regex(@"([^\n\s]+),").ToString()).Select(e => e.Groups[1].Value).ToList();
        }

        public override string ToString() =>
            _template
            .Replace("uml_name", Name)
            .Replace("uml_enum_values", string.Join(",\n", Indexes));
    }
}