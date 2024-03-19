namespace GAR.Services.ReaderApi.Models;

/// <summary>
/// Represents a house.
/// </summary>
public record House(
    /// <summary>
    /// Represents the global unique identifier of the object.
    /// </summary>
    int ObjectId,

    /// <summary>
    /// The house number.
    /// </summary>
    string HouseNum,

    /// <summary>
    /// The type of the house.
    /// </summary>
    int HouseType)
{
    /// <summary>
    /// The XML element name for the house.
    /// </summary>
    public const string XmlElementName = "HOUSE";

    /// <summary>
    /// Provides the XML names of the properties.
    /// </summary>
    public static class XmlNames
    {
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
