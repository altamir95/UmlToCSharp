namespace UmlToCSharp.Pattern
{
    public static class PatternParts
    {
        public static readonly PatternItem[] entityPatternParts = new[]
        {
            Patterns.Dictionary[PattternKeys.EntityBase],
            Patterns.Dictionary[PattternKeys.Space],
            Patterns.Dictionary[PattternKeys.ObjectName],
            Patterns.Dictionary[PattternKeys.Space],
            Patterns.Dictionary[PattternKeys.Comment],
            Patterns.Dictionary[PattternKeys.Space],
            Patterns.Dictionary[PattternKeys.EntityInterfaces],
            Patterns.Dictionary[PattternKeys.Space],
            Patterns.Dictionary[PattternKeys.OpenBrace],
            Patterns.Dictionary[PattternKeys.ObjectInner],
            Patterns.Dictionary[PattternKeys.CloseBrace],
        };

        public static readonly PatternItem[] enumPatternParts = new[]
        {
            Patterns.Dictionary[PattternKeys.EnumBase],
            Patterns.Dictionary[PattternKeys.Space],
            Patterns.Dictionary[PattternKeys.ObjectName],
            Patterns.Dictionary[PattternKeys.Space],
            Patterns.Dictionary[PattternKeys.Comment],
            Patterns.Dictionary[PattternKeys.Space],
            Patterns.Dictionary[PattternKeys.OpenBrace],
            Patterns.Dictionary[PattternKeys.ObjectInner],
            Patterns.Dictionary[PattternKeys.CloseBrace],
        };

        public static readonly PatternItem[] propertyPatternParts = new[]
        {
            Patterns.Dictionary[PattternKeys.PropReadOnlyState],
            Patterns.Dictionary[PattternKeys.ObjectName],
            Patterns.Dictionary[PattternKeys.PropRequiredState],
            Patterns.Dictionary[PattternKeys.DoubleDot],
            Patterns.Dictionary[PattternKeys.Space],
            Patterns.Dictionary[PattternKeys.PropType],
            Patterns.Dictionary[PattternKeys.Space],
            Patterns.Dictionary[PattternKeys.Comment],
        };

        public static readonly PatternItem[] enumIndexesPatternParts = new[]
        {
            Patterns.Dictionary[PattternKeys.ObjectName]
        };

        public static readonly PatternItem[] relationshipPatternParts = new[]
        {
            Patterns.Dictionary[PattternKeys.ObjectName],
            Patterns.Dictionary[PattternKeys.DoubleDot],
            Patterns.Dictionary[PattternKeys.DoubleDot],
            Patterns.Dictionary[PattternKeys.ObjectName],
            Patterns.Dictionary[PattternKeys.Space],
            Patterns.Dictionary[PattternKeys.RelationshipSymbol],
            Patterns.Dictionary[PattternKeys.Space],
            Patterns.Dictionary[PattternKeys.ObjectName],
        };

        public static string ToString(this PatternItem[] parts) => string.Join(string.Empty, parts.Select(r => r.Pattern));
    }
}