using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UmlToCSharp
{
    public enum PattternKeys
    {
        Space,
        Comment,
        OpenBrace,
        CloseBrace,
        DoubleDot,

        EntityBase,
        EntityName,
        EntityInterfaces,
        EntityInner,

        PropReadOnlyState,
        PropName,
        PropRequiredState,
        PropType
    }
}
