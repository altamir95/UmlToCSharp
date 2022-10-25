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
        private string _propTemplate = @"public umlTypeProp umlNameProp { get; set; }";

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

        private const string _relationshipsPattern =
            @"(.+)::(.+)\s(\*-|..>)\s(.+)";

        private const string _entitiesPattern =
            @"entity\s(.+)\s<(.+)>(\s<<((.+),[\s]?){0,}>>)?(\G(\u002b|\u002d)(.+)[\u002a]?:\s(.+)(\s<(.+)>)?){1,}}";

        private const string _notesPattern =
            @"note\s(left|right)\sof\s(.+)::(.+)\n(.+)\nend note";

        public UmlData(string input = _testUmlInput)
        {
            Input = input;
            SetInputLines();
            SetNodes();
        }

        public string Input { get; set; }

        public IReadOnlyCollection<string> InputLines { get; private set; }
        public IReadOnlyCollection<string> Entities { get; private set; }
        public IReadOnlyCollection<string> Relationships { get; private set; }
        public IReadOnlyCollection<string> Notes { get; private set; }

        private void SetInputLines()
            => InputLines = new List<string>(Regex.Split(Input, Environment.NewLine));

        public bool IsRelationshipsValid()
            => Relationships.All(rs => Regex.IsMatch(rs, _relationshipsPattern));

        public bool IsEntitiesValid()
            => Entities.All(e => Regex.IsMatch(e, _entitiesPattern));

        public bool IsNotesValid()
            => Notes.All(n => Regex.IsMatch(n, _notesPattern));

        public void EntityInfo()
        {
            var newClaccStr = _classTemplate;
            var res = Entities.Select(e => new EntityToClass(e).ToString());
            var res1 =Enums.Select(e => new UmlEnum(e).ToString());
            foreach (var item in res)
            {
                Console.WriteLine(item);
            };
        }

        private void SetNodes()
        {
            string[] InputLineArray = InputLines.ToArray();

            var entities = new List<string>();
            var relationships = new List<string>();
            var notes = new List<string>();

            var index = 0;
            while (InputLineArray.Length != index)
            {

                if (InputLineArray[index].Contains('{'))
                {
                    var entity = new StringBuilder();
                    for (; index < InputLineArray.Length; index++)
                    {
                        entity.Append($"{InputLineArray[index]}\n");
                        if (InputLineArray[index].Contains('}')) break;
                    }
                    if (!entity.ToString().Contains('}'))
                        throw new Exception("КРЯ");
                    entities.Add(entity.ToString());
                }
                else if (
                    InputLineArray[index].Contains("-->") ||
                    InputLineArray[index].Contains("..>") ||
                    InputLineArray[index].Contains("*-")
                    )
                {
                    relationships.Add(InputLineArray[index]);
                }
                else if (InputLineArray[index].Contains("note"))
                {
                    var note = new StringBuilder();
                    for (; index < InputLineArray.Length; index++)
                    {
                        note.Append($"{InputLineArray[index].TrimStart()}\n");
                        if (InputLineArray[index].Contains("end")) break;
                    }
                    if (!note.ToString().Contains("end"))
                        throw new Exception("КРЯ");
                    notes.Add(note.ToString());
                }

                ++index;
            }


            Entities = entities;
            Relationships = relationships;
            Notes = notes;
        }
    }
}
