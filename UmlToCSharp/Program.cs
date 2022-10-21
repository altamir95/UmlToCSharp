// See https://aka.ms/new-console-template for more information
using System.Text.RegularExpressions;

Console.WriteLine("Hello, World!");

string text = @"First line
entity Document <Документ> <<Lookup, Workflow>> {
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

Document::Type        --> DocumentType
Document::Status ..> DocumentStatus
Document::Items *- DocumentItem


End line";

text = DropFromString(text);

List<string> list = new List<string>(Regex.Split(text, Environment.NewLine));
list.ForEach(x => x.Trim());

List<string> relationships;
List<string> entities;
List<string> comments;

string currentEntity;
string currentRelationship;
string currentComment;

foreach (var item in list)
{
    if (item.Contains("entity"))
    {
        Regex regex = new Regex(@"entity {0,} <{0,}> <<{0,}>> {");
        var t = regex.IsMatch(item);
    }
    Console.WriteLine(item);
}


string DropFromString(string str)
{
    while (str.Contains("  ")) { str = str.Replace("  ", " "); }
    return str;
}