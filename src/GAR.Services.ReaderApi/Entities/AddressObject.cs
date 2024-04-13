namespace GAR.Services.ReaderApi.Models;

/// <summary>
/// Represents an address object.
/// </summary>
public class AddressObject
{
    public const string XmlElementName = "OBJECT";

    public AddressObject()
    {
    }

    public AddressObject(int id, int objectId, string fullName)
    {
        Id = id;
        ObjectId = objectId;
        FullName = fullName;
    }

    public int Id { get; set; }

    public int ObjectId { get; set; }

    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Represents the XML element names for the properties in the AddressObject class.
    /// </summary>
    public static class XmlNames
    {
        /// <summary>
        /// Gets the XML element name for the Id property.
        /// </summary>
        public const string Id = "ID";

        /// <summary>
        /// Gets the XML element name for the ObjectId property.
        /// </summary>
        public const string ObjectId = "OBJECTID";

        /// <summary>
        /// Gets the XML element name for the TypeName property.
        /// </summary>
        public const string TypeName = "TYPENAME";

        /// <summary>
        /// Gets the XML element name for the Name property.
        /// </summary>
        public const string Name = "NAME";
    }
}
