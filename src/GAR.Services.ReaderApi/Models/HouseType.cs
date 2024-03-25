namespace GAR.Services.ReaderApi.Models;

public record HouseType(
    int Id,
    string Name)
{
    public const string XmlElementName = "HOUSETYPE";

    public static class XmlNames
    {
        public const string Id = "ID";

        public const string Name = "NAME";
    }
}
