using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using UmlToCSharp.Pattern;
using UmlToCSharp.Utils;

namespace UmlToCSharp.Parsers
{
    public class EntityParser
    {
        private const string _classTemplate = "public class umlClassName : BaseObject umlInterfaces\n{\numlProps\n}\numlCollectionClasses";
        private const string _classEasyCollectionTemplate =
            $"public class umlClassNameEasyEntry : EasyCollectionEntry<umlL, umlR> "+"{}\n";

        public EntityParser(string entity)
        {
            Entity = entity;
        }

        public string Entity { get; private set; }

        public override string ToString()
        {
            var currentClassName = RegexUtil.GetInnerGroupFromUml(Entity, PatternParts.entityPatternParts, "object_name");
            var innerArray = Regex.Matches(Entity, PatternParts.ToString(PatternParts.propertyPatternParts)).Select(s => new PropertyParser(s.Value));

            var classEasyCollection = "";
            foreach (var item in innerArray)
            {
                if (item.IsNeedToEntryCollection )
                {
                    classEasyCollection += _classEasyCollectionTemplate
                        .Replace("umlClassName", item.Type)
                        .Replace("umlR", item.Type)
                        .Replace("umlL", currentClassName);
                }
            }

            return _classTemplate
              .Replace("umlCollectionClasses", classEasyCollection)
              .Replace("umlClassName", currentClassName)
              .Replace("umlInterfaces", RegexUtil.GetInnerGroupFromUml(Entity, PatternParts.entityPatternParts, "interfaces"))
              .Replace("umlProps", string.Join(Environment.NewLine, innerArray));
        }
    }
}