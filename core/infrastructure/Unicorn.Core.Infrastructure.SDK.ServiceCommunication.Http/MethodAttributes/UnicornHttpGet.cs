using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Http.MethodAttributes;

public sealed class UnicornHttpGet : UnicornHttpAttribute
{
    public UnicornHttpGet(string pathTemplate) : base(pathTemplate)
    {
    }
}
