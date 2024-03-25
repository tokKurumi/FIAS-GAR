namespace GAR.Services.ReaderApi.Models;

public record ApartmentType(
    int Id,
    string Name)
{
    public const string XmlElementName = "APARTMENTTYPE";

    public static class XmlNames
    {
        public const string Id = "ID";

        public const string Name = "NAME";
    }
}
