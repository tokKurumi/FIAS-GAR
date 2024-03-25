namespace GAR.Services.ReaderApi.Models;

/// <summary>
/// Represents a hierarchy item.
/// </summary>
public record Hierarchy(

    /// <summary>
    /// Represents the identifier of the hierarchy.
    /// </summary>
    int Id,

    /// <summary>
    /// Represents the global unique identifier of the object.
    /// </summary>
    int ObjectId,

    /// <summary>
    /// Represents the path of the object.
    /// </summary>
    string Path)
{
    /// <summary>
    /// Represents the XML element name for the hierarchy item.
    /// </summary>
    public const string XmlElementName = "ITEM";

    /// <summary>
    /// Represents the XML attribute name for the object ID.
    /// </summary>
    public static class XmlNames
    {
        /// <summary>
        /// Gets the XML element name for the Id property.
        /// </summary>
        public const string Id = "ID";

        /// <summary>
        /// Represents the XML attribute name for the object ID.
        /// </summary>
        public const string ObjectId = "OBJECTID";

        /// <summary>
        /// Represents the XML attribute name for the object path.
        /// </summary>
        public const string Path = "PATH";
    }
}
