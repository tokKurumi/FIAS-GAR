namespace GAR.Services.ReaderApi.Models;

/// <summary>
/// Represents a stead.
/// </summary>
public record Stead(

    /// <summary>
    /// Represents the identifier of the stead.
    /// </summary>
    int Id,

    /// <summary>
    /// Represents the global unique identifier of the object.
    /// </summary>
    int ObjectId,

    /// <summary>
    /// Represents the number of the stead.
    /// </summary>
    string Number)
{
    /// <summary>
    /// Represents the XML element name for the stead.
    /// </summary>
    public const string XmlElementName = "STEAD";

    /// <summary>
    /// Represents the table entity name for the stead.
    /// </summary>
    public const string TableEntityName = "Steads";

    /// <summary>
    /// Contains XML element names for the stead properties.
    /// </summary>
    public static class XmlNames
    {
        /// <summary>
        /// Gets the XML element name for the Id property.
        /// </summary>
        public const string Id = "ID";

        /// <summary>
        /// Represents the XML element name for the ObjectId property.
        /// </summary>
        public const string ObjectId = "OBJECTID";

        /// <summary>
        /// Represents the XML element name for the Number property.
        /// </summary>
        public const string Number = "NUMBER";
    }
}
