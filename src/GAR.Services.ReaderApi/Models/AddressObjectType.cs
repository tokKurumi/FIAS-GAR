namespace GAR.Services.ReaderApi.Models;

public record AddressObjectType(
    string ShortName,
    string Name)
{
    public const string XmlElementName = "ADDRESSOBJECTTYPE";

    public static class XmlNames
    {
        public const string ShortName = "SHORTNAME";

        public const string Name = "NAME";
    }
}