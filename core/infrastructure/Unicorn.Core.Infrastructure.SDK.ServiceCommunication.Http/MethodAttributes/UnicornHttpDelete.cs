using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Http.MethodAttributes;

public sealed class UnicornHttpDelete : UnicornHttpAttribute
{
    public UnicornHttpDelete(string pathTemplate) : base(pathTemplate)
    {
    }
}
