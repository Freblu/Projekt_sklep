using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace PartsCom.Domain.Entities;

public sealed class NewsPost
{
    private NewsPost() { }

    public Guid Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty;
    public string ShortDescription { get; private set; } = string.Empty;
    public string Content { get; private set; } = string.Empty; // Full content placeholder
    public string ImageUrl { get; private set; } = string.Empty;
    public string Author { get; private set; } = string.Empty;
    public DateTime PublishedAt { get; private set; }

    [SuppressMessage("Design", "CA1054:URI-like parameters should not be strings", Justification = "URL stored as string in database")]
    public static NewsPost Create(string title, string shortDescription, string imageUrl, string author)
    {
        return new NewsPost
        {
            Id = Guid.NewGuid(),
            Title = title,
            ShortDescription = shortDescription,
            ImageUrl = imageUrl,
            Author = author,
            PublishedAt = DateTime.UtcNow,
            Content = shortDescription, // Defaulting content for now
            Slug = title.ToLower(CultureInfo.InvariantCulture).Replace(" ", "-")
        };
    }
}