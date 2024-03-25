namespace GAR.Services.ReaderApi.Models;

public record RoomType(
    int Id,
    string Name)
{
    public const string XmlElementName = "ROOMTYPE";

    public static class XmlNames
    {
        public const string Id = "ID";

        public const string Name = "NAME";
    }
}
