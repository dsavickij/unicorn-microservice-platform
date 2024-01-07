using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicorn.eShop.Catalog.SDK.Events;

public record CatalogItemSoftDeleted
{
    public Guid Id { get; set; }
}
