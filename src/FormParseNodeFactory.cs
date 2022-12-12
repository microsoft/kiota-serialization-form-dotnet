using Microsoft.Kiota.Abstractions.Serialization;

namespace Microsoft.Kiota.Serialization.Form;

/// <summary>
/// The <see cref="IParseNodeFactory"/> implementation for form content types
/// </summary>
public class FormParseNodeFactory : IParseNodeFactory
{
    /// <inheritdoc/>
    public string ValidContentType => "application/x-www-form-urlencoded";
    /// <inheritdoc/>
    public IParseNode GetRootParseNode(string contentType, Stream content) {
        if(string.IsNullOrEmpty(contentType))
            throw new ArgumentNullException(nameof(contentType));
        if(!ValidContentType.Equals(contentType, StringComparison.OrdinalIgnoreCase))
            throw new ArgumentOutOfRangeException($"expected a {ValidContentType} content type");
        if( content == null)
            throw new ArgumentNullException(nameof(content));

        using var reader = new StreamReader(content);
        var rawValue = reader.ReadToEnd();
        return new FormParseNode(rawValue);
    }
}
