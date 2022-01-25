using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicorn.Core.Infrastructure.Communication.Http.SDK.Attributes.HttpMethods;

[AttributeUsage(AttributeTargets.Method)]
public class UnicornOneWayAttribute : Attribute
{
}
