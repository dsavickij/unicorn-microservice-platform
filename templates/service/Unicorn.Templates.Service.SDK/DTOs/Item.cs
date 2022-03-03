using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicorn.Templates.Service.SDK.DTOs;

public record Item
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
