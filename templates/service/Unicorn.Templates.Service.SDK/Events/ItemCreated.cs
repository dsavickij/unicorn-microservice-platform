using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicorn.Templates.Service.SDK.Events;

public record ItemCreated
{
    public Guid ItemId { get; set; }
}
