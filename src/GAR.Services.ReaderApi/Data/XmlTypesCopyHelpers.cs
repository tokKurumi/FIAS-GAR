namespace GAR.Services.ReaderApi.Data;

using GAR.Services.ReaderApi.Models;
using GAR.XmlReaderCopyHelper.Core;

public class XmlTypesCopyHelpers
{
    public XmlReaderCopyHelper<AddressObjectType> AddressTypes =>
        new XmlReaderCopyHelper<AddressObjectType>()
            .Map(AddressObjectType.XmlNames.Name, (aot, value) => aot.Name = value)
            .Map(AddressObjectType.XmlNames.ShortName, (aot, value) => aot.ShortName = value);

    public XmlReaderCopyHelper<ApartmentType> ApartmentTypes =>
        new XmlReaderCopyHelper<ApartmentType>()
            .Map(ApartmentType.XmlNames.Id, (at, value) => at.Id = int.Parse(value))
            .Map(ApartmentType.XmlNames.Name, (at, value) => at.Name = value);

    public XmlReaderCopyHelper<HouseType> HouseTypes =>
        new XmlReaderCopyHelper<HouseType>()
            .Map(HouseType.XmlNames.Id, (ht, value) => ht.Id = int.Parse(value))
            .Map(HouseType.XmlNames.Name, (ht, value) => ht.Name = value);

    public XmlReaderCopyHelper<RoomType> RoomTypes =>
        new XmlReaderCopyHelper<RoomType>()
            .Map(RoomType.XmlNames.Id, (rt, value) => rt.Id = int.Parse(value))
            .Map(RoomType.XmlNames.Name, (rt, value) => rt.Name = value);
}
