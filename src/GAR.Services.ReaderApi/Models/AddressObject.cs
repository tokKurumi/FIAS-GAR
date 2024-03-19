namespace GAR.Services.ReaderApi.Models;

/// <summary>
/// Represents an address object.
/// </summary>
public record AddressObject(
    /// <summary>
    /// Represents the unique identifier of the object.
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
    /// Represents the names of the properties in the AddressObject class.
    /// </summary>
    public static class Names
    {
        /// <summary>
        /// Gets the name of the ObjectId property.
        /// </summary>
        public const string ObjectId = "OBJECTID";

        /// <summary>
        /// Gets the name of the TypeName property.
        /// </summary>
        public const string TypeName = "TYPENAME";

        /// <summary>
        /// Gets the name of the Name property.
        /// </summary>
        public const string Name = "NAME";
    }
}
