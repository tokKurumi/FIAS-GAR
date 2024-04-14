namespace GAR.Services.ReaderApi.Data;

using System.Runtime.CompilerServices;
using GAR.Services.ReaderApi.Entities;
using GAR.XmlReaderCopyHelper.Core;

public class XmlDataCopyHelpers
{
    public XmlReaderCopyHelper<AddressObject> Addresses { get; } =
        new XmlReaderCopyHelper<AddressObject>()
            .Map(AddressObject.XmlNames.Id, (ao, value) => ao.Id = int.Parse(value))
            .Map(AddressObject.XmlNames.ObjectId, (ao, value) => ao.ObjectId = int.Parse(value))
            .WithCondition("ISACTUAL", GarBoolCondition)
            .WithCondition("ISACTIVE", GarBoolCondition);

    public XmlReaderCopyHelper<House> Houses { get; } =
        new XmlReaderCopyHelper<House>()
            .Map(House.XmlNames.Id, (h, value) => h.Id = int.Parse(value))
            .Map(House.XmlNames.ObjectId, (h, value) => h.ObjectId = int.Parse(value))
            .WithCondition("ISACTUAL", GarBoolCondition)
            .WithCondition("ISACTIVE", GarBoolCondition);

    public XmlReaderCopyHelper<Apartment> Apartments { get; } =
        new XmlReaderCopyHelper<Apartment>()
            .Map(Apartment.XmlNames.Id, (a, value) => a.Id = int.Parse(value))
            .Map(Apartment.XmlNames.ObjectId, (a, value) => a.ObjectId = int.Parse(value))
            .WithCondition("ISACTUAL", GarBoolCondition)
            .WithCondition("ISACTIVE", GarBoolCondition);

    public XmlReaderCopyHelper<Room> Rooms { get; } =
        new XmlReaderCopyHelper<Room>()
            .Map(Room.XmlNames.Id, (r, value) => r.Id = int.Parse(value))
            .Map(Room.XmlNames.ObjectId, (r, value) => r.ObjectId = int.Parse(value))
            .WithCondition("ISACTUAL", GarBoolCondition)
            .WithCondition("ISACTIVE", GarBoolCondition);

    public XmlReaderCopyHelper<Stead> Steads { get; } =
        new XmlReaderCopyHelper<Stead>()
            .Map(Stead.XmlNames.Id, (s, value) => s.Id = int.Parse(value))
            .Map(Stead.XmlNames.ObjectId, (s, value) => s.ObjectId = int.Parse(value))
            .Map(Stead.XmlNames.Number, (s, value) => s.FullName = value)
            .WithCondition("ISACTUAL", GarBoolCondition)
            .WithCondition("ISACTIVE", GarBoolCondition);

    public XmlReaderCopyHelper<Hierarchy> Hierarchies { get; } =
        new XmlReaderCopyHelper<Hierarchy>()
            .Map(Hierarchy.XmlNames.Id, (h, value) => h.Id = int.Parse(value))
            .Map(Hierarchy.XmlNames.ObjectId, (h, value) => h.ObjectId = int.Parse(value))
            .Map(Hierarchy.XmlNames.Path, (h, value) => h.Path = value)
            .WithCondition("ISACTUAL", GarBoolCondition)
            .WithCondition("ISACTIVE", GarBoolCondition);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool GarBoolCondition(string? attributeBoolValue)
    {
        return attributeBoolValue is null or "1" || (bool.TryParse(attributeBoolValue, out var parseResult) && parseResult);
    }
}
