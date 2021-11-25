using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playground.Core.Infrastructure.SDK.ServiceCommunication.Http.MethodAttributes;

public sealed class PlaygroundHttpPut : PlaygroundHttpAttribute
{
    public PlaygroundHttpPut(string path) : base(path)
    {
    }
}
