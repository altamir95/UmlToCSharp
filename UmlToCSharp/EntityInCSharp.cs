using System.Text.RegularExpressions;

namespace UmlToCSharp
{
    public class EntityInCSharp
    {
        private const string _classTemplate = "public class umlClassName : BaseObject umlInterfaces\n{\numlProps\n}";

        public EntityInCSharp(string entity)
        {
            Entity = entity;
            SetPropsInMatch();
            SetProps();
            SetName();
            SetInterfaces();
        }

        public string Entity { get; private set; }
        public string UmlType { get; private set; }
        public string Name { get; private set; }
        public string Interfaces { get; private set; }
        public MatchCollection PropsInMatch { get; private set; }
        public List<string> Props { get; private set; }

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
}