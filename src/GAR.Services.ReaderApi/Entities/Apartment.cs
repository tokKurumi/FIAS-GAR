namespace GAR.Services.ReaderApi.Models;

/// <summary>
/// Represents an apartment.
/// </summary>
public record Apartment(

    /// <summary>
    /// Represents the identifier of the apartment.
    /// </summary>
    int Id,

    /// <summary>
    /// Represents the global unique identifier of the object.
    /// </summary>
    int ObjectId,

    /// <summary>
    /// Represents the full name of the apartment.
    /// </summary>
    string FullName)
{
    /// <summary>
    /// Represents the XML element name for the apartment.
    /// </summary>
    public const string XmlElementName = "APARTMENT";

    /// <summary>
    /// Represents the table entity name for the apartment.
    /// </summary>
    public const string TableEntityName = "Apartments";

    /// <summary>
    /// Contains XML element names for the apartment properties.
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

        /// <summary>
        /// Represents the XML element name for the ApartType property.
        /// </summary>
        public const string ApartType = "APARTTYPE";
    }
}
