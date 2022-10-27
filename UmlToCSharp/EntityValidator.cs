using System.Text;
using R = System.Text.RegularExpressions.Regex;
using System.Text.RegularExpressions;

namespace UmlToCSharp
{
    public class EntityValidator
    {
        private string error;

        public string Uml { get; set; }
        public EntityValidator(string uml)
        {
            Uml = uml;

        }

        public EntityValidator IsValid()
        {
            StringBuilder currentPattern = new StringBuilder("");
            Match currentMatch = null;
            for (int i = 0; i < Patterns.entityPatternParts.Length; i++)
            {
                currentPattern.Append(Patterns.entityPatternParts[i].Pattern);
                if ((currentMatch = R.Match(Uml, currentPattern.ToString())).Value == "") { error = Patterns.entityPatternParts[i].Message; return this; }
            }
            IsPropsValid(currentMatch.Groups["props"].Value);

            return this;
        }

        private void IsPropsValid(string props)
        {
            var propLines = new List<string>(R.Split(props, Environment.NewLine));

            foreach (var prop in propLines)
            {
                var propAfterTrim = prop.Trim();
                if (string.IsNullOrWhiteSpace(propAfterTrim) || string.IsNullOrEmpty(propAfterTrim)) continue;

                var currentPattern = new StringBuilder("");
                for (int i = 0; i < Patterns.propertyPatternParts.Length; i++)
                {
                    currentPattern.Append(Patterns.propertyPatternParts[i].Pattern);
                    if (R.Match(prop, currentPattern.ToString()).Value == "") { error = Patterns.propertyPatternParts[i].Message; return; }
                }
            }
        }

        public string ValidationResult() => error;
    }
}