using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UmlToCSharp.Parsers;

namespace UmlToCSharp
{
    public class UmlData
    {
        private const string _testUmlInput = 
@"entity Document <Документ> <<Lookup, Workflow>> {
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
Active
Canceled
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
            SetNodes();
        }

        public string Input { get; set; }

        public IReadOnlyCollection<string> InputLines { get; private set; }
        public IReadOnlyCollection<string> Entities { get; private set; }
        public IReadOnlyCollection<string> Relationships { get; private set; }
        public IReadOnlyCollection<string> Notes { get; private set; }
        public IReadOnlyCollection<string> Enums { get; private set; }

        private void SetNodes()
        {
            Entities = Regex.Matches(input: Input, pattern: "entity([^{]+){([^}]+)}").Select(v => v.Value).ToList();
            Relationships = Regex.Matches(input: Input, pattern: "(.+)::(.+)").Select(v => v.Value).ToList();
            Notes = Regex.Matches(input: Input, pattern: "(.+)::(.+)(-->|..>|\\*-)(.+)").Select(v => v.Value).ToList();
            Enums = Regex.Matches(input: Input, pattern: "enum([^{]+){([^}]+)}").Select(v => v.Value).ToList();
        }

        public void EntityInfo()
        {
            var res = Entities.Select(e => new EntityParser(e).ToString());
            var res1 = Enums.Select(e => new EnumParser(e).ToString());
            foreach (var item in res)
            {
                Console.WriteLine(item);
            };
            foreach (var item in res1)
            {
                Console.WriteLine(item);
            };
        }

    }
}
