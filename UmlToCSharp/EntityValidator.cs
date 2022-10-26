using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UmlToCSharp
{
    public class ValidatorConroller
    {
        public void Test()
        {
            new EntityValidator("")
                .IsNameEntityStartWithEngUpperLatter()
                .IsEntityNameOnlyEngLetterAndDigit();

        }
    }

    public class EntityValidator : BaseUmlValidator
    {
        public EntityValidator(string uml) : base(uml)
        {
        }

        public void Foo()
        {

        }
    }

    public class BaseUmlValidator
    {
        public List<string> Errors { get; set; } = new List<string>();
        public string Uml { get; set; }
        public BaseUmlValidator(string uml)
        {
            Uml = uml;
        }

        public BaseUmlValidator IsNameEntityStartWithEngUpperLatter()
        {
            var isValid = new Regex(@"^entity\s+[A-Z]").IsMatch(Uml);
            if (!isValid)
                Errors.Add("Назавние сущности должно начинаться с заглавной английской буквы");
            return this;
        }

        public BaseUmlValidator IsEntityNameOnlyEngLetterAndDigit()
        {
            var isValid = new Regex(@"^entity\s+[A-Za-z0-9]+\s").IsMatch(Uml);
            if (!isValid)
                Errors.Add("Назавние сущности должно состоять только из английских букв и цифр");
            return this;
        }

        public BaseUmlValidator IsCommentCorrect()
        {
            var isValid = new Regex(@"^entity\s+.+\s<[^<>]+>").IsMatch(Uml);
            if (!isValid)
                Errors.Add("У сущности обязательно должен быть комментарий в котором нет символов:'<','>'.");
            return this;
        }

        public BaseUmlValidator IsInterfacesCorrect()
        {
            var isValid = new Regex(@"^entity\s+.+\s<[^<>]+>\s+(<<([\s]{0,}[A-Z][A-Za-z0-9]+)?([\s]{0,},[\s]{0,}[A-Z][A-Za-z0-9]+[\s]{0,})+?>>)?").IsMatch(Uml);
            if (!isValid)
                Errors.Add("Если у сущности есть интерфейсы то они обязательно должены быть разделены запятыми, состаять только из букв и цифр а так же наинаться с большой бкувы");
            return this;
        }

        public BaseUmlValidator IsInterfacesCorrect()
        {
            var isValid = new Regex(@"").IsMatch(Uml);
            if (!isValid)
                Errors.Add("");
            return this;
        }
    }
}
