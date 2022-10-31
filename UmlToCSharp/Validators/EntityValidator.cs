using System.Text.RegularExpressions;
using UmlToCSharp.Pattern;

namespace UmlToCSharp.Validators
{
    public class EntityValidator : BaseUmlValidator
    {
        public EntityValidator(string uml) : base(uml) { }

        public override bool IsValid()
        {
            // Провекрка сущности
            LoopInPatterns(Uml, PatternParts.entityPatternParts);

            // Провекрка полей сущности
            foreach (var prop in Regex.Split(GetInnerGroupFromUml(PatternParts.entityPatternParts), Environment.NewLine))
                if (!string.IsNullOrEmpty(Error))
                    LoopInPatterns(prop, PatternParts.propertyPatternParts);

            return string.IsNullOrEmpty(Error);
        }
    }
}