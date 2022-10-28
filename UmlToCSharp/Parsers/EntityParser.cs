using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using UmlToCSharp.Pattern;

namespace UmlToCSharp.Parsers
{
    public class EntityParser
    {
        private const string _classTemplate = "public class umlClassName : BaseObject umlInterfaces\n{\numlProps\n}";

        public EntityParser(string entity)
        {
            Entity = entity;
        }
        public string Entity { get; private set; }

        public override string ToString()
        {
            var propPattern = PatternParts.ToString(PatternParts.propertyPatternParts);

            var innerArray = Regex.Matches(Entity, propPattern).Select(s => new PropertyParser(s.Value).ToString());

            return _classTemplate
              .Replace("umlClassName", GetInnerGroupFromUml("object_name"))
              .Replace("umlInterfaces", GetInnerGroupFromUml("interfaces"))
              .Replace("umlProps", string.Join(Environment.NewLine, innerArray));
        }

        protected string GetInnerGroupFromUml(string groupName)
        {
            return Regex.Match(Entity, PatternParts.ToString(PatternParts.entityPatternParts)).Groups[groupName].Value;
        }
    }
}
