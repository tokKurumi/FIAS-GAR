namespace GAR.Services.ReaderApi.Data;

using GAR.Services.ReaderApi.Core;
using GAR.Services.ReaderApi.Models;

public class XmlCopyHelpers
{
    public XmlReaderCopyHelper<AddressObject> Addresses =>
        new XmlReaderCopyHelper<AddressObject>(AddressObject.XmlElementName)
            .Map(AddressObject.XmlNames.Id, ao => ao.Id)
            .Map(AddressObject.XmlNames.ObjectId, ao => ao.ObjectId);
}
