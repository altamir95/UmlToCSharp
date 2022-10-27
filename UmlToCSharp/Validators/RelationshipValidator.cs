using UmlToCSharp.Pattern;

namespace UmlToCSharp.Validators
{
    public class RelationshipValidator : BaseUmlValidator
    {
        public RelationshipValidator(string uml) : base(uml)
        {
        }

        public override bool IsValid()
        {
            LoopInPatterns(Uml, PatternParts.relationshipPatternParts);

            return !string.IsNullOrEmpty(Error);
        }
    }
}