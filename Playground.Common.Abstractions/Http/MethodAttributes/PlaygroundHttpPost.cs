using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playground.Common.SDK.Abstractions.Http.MethodAttributes;

public sealed class PlaygroundHttpPost : PlaygroundHttpAttribute
{
    public PlaygroundHttpPost(string path) : base(path)
    {
    }
}
