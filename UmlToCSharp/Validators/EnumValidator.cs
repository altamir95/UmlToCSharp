using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UmlToCSharp.Pattern;

namespace UmlToCSharp.Validators
{
    public class EnumValidator : BaseUmlValidator
    {
        public EnumValidator(string uml) : base(uml)
        {
        }

        public override bool IsValid()
        {
            // Провекрка перечисляемого типа
            LoopInPatterns(Uml, PatternParts.enumPatternParts);

            // Провекрка индексов перечисляемого типа
            foreach (var prop in Regex.Split(GetInnerGroupFromUml(PatternParts.enumPatternParts), Environment.NewLine))
                if (!string.IsNullOrEmpty(Error))
                    LoopInPatterns(prop, PatternParts.enumIndexesPatternParts);

            return !string.IsNullOrEmpty(Error);
        }
    }
}