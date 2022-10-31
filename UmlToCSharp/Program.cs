// See https://aka.ms/new-console-template for more information
// See https://learn.microsoft.com/ru-ru/dotnet/standard/base-types/regular-expressions
// See https://gitlab.pba.su/visary/issues/-/wikis/domain-plantuml
// See https://www.cisa.gov/uscert/ncas/tips/ST04-015
using UmlToCSharp;
using UmlToCSharp.Validators;

string t =
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
}";

string enumUml =
@"enum DocumentStatus <Статус документа> {
Active
Canceled
}";


var umlData = new Uml();
umlData.EntityInfo();




var r = new EntityValidator(t);
r.IsValid();
Console.WriteLine(r.Error);

Console.WriteLine("Hello, World!");

var r1 = new EnumValidator(enumUml);
r1.IsValid();
Console.WriteLine(r1.Error);
var r2 = new RelationshipValidator("Document::Type --> DocumentType");
r2.IsValid();
Console.WriteLine(r2.Error);

Console.WriteLine("Hello, World!");