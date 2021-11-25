using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playground.Core.Infrastructure.SDK.ServiceCommunication.Http.MethodAttributes;

public sealed class PlaygroundHttpPost : PlaygroundHttpAttribute
{
    public PlaygroundHttpPost(string path) : base(path)
    {
    }
}
