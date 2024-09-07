namespace Unicorn.Core.Development.ServiceHost.SDK.DTOs;

public record FilmDescription
{
    public Guid DescriptionId { get; set; }
    public Guid FilmId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime ReleaseDate { get; set; }
    public DateTime LastUpdatedOn { get; set; }
}
