namespace GAR.Services.ReaderApi.Entities;

using GAR.Services.ReaderApi.Attributes;
using GAR.XmlReaderCopyHelper.Core;

/// <summary>
/// Represents a house.
/// </summary>
[XmlElementName("HOUSE")]
[ZipEntryName("HOUSE")]
public class House
{
    /// <summary>
    /// The XML element name for the house.
    /// </summary>
    public const string XmlElementName = "HOUSE";

    /// <summary>
    /// Gets or sets the ID of the house.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the object ID of the house.
    /// </summary>
    public int ObjectId { get; set; }

    /// <summary>
    /// Gets or sets the full name of the house.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Provides the XML names of the properties.
    /// </summary>
    public static class XmlNames
    {
        /// <summary>
        /// The XML name of the Id property.
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

        /// <summary>
        /// The XML name of the AddNum1 property.
        /// </summary>
        public const string AddNum1 = "ADDNUM1";

        /// <summary>
        /// The XML name of the AddType1 property.
        /// </summary>
        public const string AddType1 = "ADDTYPE1";

        /// <summary>
        /// The XML name of the AddNum2 property.
        /// </summary>
        public const string AddNum2 = "ADDNUM2";

        /// <summary>
        /// The XML name of the AddType2 property.
        /// </summary>
        public const string AddType2 = "ADDTYPE2";
    }
}
