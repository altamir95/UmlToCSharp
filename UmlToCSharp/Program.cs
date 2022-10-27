// See https://aka.ms/new-console-template for more information
// See https://learn.microsoft.com/ru-ru/dotnet/standard/base-types/regular-expressions
// See https://gitlab.pba.su/visary/issues/-/wikis/domain-plantuml
// See https://www.cisa.gov/uscert/ncas/tips/ST04-015
using UmlToCSharp;

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


//var umlData = new UmlData();
//umlData.EntityInfo();

Console.WriteLine(new EntityValidator(t).IsValid().ValidationResult());

Console.WriteLine("Hello, World!");