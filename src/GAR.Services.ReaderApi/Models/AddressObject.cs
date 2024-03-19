﻿namespace GAR.Services.ReaderApi.Models;

/// <summary>
/// Represents an address object.
/// </summary>
public record AddressObject(
    /// <summary>
    /// Represents the global unique identifier of the object.
    /// </summary>
    int ObjectId,

    /// <summary>
    /// Represents the type name of the object.
    /// </summary>
    string TypeName,

    /// <summary>
    /// Represents the name of the object.
    /// </summary>
    string Name)
{
    /// <summary>
    /// Represents the XML element name for the object.
    /// </summary>
    public const string XmlElementName = "OBJECT";

    /// <summary>
    /// Represents the XML element names for the properties in the AddressObject class.
    /// </summary>
    public static class XmlNames
    {
        /// <summary>
        /// Gets the XML element name for the ObjectId property.
        /// </summary>
        public const string ObjectId = "OBJECTID";

        /// <summary>
        /// Gets the XML element name for the TypeName property.
        /// </summary>
        public const string TypeName = "TYPENAME";

        /// <summary>
        /// Gets the XML element name for the Name property.
        /// </summary>
        public const string Name = "NAME";
    }
}
