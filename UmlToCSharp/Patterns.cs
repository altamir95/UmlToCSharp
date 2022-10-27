using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace UmlToCSharp
{
    public class Patterns
    {
        private static readonly Dictionary<PattternKeys, PatternItem> Dictionary = new()
        {
            { PattternKeys.Space,
                new (
                    regex:  new(@"\s+"),
                    message: "После {l} должно быть пространство")
            },
            { PattternKeys.Comment,
                new (
                    regex: new(@"<[a-zA-Zа-яА-Я0-9\s,.!?()']+>"),
                    message: "Комментарий должеть быть обурнут в <КОММЕНТАРИЙ>, содержать русские или английсике буквы, а так же может содержаьб следующие символы:?, !, (, ), '")
            },
            { PattternKeys.OpenBrace,
                new (
                    regex: new(@"{"),
                    message: "Ожидаеться открывающая фигурная скобка")
            },
            { PattternKeys.CloseBrace,
                new (
                    regex: new(@"}"),
                    message: "Ожидаеться закрывающая фигурная скобка")
            },
            { PattternKeys.DoubleDot,
                new (
                    regex: new(@":"),
                    message: "Ожидаеться двоеточие")
            },

            { PattternKeys.EntityBase,
                new (
                    regex: new(@"^entity"),
                    message: "Срока должн начинаться с entity")
            },
            { PattternKeys.EntityName,
                new (
                    regex: new(@"[A-Z][A-Za-z0-9]+"),
                    message: "Срока должн начинаться с entity и должна быть отделена прабелом от назавния")
            },
            { PattternKeys.EntityInterfaces,
                new (
                    regex: new(@"<<(\s+)?[A-Z][A-Za-z0-9]+(\s+)?(,(\s+)?[A-Z][A-Za-z0-9]+(\s+)?){0,}>>"),
                    message: "Интерфейсы должны быть указаны в скобках (<<интерфейсы>>), а так же должныть быть перечислены через запятую и иметь корректное для интерфейса наименование")
            },
            { PattternKeys.EntityInner,
                new (
                    regex: new(@"(?<props>[^}]+)"),
                    message: "Сущность пуста")
            },
            { PattternKeys.PropReadOnlyState,
                new (
                    regex: new(@"^(\+|\-)"),
                    message: "Поле должно наинаться с указателяна статус 'ReadOnly', это символы: +, -.")
            },
            { PattternKeys.PropName,
                new (
                    regex: new(@"[A-Z][A-Za-z0-9]+"),
                    message: "Наименование поля должно начинаться с большой буквы и состоять только из английских букв и цфир")
            },
            { PattternKeys.PropRequiredState,
                new (
                    regex: new(@"(\*)?"),
                    message: "")
            },
            { PattternKeys.PropType,
                new (
                    regex: new(@"[A-Za-z0-9]+(\[([0-9]+)?\])?"),
                    message: "Тип не соответсвует правилу наименаниятипов")
            },
        };

        public static readonly PatternItem[] entityPatternParts = new[]
        {
            Dictionary[PattternKeys.EntityBase],
            Dictionary[PattternKeys.Space],
            Dictionary[PattternKeys.EntityName],
            Dictionary[PattternKeys.Space],
            Dictionary[PattternKeys.Comment],
            Dictionary[PattternKeys.Space],
            Dictionary[PattternKeys.EntityInterfaces],
            Dictionary[PattternKeys.Space],
            Dictionary[PattternKeys.OpenBrace],
            Dictionary[PattternKeys.EntityInner],
            Dictionary[PattternKeys.CloseBrace],
        };

        public static readonly PatternItem[] propertyPatternParts = new[]
        {
            Dictionary[PattternKeys.PropReadOnlyState],
            Dictionary[PattternKeys.PropName],
            Dictionary[PattternKeys.PropRequiredState],
            Dictionary[PattternKeys.DoubleDot],
            Dictionary[PattternKeys.Space],
            Dictionary[PattternKeys.PropType],
            Dictionary[PattternKeys.Space],
            Dictionary[PattternKeys.Comment],
        };
    }
}
