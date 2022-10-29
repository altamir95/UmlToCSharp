using System;
using System.Text.RegularExpressions;

namespace UmlToCSharp.Utils
{
    public class TypeConvertor
    {
        private Dictionary<Regex, string> _typeWithRegex = new Dictionary<Regex, string>()
        {
            { new Regex(@"string(\[([0-9]+)\])?"),"string"},
            { new Regex(@"text"),"string"},
            { new Regex(@"bool"),"bool"},
            { new Regex(@"number"),"int"},
            { new Regex(@"double"),"double"},
            { new Regex(@"Location"),"Location"},
            { new Regex(@"file(\[])?"),"string"},
        };

        public TypeConvertor(string type)
        {
            SetProps(type);
        }

        public bool IsArray { get; set; }
        public string UmlType { get; set; }
        public string CSharpType { get; set; }
        public bool IsEnumOrEntity { get; set; }

        private void SetProps(string type)
        {
            UmlType = type;

            foreach (var kv in _typeWithRegex)
            {
                if (kv.Key.IsMatch(type))
                {
                    CSharpType = kv.Value;
                    break;
                }
            }

            if (string.IsNullOrEmpty(CSharpType))
            {
                CSharpType = Regex.Match(type, @"[A-Z][a-zA-Z0-9]*").Value;
                IsEnumOrEntity = true;
                IsArray= Regex.IsMatch(type, @"[^\[\]]{1,}\[\]");
            }
        }
    }
}