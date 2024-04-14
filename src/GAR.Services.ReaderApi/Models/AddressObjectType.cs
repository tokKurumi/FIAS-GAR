namespace GAR.Services.ReaderApi.Models;

using GAR.Services.ReaderApi.Attributes;
using GAR.XmlReaderCopyHelper.Core;

[XmlElementName("ADDRESSOBJECTTYPE")]
[ZipEntryName("ADDRESSOBJECTTYPE")]
public class AddressObjectType
{
    public const string XmlElementName = "ADDRESSOBJECTTYPE";

    public string ShortName { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public override bool Equals(object? obj)
    {
        return obj is AddressObjectType type &&
               ShortName == type.ShortName;
    }

    public override int GetHashCode()
    {
        return ShortName.GetHashCode();
    }

    public static class XmlNames
    {
        public const string ShortName = "SHORTNAME";

        public const string Name = "NAME";
    }
}