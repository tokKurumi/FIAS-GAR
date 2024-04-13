namespace GAR.Services.ReaderApi.Entities;

using GAR.XmlReaderCopyHelper.Core;

/// <summary>
/// Represents a hierarchy item.
/// </summary>
[XmlElementName("ITEM")]
public class Hierarchy
{
    /// <summary>
    /// Represents the XML element name for the hierarchy item.
    /// </summary>
    public const string XmlElementName = "ITEM";

    /// <summary>
    /// Gets or sets the ID of the hierarchy item.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the object ID of the hierarchy item.
    /// </summary>
    public int ObjectId { get; set; }

    /// <summary>
    /// Gets or sets the path of the hierarchy item.
    /// </summary>
    public string Path { get; set; } = string.Empty;

    /// <summary>
    /// Represents the XML attribute names for the hierarchy item.
    /// </summary>
    public static class XmlNames
    {
        /// <summary>
        /// Represents the XML attribute name for the ID property.
        /// </summary>
        public const string Id = "ID";

        /// <summary>
        /// Represents the XML attribute name for the object ID property.
        /// </summary>
        public const string ObjectId = "OBJECTID";

        /// <summary>
        /// Represents the XML attribute name for the path property.
        /// </summary>
        public const string Path = "PATH";
    }
}
