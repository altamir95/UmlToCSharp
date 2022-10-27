using R = System.Text.RegularExpressions.Regex;

namespace UmlToCSharp
{
    public class PatternItem
    {
        public PatternItem(R regex, string message)
        {
            Message = message;
            Pattern = regex;
        }

        public R Pattern { get; private set; }
        public string Message { get; private set; }
    }
}