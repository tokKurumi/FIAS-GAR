namespace GAR.Services.ReaderApi.Data;

using GAR.Services.ReaderApi.Core;
using GAR.Services.ReaderApi.Models;

public class XmlCopyHelpers
{
    public XmlReaderCopyHelper<AddressObject> Addresses =>
        new XmlReaderCopyHelper<AddressObject>(AddressObject.XmlElementName)
            .Map(AddressObject.XmlNames.Id, (ao, value) => ao.Id = int.Parse(value))
            .Map(AddressObject.XmlNames.ObjectId, (ao, value) => ao.ObjectId = int.Parse(value))
            .Map([AddressObject.XmlNames.TypeName, AddressObject.XmlNames.Name], (ao, values) => ao.FullName = $"{values[0]} {values[1]}");
}
