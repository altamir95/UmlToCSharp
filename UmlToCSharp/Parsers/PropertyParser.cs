using System;
using System.Text.RegularExpressions;
using UmlToCSharp.Pattern;

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
        public List<string> Attributes { get; set; } = new List<string>();

        private void SetPropsFromUml(string umlProp)
        {
            var propMatch = Regex.Match(input: umlProp, pattern: PatternParts.ToString(PatternParts.propertyPatternParts));

            Name = propMatch.Groups["object_name"].Value;
            Comment = propMatch.Groups["comment"].Value;

            ReadOnly = propMatch.Groups["read_only"].Value != "+";
            Required = propMatch.Groups["required"].Value == "*";

            SetAttributes(propMatch.Groups["prop_type"].Value);
            SetType(propMatch.Groups["prop_type"].Value);
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

        private void SetType(string typeInUml) => Type = typeInUml switch
        {
            var _ when Regex.IsMatch(typeInUml, @"string(\[([0-9]+)\])?") => "string",
            var _ when Regex.IsMatch(typeInUml, @"text") => "string",
            var _ when Regex.IsMatch(typeInUml, @"bool") => "bool",
            var _ when Regex.IsMatch(typeInUml, @"number") => "int",
            var _ when Regex.IsMatch(typeInUml, @"double") => "double",
            var _ when Regex.IsMatch(typeInUml, @"Location") => "Location",
            var _ when Regex.IsMatch(typeInUml, @"file(\[])?") => "string",
            _ => typeInUml
        };

        public override string ToString()
        {
            return $"[{string.Join(",", Attributes)}]\npublic {Type} {Name} " + "{get; set;}";
        }
    }
}