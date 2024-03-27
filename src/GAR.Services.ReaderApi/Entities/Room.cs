namespace GAR.Services.ReaderApi.Models;

/// <summary>
/// Represents a room.
/// </summary>
public record Room(

    /// <summary>
    /// Represents the identifier of the room.
    /// </summary>
    int Id,

    /// <summary>
    /// Represents the global unique identifier of the object.
    /// </summary>
    int ObjectId,

    /// <summary>
    /// Represents the full name of the room.
    /// </summary>
    string FullName)
{
    /// <summary>
    /// Represents the XML element name for the room.
    /// </summary>
    public const string XmlElementName = "ROOM";

    /// <summary>
    /// Represents the name of the table entity for the room.
    /// </summary>
    public const string TableEntityName = "Rooms";

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
