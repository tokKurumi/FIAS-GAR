namespace GAR.Services.ReaderApi.Models;

/// <summary>
/// Represents a room.
/// </summary>
public class Room
{
    /// <summary>
    /// Represents the XML element name for the room.
    /// </summary>
    public const string XmlElementName = "ROOM";

    /// <summary>
    /// Gets or sets the Id of the room.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the ObjectId of the room.
    /// </summary>
    public int ObjectId { get; set; }

    /// <summary>
    /// Gets or sets the full name of the room.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Contains XML element names for the room properties.
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
        /// Represents the XML element name for the RoomType property.
        /// </summary>
        public const string RoomType = "ROOMTYPE";
    }
}
