namespace GAR.Services.ReaderApi.Models;

/// <summary>
/// Represents an address object.
/// </summary>
public class AddressObject
{
    /// <summary>
    /// The XML element name for the address object.
    /// </summary>
    public const string XmlElementName = "OBJECT";

    /// <summary>
    /// Gets or sets the ID of the address object.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the object ID of the address object.
    /// </summary>
    public int ObjectId { get; set; }

    /// <summary>
    /// Gets or sets the full name of the address object.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Represents the XML element names for the properties in the AddressObject class.
    /// </summary>
    public static class XmlNames
    {
        /// <summary>
        /// The XML element name for the ID property.
        /// </summary>
        public const string Id = "ID";

        /// <summary>
        /// The XML element name for the ObjectId property.
        /// </summary>
        public const string ObjectId = "OBJECTID";

        /// <summary>
        /// The XML element name for the TypeName property.
        /// </summary>
        public const string TypeName = "TYPENAME";

        /// <summary>
        /// The XML element name for the Name property.
        /// </summary>
        public const string Name = "NAME";
    }
}
