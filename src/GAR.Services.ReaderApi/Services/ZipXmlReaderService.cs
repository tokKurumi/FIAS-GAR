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
    : IDisposable
{
    private readonly string _folderRegion = folderRegion;
    private readonly int _bucketSize = bucketSize;

    private bool _disposed;

    private ZipArchive? _zipArchive;
    private XmlReader? _addressesXmlReader;
    private XmlReader? _housesXmlReader;
    private XmlReader? _apartmentsXmlReader;
    private XmlReader? _roomsXmlReader;
    private XmlReader? _steadsXmlReader;

    /// <summary>
    /// Gets a value indicating whether this instance can read objects.
    /// </summary>
    public bool CanReadObjects { get; private set; } = true;

    /// <summary>
    /// Gets a value indicating whether this instance can read houses.
    /// </summary>
    public bool CanReadHouses { get; private set; } = true;

    /// <summary>
    /// Gets a value indicating whether this instance can read apartments.
    /// </summary>
    public bool CanReadApartments { get; private set; } = true;

    /// <summary>
    /// Gets a value indicating whether this instance can read rooms.
    /// </summary>
    public bool CanReadRooms { get; private set; } = true;

    /// <summary>
    /// Gets a value indicating whether this instance can read steads.
    /// </summary>
    public bool CanReadSteads { get; private set; } = true;

    /// <summary>
    /// Starts the reader.
    /// </summary>
    public void StartReader()
    {
        _zipArchive = ZipFile.OpenRead(@$"Data/gar_xml.zip");

        StartObjectsReader();
        StartHousesReader();
        StartApartmentsReader();
        StartRoomsReader();
        StartSteadsReader();
    }

    /// <summary>
    /// Reads the objects asynchronously.
    /// </summary>
    /// <returns>An asynchronous enumerable of <see cref="AddressObject"/>.</returns>
    public async IAsyncEnumerable<AddressObject> ReadObjectsAsync()
    {
        if (_addressesXmlReader is null)
        {
            throw new ArgumentNullException(nameof(_addressesXmlReader));
        }

        for (int i = 0; i < _bucketSize; i++)
        {
            CanReadObjects = await _addressesXmlReader.ReadAsync();

            if (_addressesXmlReader is { NodeType: XmlNodeType.Element, Name: AddressObject.XmlElementName })
            {
                var isActualAttribute = _addressesXmlReader.GetAttribute("ISACTUAL");
                var isActiveAttribute = _addressesXmlReader.GetAttribute("ISACTIVE");

                var isActual = string.IsNullOrEmpty(isActualAttribute) || isActualAttribute == "1";
                var isActive = string.IsNullOrEmpty(isActiveAttribute) || isActiveAttribute == "1";

                if (isActual && isActive)
                {
                    var objectId = int.Parse(_addressesXmlReader.GetAttribute(AddressObject.XmlNames.ObjectId) ?? throw new ArgumentNullException(AddressObject.XmlNames.ObjectId));
                    var typeName = _addressesXmlReader.GetAttribute(AddressObject.XmlNames.TypeName) ?? throw new ArgumentNullException(AddressObject.XmlNames.TypeName);
                    var name = _addressesXmlReader.GetAttribute(AddressObject.XmlNames.Name) ?? throw new ArgumentNullException(AddressObject.XmlNames.Name);

                    yield return new AddressObject(objectId, typeName, name);
                }
            }
        }
    }

    /// <summary>
    /// Reads the houses asynchronously.
    /// </summary>
    /// <returns>An asynchronous enumerable of <see cref="House"/>.</returns>
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

    /// <summary>
    /// Reads the apartments asynchronously.
    /// </summary>
    /// <returns>An asynchronous enumerable of <see cref="Apartment"/>.</returns>
    public async IAsyncEnumerable<Apartment> ReadApartmentsAsync()
    {
        if (_apartmentsXmlReader is null)
        {
            throw new ArgumentNullException(nameof(_apartmentsXmlReader));
        }

        for (int i = 0; i < _bucketSize; i++)
        {
            CanReadApartments = await _apartmentsXmlReader.ReadAsync();

            if (_apartmentsXmlReader is { NodeType: XmlNodeType.Element, Name: Apartment.XmlElementName })
            {
                var isActualAttribute = _apartmentsXmlReader.GetAttribute("ISACTUAL");
                var isActiveAttribute = _apartmentsXmlReader.GetAttribute("ISACTIVE");

                var isActual = string.IsNullOrEmpty(isActualAttribute) || isActualAttribute == "1";
                var isActive = string.IsNullOrEmpty(isActiveAttribute) || isActiveAttribute == "1";

                if (isActual && isActive)
                {
                    var objectId = int.Parse(_apartmentsXmlReader.GetAttribute(Apartment.XmlNames.ObjectId) ?? throw new ArgumentNullException(Apartment.XmlNames.ObjectId));
                    var number = _apartmentsXmlReader.GetAttribute(Apartment.XmlNames.Number) ?? throw new ArgumentNullException(Apartment.XmlNames.Number);
                    var apartType = int.Parse(_apartmentsXmlReader.GetAttribute(Apartment.XmlNames.ApartType) ?? throw new ArgumentNullException(Apartment.XmlNames.ApartType));

                    yield return new Apartment(objectId, number, apartType);
                }
            }
        }
    }

    /// <summary>
    /// Reads the rooms asynchronously.
    /// </summary>
    /// <returns>An asynchronous enumerable of <see cref="Room"/>.</returns>
    public async IAsyncEnumerable<Room> ReadRoomsAsync()
    {
        if (_roomsXmlReader is null)
        {
            throw new ArgumentNullException(nameof(_roomsXmlReader));
        }

        for (int i = 0; i < _bucketSize; i++)
        {
            CanReadRooms = await _roomsXmlReader.ReadAsync();

            if (_roomsXmlReader is { NodeType: XmlNodeType.Element, Name: Room.XmlElementName })
            {
                var isActualAttribute = _roomsXmlReader.GetAttribute("ISACTUAL");
                var isActiveAttribute = _roomsXmlReader.GetAttribute("ISACTIVE");

                var isActual = string.IsNullOrEmpty(isActualAttribute) || isActualAttribute == "1";
                var isActive = string.IsNullOrEmpty(isActiveAttribute) || isActiveAttribute == "1";

                if (isActual && isActive)
                {
                    var objectId = int.Parse(_roomsXmlReader.GetAttribute(Room.XmlNames.ObjectId) ?? throw new ArgumentNullException(Room.XmlNames.ObjectId));
                    var number = _roomsXmlReader.GetAttribute(Room.XmlNames.Number) ?? throw new ArgumentNullException(Room.XmlNames.Number);
                    var roomType = int.Parse(_roomsXmlReader.GetAttribute(Room.XmlNames.RoomType) ?? throw new ArgumentNullException(Room.XmlNames.RoomType));

                    yield return new Room(objectId, number, roomType);
                }
            }
        }
    }

    /// <summary>
    /// Reads the steads asynchronously.
    /// </summary>
    /// <returns>An asynchronous enumerable of <see cref="Stead"/>.</returns>
    public async IAsyncEnumerable<Stead> ReadSteadsAsync()
    {
        if (_steadsXmlReader is null)
        {
            throw new ArgumentNullException(nameof(_steadsXmlReader));
        }

        for (int i = 0; i < _bucketSize; i++)
        {
            CanReadSteads = await _steadsXmlReader.ReadAsync();

            if (_steadsXmlReader is { NodeType: XmlNodeType.Element, Name: Stead.XmlElementName })
            {
                var isActualAttribute = _steadsXmlReader.GetAttribute("ISACTUAL");
                var isActiveAttribute = _steadsXmlReader.GetAttribute("ISACTIVE");

                var isActual = string.IsNullOrEmpty(isActualAttribute) || isActualAttribute == "1";
                var isActive = string.IsNullOrEmpty(isActiveAttribute) || isActiveAttribute == "1";

                if (isActual && isActive)
                {
                    var objectId = int.Parse(_steadsXmlReader.GetAttribute(Stead.XmlNames.ObjectId) ?? throw new ArgumentNullException(Stead.XmlNames.ObjectId));
                    var number = _steadsXmlReader.GetAttribute(Stead.XmlNames.Number) ?? throw new ArgumentNullException(Stead.XmlNames.Number);

                    yield return new Stead(objectId, number);
                }
            }
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            _zipArchive?.Dispose();
            _addressesXmlReader?.Dispose();
            _housesXmlReader?.Dispose();
            _apartmentsXmlReader?.Dispose();
            _roomsXmlReader?.Dispose();
            _steadsXmlReader?.Dispose();
        }

        _disposed = true;
    }

    private void StartObjectsReader()
    {
        var addressEntry = GetAddressEntry();
        var addressStream = addressEntry.Open();

        _addressesXmlReader = XmlReader.Create(addressStream, new() { Async = true });
    }

    private ZipArchiveEntry GetAddressEntry()
    {
        var regexp = new Regex($@"^{_folderRegion}/AS_ADDR_OBJ_(\d{{8}})_.+\.XML$");

        return _zipArchive!.Entries.First(file => regexp.IsMatch(file.FullName));
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

        return _zipArchive!.Entries.First(file => regexp.IsMatch(file.FullName));
    }

    private void StartApartmentsReader()
    {
        var apartmentsEntry = GetApartmentsEntry();
        var apartmentsStream = apartmentsEntry.Open();

        _apartmentsXmlReader = XmlReader.Create(apartmentsStream, new() { Async = true });
    }

    private ZipArchiveEntry GetApartmentsEntry()
    {
        var regexp = new Regex($@"^{_folderRegion}/AS_APARTMENTS_(\d{{8}})_.+\.XML$");

        return _zipArchive!.Entries.First(file => regexp.IsMatch(file.FullName));
    }

    private void StartRoomsReader()
    {
        var roomsEntry = GetRoomsEntry();
        var roomsStream = roomsEntry.Open();

        _roomsXmlReader = XmlReader.Create(roomsStream, new() { Async = true });
    }

    private ZipArchiveEntry GetRoomsEntry()
    {
        var regexp = new Regex($@"^{_folderRegion}/AS_ROOMS_(\d{{8}})_.+\.XML$");

        return _zipArchive!.Entries.First(file => regexp.IsMatch(file.FullName));
    }

    private void StartSteadsReader()
    {
        var steadsEntry = GetSteadsEntry();
        var steadsStream = steadsEntry.Open();

        _steadsXmlReader = XmlReader.Create(steadsStream, new() { Async = true });
    }

    private ZipArchiveEntry GetSteadsEntry()
    {
        var regexp = new Regex($@"^{_folderRegion}/AS_STEADS_(\d{{8}})_.+\.XML$");

        return _zipArchive!.Entries.First(file => regexp.IsMatch(file.FullName));
    }
}
