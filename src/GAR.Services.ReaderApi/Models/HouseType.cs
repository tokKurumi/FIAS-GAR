namespace GAR.Services.ReaderApi.Models;

using GAR.Services.ReaderApi.Attributes;
using GAR.XmlReaderCopyHelper.Core;

[XmlElementName("HOUSETYPE")]
[ZipEntryName("HOUSETYPE")]
public class HouseType
{
    public const string XmlElementName = "HOUSETYPE";

    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public override bool Equals(object? obj)
    {
        return obj is HouseType type &&
               Id == type.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public static class XmlNames
    {
        public const string Id = "ID";

        public const string Name = "NAME";
    }
}
