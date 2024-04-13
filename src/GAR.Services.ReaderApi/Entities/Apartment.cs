namespace GAR.Services.ReaderApi.Models;

/// <summary>
/// Represents an apartment.
/// </summary>
public class Apartment
{
    /// <summary>
    /// Represents the XML element name for the apartment.
    /// </summary>
    public const string XmlElementName = "APARTMENT";

    /// <summary>
    /// Gets or sets the Id of the apartment.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the ObjectId of the apartment.
    /// </summary>
    public int ObjectId { get; set; }

    /// <summary>
    /// Gets or sets the full name of the apartment.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

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
