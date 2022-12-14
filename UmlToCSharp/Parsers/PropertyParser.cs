using System;
using System.Text.RegularExpressions;
using UmlToCSharp.Pattern;
using UmlToCSharp.Utils;

namespace UmlToCSharp.Parsers
{
    public class PropertyParser
    {
        public PropertyParser(string propUml)
        {
            SetPropsFromUml(propUml);
        }

        public string Type { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public bool ReadOnly { get; set; }
        public bool Required { get; set; }
        public bool IsNeedToEntryCollection { get; set; }
        public List<string> Attributes { get; set; } = new List<string>();

        private void SetPropsFromUml(string umlProp)
        {
            var propMatch = Regex.Match(input: umlProp, pattern: PatternParts.ToString(PatternParts.propertyPatternParts));


            Name = propMatch.Groups["object_name"].Value;
            Comment = propMatch.Groups["comment"].Value;
            ReadOnly = propMatch.Groups["read_only"].Value != "+";
            Required = propMatch.Groups["required"].Value == "*";
            var t = new TypeConvertor(propMatch.Groups["prop_type"].Value);
            Type = t.CSharpType;
            IsNeedToEntryCollection = t.IsEnumOrEntity && t.IsArray;
            SetAttributes(propMatch.Groups["prop_type"].Value);
        }

        private void SetAttributes(string typeInUml)
        {
            var dvParams = new List<string>();
            if (ReadOnly)
                dvParams.Add("ReadOnly = true");
            if (Required)
                dvParams.Add("Required = true");

            Attributes.Add($"DetailView({string.Join(", ", dvParams)})");
            Attributes.Add("ListView");

            switch (true)
            {
                case bool _ when Regex.IsMatch(typeInUml, @"string(\[([0-9]+)\])?"):
                    var m = Regex.Match(typeInUml, @"string(\[([0-9]+)\])?");
                    var length = m.Groups[3].Value == "" ? "255" : m.Groups[3].Value;
                    var attributeMaxLength = $"MaxLength({length})";
                    Attributes.Add(attributeMaxLength);
                    break;

                case bool _ when Regex.IsMatch(typeInUml, @"file(\[])?"):
                    var mFile = Regex.Match(typeInUml, @"file(\[])?");
                    var attributeFileStorageLinkParam = mFile.Groups[1].Value == "[]" ? "Multiple = true" : "";
                    Attributes.Add($"FileStorageLink({attributeFileStorageLinkParam})");
                    break;

                default:
                    break;
            }
        }

        public override string ToString()
        {
            return $"[{string.Join(",", Attributes)}]\npublic {Type} {Name} " + "{get; set;}";
        }
    }
}