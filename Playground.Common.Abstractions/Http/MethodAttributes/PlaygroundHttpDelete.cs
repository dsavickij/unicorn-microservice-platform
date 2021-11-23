using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playground.Common.SDK.Abstractions.Http.MethodAttributes;

public sealed class PlaygroundHttpDelete : PlaygroundHttpAttribute
{
    public PlaygroundHttpDelete(string path) : base(path)
    {
    }
}
