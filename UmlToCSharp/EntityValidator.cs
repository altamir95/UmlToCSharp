using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using R = System.Text.RegularExpressions.Regex;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace UmlToCSharp
{
    public class ValidatorConroller
    {
        public void Test()
        {
            Console.WriteLine(new EntityValidator("").IsValid().ValidationResult());

        }
    }

    public class EntityValidator
    {
        //(^entity)(\s+)([A-Z][A-Za-z0-9]+)(\s+)(<[a-zA-Zа-яА-Я0-9\s,.!?()']+>)(\s+)(<<(\s+)?[A-Z][A-Za-z0-9]+(\s+)?(,(\s+)?[A-Z][A-Za-z0-9]+(\s+)?){0,}>>)(\s+)({)(?<props>[^}]+)(})
        private string error;

        private static PatternItem Space(string l) => new(new(@"\s+"), $"После {l} должно быть пространство");

        private static readonly PatternItem _base = new(new(@"^entity"), "Срока должн начинаться с entity");
        private static readonly PatternItem _entityName = new(new(@"[A-Z][A-Za-z0-9]+"), "Срока должн начинаться с entity и должна быть отделена прабелом от назавния");
        private static readonly PatternItem _entityComment = new(new(@"<[a-zA-Zа-яА-Я0-9\s,.!?()']+>"), "Комментарий должеть быть обурнут в <КОММЕНТАРИЙ>, содержать русские или английсике буквы, а так же может содержаьб следующие символы:?, !, (, ), '");
        private static readonly PatternItem _interfaces = new(new(@"<<(\s+)?[A-Z][A-Za-z0-9]+(\s+)?(,(\s+)?[A-Z][A-Za-z0-9]+(\s+)?){0,}>"), "Интерфейсы должны быть указаны в скобках (<<интерфейсы>>), а так же должныть быть перечислены через запятую и иметь корректное для интерфейса наименование");
        private static readonly PatternItem _openBrace = new(new(@"{"), "Ожидаеться открывающая фигурная скобка");
        private static readonly PatternItem _entityInner = new(new(@"(?<props>[^}]+)"), "Сущность пуста");
        private static readonly PatternItem _closeBrace = new(new(@"}"), "Ожидаеться закрывающая фигурная скобка");

        private static readonly PatternItem _propReadOnlyState = new(new(@"^(\+|\-)"), "TODO");
        private static readonly PatternItem _propName = new(new(@"[A-Z][A-Za-z0-9]+"), "TODO");
        private static readonly PatternItem _propRequiredState = new(new(@"(\*)?"), "");
        private static readonly PatternItem _doubleDot = new(new(@":"), "Ожидаеться двоеточие");

        public readonly PatternItem[] entityPatterns = new[]
        {
            _base,
            Space("entity"),
            _entityName,
            Space("entity"),
            _entityComment,
            Space("entity"),
            _interfaces,
            Space("entity"),
            _openBrace,
            _entityInner,
            _closeBrace
        };

        public readonly PatternItem[] propertyPatterns = new[]
        {
            _propReadOnlyState,
            _propName,
            _propRequiredState,
            _doubleDot,
            Space("двоеточие"),

        };

        public string Uml { get; set; }
        public EntityValidator(string uml)
        {
            Uml = uml;

        }

        public EntityValidator IsValid()
        {
            StringBuilder currentPattern = new StringBuilder("");
            Match currentMatch = null;
            for (int i = 0; i < entityPatterns.Length; i++)
            {
                currentPattern.Append(entityPatterns[i].Pattern);
                if ((currentMatch = R.Match(Uml, currentPattern.ToString())).Value == "") { error = entityPatterns[i].Message; return this; }
            }
            IsPropsValid(currentMatch.Groups["props"].Value);

            return this;
        }

        private void IsPropsValid(string props)
        {
            //(^(\+|\-))([A-Z][A-Za-z0-9]+)(\*)?(:)(\s+)([A-Za-z0-9]+\[[0-9]\])
            var propLines = new List<string>(R.Split(props, Environment.NewLine));

            foreach (var prop in propLines)
            {
                var propAfterTrim = prop.Trim();
                if (string.IsNullOrWhiteSpace(propAfterTrim) || string.IsNullOrEmpty(propAfterTrim)) continue;

                var currentPattern = new StringBuilder("");
                for (int i = 0; i < propertyPatterns.Length; i++)
                {
                    currentPattern.Append(propertyPatterns[i].Pattern);
                    if (R.Match(prop, currentPattern.ToString()).Value == "") { error = propertyPatterns[i].Message; return; }
                }
            }
        }

        public string ValidationResult() => error;
    }

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
