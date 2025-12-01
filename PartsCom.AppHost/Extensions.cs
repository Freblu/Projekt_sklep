using Microsoft.Extensions.Configuration;

namespace PartsCom.AppHost;

internal static class Extensions
{
    internal static IResourceBuilder<T> WithEnvironmentFromConfiguration<T>(this IResourceBuilder<T> builder, string sectionName)
        where T : IResourceWithEnvironment
    {
        IConfigurationSection section = builder.ApplicationBuilder.Configuration.GetSection(sectionName);
        
        foreach (KeyValuePair<string, string?> entry in section.AsEnumerable(makePathsRelative: true))
        {
            if (string.IsNullOrWhiteSpace(entry.Key) || entry.Value is null)
            {
                continue;
            }

            string envVarName = entry.Key.Replace(":", "__");
            builder.WithEnvironment(envVarName, entry.Value);
        }

        return builder;
    }
}
