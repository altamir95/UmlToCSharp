using System.Text.RegularExpressions;

namespace UmlToCSharp.Pattern
{
    public class PatternItem
    {
        public PatternItem(Regex regex, string message)
        {
            Message = message;
            Pattern = regex;
        }

        public Regex Pattern { get; private set; }
        public string Message { get; private set; }

        public override string ToString()
        {
            return Pattern.ToString();
        }
    }
}