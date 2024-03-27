namespace GAR.Services.ReaderApi.Models;

/// <summary>
/// Represents a house.
/// </summary>
public record House(

    /// <summary>
    /// Represents the identifier of the house.
    /// </summary>
    int Id,

    /// <summary>
    /// Represents the global unique identifier of the object.
    /// </summary>
    int ObjectId,

    /// <summary>
    /// Represents the full name of the house.
    /// </summary>
    string FullName)
{
    /// <summary>
    /// The XML element name for the house.
    /// </summary>
    public const string XmlElementName = "HOUSE";

    /// <summary>
    /// The name of the table entity for the house.
    /// </summary>
    public const string TableEntityName = "Houses";

    /// <summary>
    /// Provides the XML names of the properties.
    /// </summary>
    public static class XmlNames
    {
        /// <summary>
        /// Gets the XML element name for the Id property.
        /// </summary>
        public const string Id = "ID";

        /// <summary>
        /// The XML name of the ObjectId property.
        /// </summary>
        public const string ObjectId = "OBJECTID";

        /// <summary>
        /// The XML name of the HouseNum property.
        /// </summary>
        public const string HouseNum = "HOUSENUM";

        /// <summary>
        /// The XML name of the HouseType property.
        /// </summary>
        public const string HouseType = "HOUSETYPE";
    }
}
