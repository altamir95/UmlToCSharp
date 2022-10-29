using System.Text.RegularExpressions;
using UmlToCSharp.Pattern;
using UmlToCSharp.Utils;

namespace UmlToCSharp.Parsers
{
    internal class EnumParser
    {
        private const string _template = "public enum uml_name\n    {\n        uml_enum_values\n    }";

        public EnumParser(string enumStr)
        {
            EnumStr = enumStr;
        }

        public string EnumStr { get; set; }

        public override string ToString() =>
            _template
            .Replace("uml_name", RegexUtil.GetInnerGroupFromUml(EnumStr,PatternParts.enumPatternParts, "object_name") )
            .Replace("uml_enum_values", RegexUtil.GetInnerGroupFromUml(",\n", EnumStr, Patterns.Dictionary[PattternKeys.EnumItems].ToString(), "enum_item") );
    }
}