using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicorn.Core.Infrastructure.Communication.Http.SDK;

[assembly: UnicornAssemblyServiceName("Unicorn.eShop.Discount")]

namespace Unicorn.eShop.Discount.SDK.Services.Http;

[UnicornHttpServiceMarker]
public interface IDiscountService
{
}
