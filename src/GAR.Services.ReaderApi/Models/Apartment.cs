namespace GAR.Services.ReaderApi.Models;

/// <summary>
/// Represents an apartment.
/// </summary>
public record Apartment(
    /// <summary>
    /// Represents the global unique identifier of the object.
    /// </summary>
    int ObjectId,

    /// <summary>
    /// Represents the number of the apartment.
    /// </summary>
    int Number,

    /// <summary>
    /// Represents the type of the apartment.
    /// </summary>
    int ApartType)
{
    /// <summary>
    /// Represents the XML element name for the apartment.
    /// </summary>
    public const string XmlElementName = "APARTMENT";

    /// <summary>
    /// Contains XML element names for the apartment properties.
    /// </summary>
    public static class XmlNames
    {
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
