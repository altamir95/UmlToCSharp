using System.Text.RegularExpressions;
using UmlToCSharp.Pattern;

namespace UmlToCSharp.Validators
{
    public abstract class BaseUmlValidator
    {
        public BaseUmlValidator(string uml)
        {
            Uml = uml;
        }

        public string? Error { get; protected set; }
        public string Uml { get; private set; }
        
        public abstract bool IsValid();

        protected void LoopInPatterns(string input, PatternItem[] patternArray)
        {
            input = input.Trim();
            var patternsStr = "";

            if (string.IsNullOrEmpty(input)) return;
            else foreach (var p in patternArray) if (!Regex.IsMatch(input, patternsStr += p.Pattern)) Error = p.Message;
        }

        protected string GetInnerGroupFromUml(PatternItem[] patternParts)
        {
            return Regex.Match(Uml, string.Join("", patternParts.Select(c => c.Pattern))).Groups["inner"].Value;
        }
    }
}