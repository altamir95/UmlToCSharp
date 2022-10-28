namespace UmlToCSharp.Pattern
{
    public class Patterns
    {
        public static readonly Dictionary<PattternKeys, PatternItem> Dictionary = new()
        {
            { PattternKeys.Space,
                new (
                    regex:  new(@"\s+"),
                    message: "После {l} должно быть пространство")
            },
            { PattternKeys.Comment,
                new (
                    regex: new(@"(?<comment><[a-zA-Zа-яА-Я0-9\s,.!?()']+>)"),
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
            { PattternKeys.ObjectName,
                new (
                    regex: new(@"(?<object_name>[A-Z][A-Za-z0-9]+)"),
                    message: "Намиенование должно состоять из букв и цифр")
            },
            { PattternKeys.ObjectInner,
                new (
                    regex: new(@"(?<inner>[^}]+)"),
                    message: "Объект пуст")
            },

            { PattternKeys.EnumBase,
                new (
                    regex: new(@"^enum"),
                    message: "Срока должн начинаться с enum")
            },

            { PattternKeys.EntityBase,
                new (
                    regex: new(@"^entity"),
                    message: "Срока должн начинаться с entity")
            },
            { PattternKeys.EntityInterfaces,
                new (
                    regex: new(@"<<(\s+)?[A-Z][A-Za-z0-9]+(\s+)?(,(\s+)?[A-Z][A-Za-z0-9]+(\s+)?){0,}>>"),
                    message: "Интерфейсы должны быть указаны в скобках (<<интерфейсы>>), а так же должныть быть перечислены через запятую и иметь корректное для интерфейса наименование")
            },

            { PattternKeys.PropReadOnlyState,
                new (
                    regex: new(@"(?<read_only>^(\+|\-))"),
                    message: "Поле должно наинаться с указателяна статус 'ReadOnly', это символы: +, -.")
            },
            { PattternKeys.PropRequiredState,
                new (
                    regex: new(@"(?<required>(\*)?)"),
                    message: "")
            },
            { PattternKeys.PropType,
                new (
                    regex: new(@"(?<prop_type>[A-Za-z0-9]+(\[([0-9]+)?\])?)"),
                    message: "Тип не соответсвует правилу наименаниятипов")
            },

            { PattternKeys.RelationshipSymbol,
                new (
                    regex: new(@"(-->|\.\.>|\*-)"),
                    message: "Ожидеяться один из следующих сиволов: '-->', '..>', '*-'")
            },
        };
    }
}