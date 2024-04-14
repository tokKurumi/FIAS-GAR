namespace GAR.Services.ReaderApi.Entities;

using GAR.Services.ReaderApi.Attributes;
using GAR.XmlReaderCopyHelper.Core;

/// <summary>
/// Represents a stead.
/// </summary>
[XmlElementName("STEAD")]
[ZipEntryName("STEAD")]
public class Stead
{
    /// <summary>
    /// Represents the XML element name for the stead.
    /// </summary>
    public const string XmlElementName = "STEAD";

    /// <summary>
    /// Gets or sets the Id of the stead.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the ObjectId of the stead.
    /// </summary>
    public int ObjectId { get; set; }

    /// <summary>
    /// Gets or sets the full name of the stead.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

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
