using System.Text.RegularExpressions;

namespace UmlToCSharp
{
    public class EntityToClass
    {
        private const string _classTemplate = "public class umlClassName : BaseObject umlInterfaces\n{\numlProps\n}";
        public string Entity { get; private set; }
        public MatchCollection PropsInMatch { get; private set; }
        public string UmlType { get; private set; }
        public string Name { get; private set; }
        public string Interfaces { get; private set; }
        public List<string> Props { get; private set; }

        public EntityToClass(string entity)
        {
            Entity = entity;
            SetPropsInMatch();
            SetProps();
            SetName();
            SetInterfaces();
        }

        public void SetPropsInMatch()
        {
            PropsInMatch = Regex.Matches(Entity, UmlProp.pattern.ToString());
        }

        public void SetProps()
        {
            Props = PropsInMatch.Select(m => new UmlProp(m.Value).ToString()).ToList();
        }

        public void SetName()
        {
            Name = Regex.Match(input: Entity, pattern: @"entity\s([^\s]+)").Groups[1].Value;
        }

        public void SetInterfaces()
        {
            var allInterfaces = Regex.Match(input: Entity, pattern: @"<<(.+)>>").Groups[1].Value;
            var interfacesStr = Regex.Matches(input: allInterfaces, pattern: @"([^\s,]+)").Select(v => ", " + v.Groups[1].Value);
            Interfaces = string.Join("", interfacesStr);
        }

        public override string ToString() => 
            _classTemplate
            .Replace("umlClassName", Name)
            .Replace("umlInterfaces", Interfaces)
            .Replace("umlProps", string.Join(Environment.NewLine, Props));

    }

    public class UmlProp
    {
        public UmlProp(string prop)
        {
            SetPropMatch(prop);
            PutGropsInProps();
            InitAttributes();
            SetAttribute();
            SetCSharpType();
        }
        private void SetPropMatch(string prop)
        {
            PropMatch = Regex.Match(input: prop, pattern: pattern.ToString());
        }

        private string GetPropMatchValue(int i)
        {
            return PropMatch.Groups[i].Value;
        }

        private void PutGropsInProps()
        {
            ReadOnlyStr = GetPropMatchValue(1);
            Name = GetPropMatchValue(2);
            RequiredStr = GetPropMatchValue(3);
            Type = GetPropMatchValue(4);
            Comment = GetPropMatchValue(5);

            ReadOnly = ReadOnlyStr != "+";
            Required = RequiredStr == "*";
        }

        private void InitAttributes()
        {
            var dvParams = new List<string>();
            if (ReadOnly)
                dvParams.Add("ReadOnly = true");
            if (Required)
                dvParams.Add("Required = true");

            var dv = $"DetailView({string.Join(", ", dvParams)})";
            Attributes.Add(dv);
            Attributes.Add("ListView");
        }

        private void SetAttribute()
        {
            switch (true)
            {
                case bool _ when Regex.IsMatch(Type, @"string(\[([0-9]+)\])?"):
                    var m = Regex.Match(Type, @"string(\[([0-9]+)\])?");
                    var length = m.Groups[3].Value == "" ? "255" : m.Groups[3].Value;
                    var attributeMaxLength = $"MaxLength({length})";
                    Attributes.Add(attributeMaxLength);
                    break;

                case bool _ when Regex.IsMatch(Type, @"file(\[])?"):
                    var mFile = Regex.Match(Type, @"file(\[])?");
                    var attributeFileStorageLinkParam = mFile.Groups[1].Value == "[]" ? "Multiple = true" : "";
                    Attributes.Add($"FileStorageLink({attributeFileStorageLinkParam})");
                    break;

                default:
                    break;
            }
        }

        private void SetCSharpType() => CSharpType = this.Type switch
        {
            var _ when Regex.IsMatch(Type, @"string(\[([0-9]+)\])?") => "string",
            var _ when Regex.IsMatch(Type, @"text") => "string",
            var _ when Regex.IsMatch(Type, @"bool") => "bool",
            var _ when Regex.IsMatch(Type, @"number") => "int",
            var _ when Regex.IsMatch(Type, @"double") => "double",
            var _ when Regex.IsMatch(Type, @"Location") => "Location",
            var _ when Regex.IsMatch(Type, @"file(\[])?") => "string",
            _ => Type
        };

        public static readonly Regex pattern = new("(\\+|\\-)([^\\*\\:]+)([\\*]?):\\s(.+)\\s<([^<>]+)>");

        public Match PropMatch { get; set; }
        public string CSharpType { get; set; }
        public List<string> Attributes { get; set; }
        public string ReadOnlyStr { get; set; }
        public bool ReadOnly { get; set; }
        public string Name { get; set; }
        public string RequiredStr { get; set; }
        public bool Required { get; set; }
        public string Type { get; set; }
        public string Comment { get; set; }

        public override string ToString() =>
             $"[{string.Join(",", Attributes)}]\npublic {CSharpType} {Name} " + "{get; set;}";
    }
}
