using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UmlToCSharp.Parsers;

namespace UmlToCSharp
{
    public class Uml
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

        public Uml(string input = _testUmlInput)
        {
            Input = input;
            SetNodes();
        }

        public string Input { get; set; }

        public IReadOnlyCollection<string> InputLines { get; private set; }

        public IReadOnlyCollection<string> EntityUml { get; private set; }
        public IReadOnlyCollection<string> RelationshipUml { get; private set; }
        public IReadOnlyCollection<string> NoteUml { get; private set; }
        public IReadOnlyCollection<string> EnumUml { get; private set; }

        public string EntityCSharp { get; set; }
        public string EnumCSharp { get; set; }

        public string File { get; set; }

        private void SetNodes()
        {
            EntityUml = Regex.Matches(input: Input, pattern: "entity([^{]+){([^}]+)}").Select(v => v.Value).ToList();
            RelationshipUml = Regex.Matches(input: Input, pattern: "(.+)::(.+)").Select(v => v.Value).ToList();
            NoteUml = Regex.Matches(input: Input, pattern: "(.+)::(.+)(-->|..>|\\*-)(.+)").Select(v => v.Value).ToList();
            EnumUml = Regex.Matches(input: Input, pattern: "enum([^{]+){([^}]+)}").Select(v => v.Value).ToList();

            EntityCSharp = string.Join(Environment.NewLine, EntityUml.Select(e => new EntityParser(e)));
            EnumCSharp = string.Join(Environment.NewLine, EnumUml.Select(e => new EnumParser(e)));

            File = string.Join(Environment.NewLine, new[] { EntityCSharp, EnumCSharp });
        }

        public void EntityInfo()
        {
            var res = EntityUml.Select(e => new EntityParser(e).ToString());
            var res1 = EnumUml.Select(e => new EnumParser(e).ToString());
            foreach (var item in res)
            {
                Console.WriteLine(item);
            };
            foreach (var item in res1)
            {
                Console.WriteLine(item);
            };
        }

        public override string ToString()
        {
            return File;
        }
    }
}
