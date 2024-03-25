﻿namespace GAR.Services.ReaderApi.Models;

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
    /// Represents the number of the room.
    /// </summary>
    string Number,

    /// <summary>
    /// Represents the type of the room.
    /// </summary>
    int RoomType)
{
    /// <summary>
    /// Represents the XML element name for the room.
    /// </summary>
    public const string XmlElementName = "ROOM";

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