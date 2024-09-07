﻿using Unicorn.Core.Development.ServiceHost.SDK.DTOs;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;

namespace Unicorn.Core.Development.ServiceHost.Features.GetFilmDescription;

public record DeleteFilmDescriptionRequest : BaseRequest.RequiringResult
{
    public Guid FilmId { get; set; }
}