namespace GAR.Services.ReaderApi.Services;

using System.Collections.Generic;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Xml;
using GAR.Services.ReaderApi.Models;

/// <summary>
/// Provides a service for reading objects from a ZIP file containing XML data.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ZipXmlReaderService"/> class.
/// </remarks>
/// <param name="folderRegion">The folder region.</param>
/// <param name="bucketSize">The bucket size.</param>
public class ZipXmlReaderService(
    string folderRegion,
    int bucketSize)
{
    private readonly string _folderRegion = folderRegion;
    private readonly int _bucketSize = bucketSize;
    private readonly ZipArchive _zipArchive = ZipFile.OpenRead(@$"Data/gar_xml.zip");

    private XmlReader? _addressXmlReader;
    private XmlReader? _housesXmlReader;

    /// <summary>
    /// Gets a value indicating whether this instance can read objects.
    /// </summary>
    public bool CanReadObjects { get; private set; } = true;

    public bool CanReadHouses { get; private set; } = true;

    /// <summary>
    /// Starts the reader.
    /// </summary>
    public void StartReader()
    {
        StartObjectsReader();
        StartHousesReader();
    }

    /// <summary>
    /// Reads the objects asynchronously.
    /// </summary>
    /// <returns>An asynchronous enumerable of <see cref="AddressObject"/>.</returns>
    public async IAsyncEnumerable<AddressObject> ReadObjectsAsync()
    {
        if (_addressXmlReader is null)
        {
            throw new ArgumentNullException(nameof(_addressXmlReader));
        }

        for (int i = 0; i < _bucketSize; i++)
        {
            CanReadObjects = await _addressXmlReader.ReadAsync();

            if (_addressXmlReader is { NodeType: XmlNodeType.Element, Name: AddressObject.XmlElementName })
            {
                var isActualAttribute = _addressXmlReader.GetAttribute("ISACTUAL");
                var isActiveAttribute = _addressXmlReader.GetAttribute("ISACTIVE");

                var isActual = string.IsNullOrEmpty(isActualAttribute) || isActualAttribute == "1";
                var isActive = string.IsNullOrEmpty(isActiveAttribute) || isActiveAttribute == "1";

                if (isActual && isActive)
                {
                    var objectId = int.Parse(_addressXmlReader.GetAttribute(AddressObject.XmlNames.ObjectId) ?? throw new ArgumentNullException(AddressObject.XmlNames.ObjectId));
                    var typeName = _addressXmlReader.GetAttribute(AddressObject.XmlNames.TypeName) ?? throw new ArgumentNullException(AddressObject.XmlNames.TypeName);
                    var name = _addressXmlReader.GetAttribute(AddressObject.XmlNames.Name) ?? throw new ArgumentNullException(AddressObject.XmlNames.Name);

                    yield return new AddressObject(objectId, typeName, name);
                }
            }
        }
    }

    public async IAsyncEnumerable<House> ReadHousesAsync()
    {
        if (_housesXmlReader is null)
        {
            throw new ArgumentNullException(nameof(_housesXmlReader));
        }

        for (int i = 0; i < _bucketSize; i++)
        {
            CanReadHouses = await _housesXmlReader.ReadAsync();

            if (_housesXmlReader is { NodeType: XmlNodeType.Element, Name: House.XmlElementName })
            {
                var isActualAttribute = _housesXmlReader.GetAttribute("ISACTUAL");
                var isActiveAttribute = _housesXmlReader.GetAttribute("ISACTIVE");

                var isActual = string.IsNullOrEmpty(isActualAttribute) || isActualAttribute == "1";
                var isActive = string.IsNullOrEmpty(isActiveAttribute) || isActiveAttribute == "1";

                if (isActual && isActive)
                {
                    var objectId = int.Parse(_housesXmlReader.GetAttribute(House.XmlNames.ObjectId) ?? throw new ArgumentNullException(House.XmlNames.ObjectId));
                    var houseNum = _housesXmlReader.GetAttribute(House.XmlNames.HouseNum) ?? throw new ArgumentNullException(House.XmlNames.HouseNum);
                    var houseType = int.Parse(_housesXmlReader.GetAttribute(House.XmlNames.HouseType) ?? throw new ArgumentNullException(House.XmlNames.HouseType));

                    yield return new House(objectId, houseNum, houseType);
                }
            }
        }
    }

    private void StartObjectsReader()
    {
        var addressEntry = GetAddressEntry();
        var addressStream = addressEntry.Open();

        _addressXmlReader = XmlReader.Create(addressStream, new() { Async = true });
    }

    private ZipArchiveEntry GetAddressEntry()
    {
        var regexp = new Regex($@"^{_folderRegion}/AS_ADDR_OBJ_(\d{{8}})_.+\.XML$");

        return _zipArchive.Entries.First(file => regexp.IsMatch(file.FullName));
    }

    private void StartHousesReader()
    {
        var housesEntry = GetHousesEntry();
        var housesStream = housesEntry.Open();

        _housesXmlReader = XmlReader.Create(housesStream, new() { Async = true });
    }

    private ZipArchiveEntry GetHousesEntry()
    {
        var regexp = new Regex($@"^{_folderRegion}/AS_HOUSES_(\d{{8}})_.+\.XML$");

        return _zipArchive.Entries.First(file => regexp.IsMatch(file.FullName));
    }
}
