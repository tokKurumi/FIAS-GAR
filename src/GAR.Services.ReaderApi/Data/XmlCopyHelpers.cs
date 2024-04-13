namespace GAR.Services.ReaderApi.Data;

using System.Runtime.CompilerServices;
using GAR.Services.ReaderApi.Entities;
using GAR.XmlReaderCopyHelper.Core;

public class XmlCopyHelpers
{
    public XmlReaderCopyHelper<AddressObject> Addresses =>
        new XmlReaderCopyHelper<AddressObject>(AddressObject.XmlElementName)
            .Map(AddressObject.XmlNames.Id, (ao, value) => ao.Id = int.Parse(value))
            .Map(AddressObject.XmlNames.ObjectId, (ao, value) => ao.ObjectId = int.Parse(value))
            .Map([AddressObject.XmlNames.TypeName, AddressObject.XmlNames.Name], (ao, values) => ao.FullName = $"{values[0]} {values[1]}")
            .WithCondition("ISACTUAL", GarBoolCondition)
            .WithCondition("ISACTIVE", GarBoolCondition);

    public XmlReaderCopyHelper<House> Houses =>
        new XmlReaderCopyHelper<House>(House.XmlElementName)
            .Map(House.XmlNames.Id, (h, value) => h.Id = int.Parse(value))
            .Map(House.XmlNames.ObjectId, (h, value) => h.ObjectId = int.Parse(value))
            .Map([House.XmlNames.HouseType, House.XmlNames.HouseNum], (h, values) => h.FullName = $"{values[0]} {values[1]}")
            .WithCondition("ISACTUAL", GarBoolCondition)
            .WithCondition("ISACTIVE", GarBoolCondition);

    public XmlReaderCopyHelper<Apartment> Apartments =>
        new XmlReaderCopyHelper<Apartment>(Apartment.XmlElementName)
            .Map(Apartment.XmlNames.Id, (a, value) => a.Id = int.Parse(value))
            .Map(Apartment.XmlNames.ObjectId, (a, value) => a.ObjectId = int.Parse(value))
            .Map([Apartment.XmlNames.ApartType, Apartment.XmlNames.Number], (a, values) => a.FullName = $"{values[0]} {values[1]}")
            .WithCondition("ISACTUAL", GarBoolCondition)
            .WithCondition("ISACTIVE", GarBoolCondition);

    public XmlReaderCopyHelper<Room> Rooms =>
        new XmlReaderCopyHelper<Room>(Room.XmlElementName)
            .Map(Room.XmlNames.Id, (r, value) => r.Id = int.Parse(value))
            .Map(Room.XmlNames.ObjectId, (r, value) => r.ObjectId = int.Parse(value))
            .Map([Room.XmlNames.RoomType, Room.XmlNames.Number], (r, values) => r.FullName = $"{values[0]} {values[1]}")
            .WithCondition("ISACTUAL", GarBoolCondition)
            .WithCondition("ISACTIVE", GarBoolCondition);

    public XmlReaderCopyHelper<Stead> Steads =>
        new XmlReaderCopyHelper<Stead>(Stead.XmlElementName)
            .Map(Stead.XmlNames.Id, (s, value) => s.Id = int.Parse(value))
            .Map(Stead.XmlNames.ObjectId, (s, value) => s.ObjectId = int.Parse(value))
            .Map(Stead.XmlNames.Number, (s, value) => s.FullName = value)
            .WithCondition("ISACTUAL", GarBoolCondition)
            .WithCondition("ISACTIVE", GarBoolCondition);

    public XmlReaderCopyHelper<Hierarchy> Hierarchies =>
        new XmlReaderCopyHelper<Hierarchy>(Hierarchy.XmlElementName)
            .Map(Hierarchy.XmlNames.Id, (h, value) => h.Id = int.Parse(value))
            .Map(Hierarchy.XmlNames.ObjectId, (h, value) => h.ObjectId = int.Parse(value))
            .Map(Hierarchy.XmlNames.Path, (h, value) => h.Path = value)
            .WithCondition("ISACTUAL", GarBoolCondition)
            .WithCondition("ISACTIVE", GarBoolCondition);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool GarBoolCondition(string attributeBoolValue)
    {
        return attributeBoolValue is "1" || (bool.TryParse(attributeBoolValue, out var parseResult) && parseResult);
    }
}
