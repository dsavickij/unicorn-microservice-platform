using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Http.Attributes.ParameterBindings;

[AttributeUsage(AttributeTargets.Parameter)]
public class UnicornFromUriAttribute : Attribute
{
}
