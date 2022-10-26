using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UmlToCSharp
{
    public class UmlData
    {
        private string _classTemplate = @"public class umlClassName : BaseObject umlInterfaces
{
    umlProps
}";

        private const string _testUmlInput = @"entity Document <Документ> <<Lookup, Workflow>> {
    +Title*: string[255] <Наименование>
    +Description: text <Описание>   
    -Creator: User <Автор>   
    -CreationDate: datetime <Дата создания>
    -Status: DocumentStatus <Статус>
    +Performers: User[] <Исполнители>
    +Items: DocumentItem[] <Записи>
    +Type*: DocumentType <Тип>
    +Files: file[] <Файлы>
}

entity DocumentItem <<Slave>> {
    +Name*: string[100] <Наименование>
    +Price: currency <Стоимость>
}

entity DocumentType <Тип документа> <<Lookup>> {
    +Title*: string[255] <Наименование>
}

enum DocumentStatus <Статус документа> {
    Active,
    Canceled,
}

note left of Document::Creator 
    при создании
    инициализируем тек. пользователем 
end note

Document::Type --> DocumentType
Document::Status ..> DocumentStatus
Document::Items *- DocumentItem";

        public UmlData(string input = _testUmlInput)
        {
            Input = input;
            SetInputLines();
            SetNodes();
        }

        public string RelationshipsPattern = new Regex(@"([a-z0-9A-Z]+)::([a-z0-9A-Z]+)\s(-->|..>|\*-)\s([a-z0-9A-Z]+)").ToString();
        public string EnumPattern = new Regex(@"(enum)\s([a-z0-9A-Z]+)\s<([a-z0-9A-Zа-яА-Я\s]+)>\s{\n((\s+[A-Za-z]+\n)+)}").ToString();
        public string EntityPattern = new Regex(@"(entity)\s[A-Z][a-z0-9A-Z]+\s(<([a-z0-9A-Zа-яА-Я\s]+)>\s)?(<<[a-z0-9A-Z\s,]+>>\s)?{\n((\s+(\+|\-)([^\*\:]+)([\*]?):\s(.+)\s<([^<>]+)>\n)+)}").ToString();
        //public string NotePattern = new Regex(@"(entity)\s[A-Z][a-z0-9A-Z]+\s(<([a-z0-9A-Zа-яА-Я\s]+)>\s)?(<<[a-z0-9A-Z\s,]+>>\s)?{\n((\s+(\+|\-)([^\*\:]+)([\*]?):\s(.+)\s<([^<>]+)>\n)+)}").ToString();

        public string Input { get; set; }

        public IReadOnlyCollection<string> InputLines { get; private set; }
        public IReadOnlyCollection<string> Entities { get; private set; }
        public IReadOnlyCollection<string> Relationships { get; private set; }
        public IReadOnlyCollection<string> Notes { get; private set; }
        public IReadOnlyCollection<string> Enums { get; private set; }

        private void SetInputLines()
            => InputLines = new List<string>(Regex.Split(Input, Environment.NewLine));

        public void EntityInfo()
        {
            var newClaccStr = _classTemplate;
            var res = Entities.Select(e => new EntityToClass(e).ToString());
            var res1 = Enums.Select(e => new UmlEnum(e).ToString());
            foreach (var item in res)
            {
                Console.WriteLine(item);
            };
            foreach (var item in res1)
            {
                Console.WriteLine(item);
            };
        }

        public void IsUmlValid()
        {
            var result = Regex.IsMatch(
                input: Input,
                pattern: $@"(({RelationshipsPattern})|({EnumPattern})|({EntityPattern})\n)" + "{0,}"
                );
            Console.WriteLine();
        }

        private void SetNodes()
        {
            Entities = Regex.Matches(input: Input, pattern: "entity([^{]+){([^}]+)}").Select(v => v.Value).ToList();
            Relationships = Regex.Matches(input: Input, pattern: "(.+)::(.+)").Select(v => v.Value).ToList();
            Notes = Regex.Matches(input: Input, pattern: "(.+)::(.+)(-->|..>|\\*-)(.+)").Select(v => v.Value).ToList();
            Enums = Regex.Matches(input: Input, pattern: "enum([^{]+){([^}]+)}").Select(v => v.Value).ToList();
        }
    }
}
