namespace GAR.Services.ReaderApi.Services;

using System.Collections.Generic;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Xml;
using GAR.Services.ReaderApi.Models;

/// <summary>
/// Provides a service for reading objects from a ZIP file containing XML data.
/// </summary>
public class ZipXmlReaderService : IDisposable
{
    private readonly string _folderPath;
    private readonly int _bucketSize;

    private readonly ZipArchive _zipArchive;
    private readonly Dictionary<string, Regex> _regexes;
    private readonly Dictionary<string, XmlReader> _readers;

    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="ZipXmlReaderService"/> class.
    /// </summary>
    /// <param name="folderPath">The folder region path.</param>
    /// <param name="bucketSize">The bucket size.</param>
    public ZipXmlReaderService(
        string folderPath,
        int bucketSize)
    {
        _folderPath = folderPath;
        _bucketSize = bucketSize;

        _zipArchive = ZipFile.OpenRead(@$"Data/gar_xml.zip");

        _regexes = new Dictionary<string, Regex>
        {
            [AddressObject.XmlElementName] = new Regex($@"^{folderPath}/AS_ADDR_OBJ_(\d{{8}})_.+\.XML$"),
            [House.XmlElementName] = new Regex($@"^{_folderPath}/AS_HOUSES_(\d{{8}})_.+\.XML$"),
            [Apartment.XmlElementName] = new Regex($@"^{_folderPath}/AS_APARTMENTS_(\d{{8}})_.+\.XML$"),
            [Room.XmlElementName] = new Regex($@"^{_folderPath}/AS_ROOMS_(\d{{8}})_.+\.XML$"),
            [Stead.XmlElementName] = new Regex($@"^{_folderPath}/AS_STEADS_(\d{{8}})_.+\.XML$"),
            [Hierarchy.XmlElementName] = new Regex($@"^{_folderPath}/AS_ADM_HIERARCHY_(\d{{8}})_.+\.XML$"),
        };

        _readers = new Dictionary<string, XmlReader>
        {
            [AddressObject.XmlElementName] = CreateAsyncXmlReader(AddressObject.XmlElementName),
            [House.XmlElementName] = CreateAsyncXmlReader(House.XmlElementName),
            [Apartment.XmlElementName] = CreateAsyncXmlReader(Apartment.XmlElementName),
            [Room.XmlElementName] = CreateAsyncXmlReader(Room.XmlElementName),
            [Stead.XmlElementName] = CreateAsyncXmlReader(Stead.XmlElementName),
            [Hierarchy.XmlElementName] = CreateAsyncXmlReader(Hierarchy.XmlElementName),
        };
    }

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
    /// Gets a value indicating whether this instance can read hierarchies.
    /// </summary>
    public bool CanReadHierarchies { get; private set; } = true;

    /// <summary>
    /// Reads the objects asynchronously.
    /// </summary>
    /// <returns>An asynchronous enumerable of <see cref="AddressObject"/>.</returns>
    public async IAsyncEnumerable<AddressObject> ReadObjectsAsync()
    {
        var addressesXmlReader = _readers[AddressObject.XmlElementName];

        for (int i = 0; i < _bucketSize; i++)
        {
            CanReadObjects = await addressesXmlReader.ReadAsync();

            if (addressesXmlReader is { NodeType: XmlNodeType.Element, Name: AddressObject.XmlElementName })
            {
                var isActualAttribute = addressesXmlReader.GetAttribute("ISACTUAL");
                var isActiveAttribute = addressesXmlReader.GetAttribute("ISACTIVE");

                var isActual = string.IsNullOrEmpty(isActualAttribute) || isActualAttribute == "1";
                var isActive = string.IsNullOrEmpty(isActiveAttribute) || isActiveAttribute == "1";

                if (isActual && isActive)
                {
                    var id = int.Parse(addressesXmlReader.GetAttribute(AddressObject.XmlNames.Id) ?? throw new ArgumentNullException(AddressObject.XmlNames.Id));
                    var objectId = int.Parse(addressesXmlReader.GetAttribute(AddressObject.XmlNames.ObjectId) ?? throw new ArgumentNullException(AddressObject.XmlNames.ObjectId));
                    var typeName = addressesXmlReader.GetAttribute(AddressObject.XmlNames.TypeName) ?? throw new ArgumentNullException(AddressObject.XmlNames.TypeName);
                    var name = addressesXmlReader.GetAttribute(AddressObject.XmlNames.Name) ?? throw new ArgumentNullException(AddressObject.XmlNames.Name);

                    yield return new AddressObject(id, objectId, typeName, name);
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
        var housesXmlReader = _readers[House.XmlElementName];

        for (int i = 0; i < _bucketSize; i++)
        {
            CanReadHouses = await housesXmlReader.ReadAsync();

            if (housesXmlReader is { NodeType: XmlNodeType.Element, Name: House.XmlElementName })
            {
                var isActualAttribute = housesXmlReader.GetAttribute("ISACTUAL");
                var isActiveAttribute = housesXmlReader.GetAttribute("ISACTIVE");

                var isActual = string.IsNullOrEmpty(isActualAttribute) || isActualAttribute == "1";
                var isActive = string.IsNullOrEmpty(isActiveAttribute) || isActiveAttribute == "1";

                if (isActual && isActive)
                {
                    var id = int.Parse(housesXmlReader.GetAttribute(House.XmlNames.Id) ?? throw new ArgumentNullException(House.XmlNames.Id));
                    var objectId = int.Parse(housesXmlReader.GetAttribute(House.XmlNames.ObjectId) ?? throw new ArgumentNullException(House.XmlNames.ObjectId));
                    var houseNum = housesXmlReader.GetAttribute(House.XmlNames.HouseNum) ?? throw new ArgumentNullException(House.XmlNames.HouseNum);
                    var houseType = int.Parse(housesXmlReader.GetAttribute(House.XmlNames.HouseType) ?? throw new ArgumentNullException(House.XmlNames.HouseType));

                    yield return new House(id, objectId, houseNum, houseType);
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
        var apartmentsXmlReader = _readers[Apartment.XmlElementName];

        for (int i = 0; i < _bucketSize; i++)
        {
            CanReadApartments = await apartmentsXmlReader.ReadAsync();

            if (apartmentsXmlReader is { NodeType: XmlNodeType.Element, Name: Apartment.XmlElementName })
            {
                var isActualAttribute = apartmentsXmlReader.GetAttribute("ISACTUAL");
                var isActiveAttribute = apartmentsXmlReader.GetAttribute("ISACTIVE");

                var isActual = string.IsNullOrEmpty(isActualAttribute) || isActualAttribute == "1";
                var isActive = string.IsNullOrEmpty(isActiveAttribute) || isActiveAttribute == "1";

                if (isActual && isActive)
                {
                    var id = int.Parse(apartmentsXmlReader.GetAttribute(Apartment.XmlNames.Id) ?? throw new ArgumentNullException(Apartment.XmlNames.Id));
                    var objectId = int.Parse(apartmentsXmlReader.GetAttribute(Apartment.XmlNames.ObjectId) ?? throw new ArgumentNullException(Apartment.XmlNames.ObjectId));
                    var number = apartmentsXmlReader.GetAttribute(Apartment.XmlNames.Number) ?? throw new ArgumentNullException(Apartment.XmlNames.Number);
                    var apartType = int.Parse(apartmentsXmlReader.GetAttribute(Apartment.XmlNames.ApartType) ?? throw new ArgumentNullException(Apartment.XmlNames.ApartType));

                    yield return new Apartment(id, objectId, number, apartType);
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
        var roomsXmlReader = _readers[Room.XmlElementName];

        for (int i = 0; i < _bucketSize; i++)
        {
            CanReadRooms = await roomsXmlReader.ReadAsync();

            if (roomsXmlReader is { NodeType: XmlNodeType.Element, Name: Room.XmlElementName })
            {
                var isActualAttribute = roomsXmlReader.GetAttribute("ISACTUAL");
                var isActiveAttribute = roomsXmlReader.GetAttribute("ISACTIVE");

                var isActual = string.IsNullOrEmpty(isActualAttribute) || isActualAttribute == "1";
                var isActive = string.IsNullOrEmpty(isActiveAttribute) || isActiveAttribute == "1";

                if (isActual && isActive)
                {
                    var id = int.Parse(roomsXmlReader.GetAttribute(Room.XmlNames.Id) ?? throw new ArgumentNullException(Room.XmlNames.Id));
                    var objectId = int.Parse(roomsXmlReader.GetAttribute(Room.XmlNames.ObjectId) ?? throw new ArgumentNullException(Room.XmlNames.ObjectId));
                    var number = roomsXmlReader.GetAttribute(Room.XmlNames.Number) ?? throw new ArgumentNullException(Room.XmlNames.Number);
                    var roomType = int.Parse(roomsXmlReader.GetAttribute(Room.XmlNames.RoomType) ?? throw new ArgumentNullException(Room.XmlNames.RoomType));

                    yield return new Room(id, objectId, number, roomType);
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
        var steadsXmlReader = _readers[Stead.XmlElementName];

        for (int i = 0; i < _bucketSize; i++)
        {
            CanReadSteads = await steadsXmlReader.ReadAsync();

            if (steadsXmlReader is { NodeType: XmlNodeType.Element, Name: Stead.XmlElementName })
            {
                var isActualAttribute = steadsXmlReader.GetAttribute("ISACTUAL");
                var isActiveAttribute = steadsXmlReader.GetAttribute("ISACTIVE");

                var isActual = string.IsNullOrEmpty(isActualAttribute) || isActualAttribute == "1";
                var isActive = string.IsNullOrEmpty(isActiveAttribute) || isActiveAttribute == "1";

                if (isActual && isActive)
                {
                    var id = int.Parse(steadsXmlReader.GetAttribute(Stead.XmlNames.Id) ?? throw new ArgumentNullException(Stead.XmlNames.Id));
                    var objectId = int.Parse(steadsXmlReader.GetAttribute(Stead.XmlNames.ObjectId) ?? throw new ArgumentNullException(Stead.XmlNames.ObjectId));
                    var number = steadsXmlReader.GetAttribute(Stead.XmlNames.Number) ?? throw new ArgumentNullException(Stead.XmlNames.Number);

                    yield return new Stead(id, objectId, number);
                }
            }
        }
    }

    /// <summary>
    /// Reads the hierarchies asynchronously.
    /// </summary>
    /// <returns>An asynchronous enumerable of <see cref="Hierarchy"/>.</returns>
    public async IAsyncEnumerable<Hierarchy> ReadHierarchiesAsync()
    {
        var hierarchiesXmlReader = _readers[Hierarchy.XmlElementName];

        for (int i = 0; i < _bucketSize; i++)
        {
            CanReadHierarchies = await hierarchiesXmlReader.ReadAsync();

            if (hierarchiesXmlReader is { NodeType: XmlNodeType.Element, Name: Hierarchy.XmlElementName })
            {
                var isActualAttribute = hierarchiesXmlReader.GetAttribute("ISACTUAL");
                var isActiveAttribute = hierarchiesXmlReader.GetAttribute("ISACTIVE");

                var isActual = string.IsNullOrEmpty(isActualAttribute) || isActualAttribute == "1";
                var isActive = string.IsNullOrEmpty(isActiveAttribute) || isActiveAttribute == "1";

                if (isActual && isActive)
                {
                    var id = int.Parse(hierarchiesXmlReader.GetAttribute(Hierarchy.XmlNames.Id) ?? throw new ArgumentNullException(Hierarchy.XmlNames.Id));
                    var objectId = int.Parse(hierarchiesXmlReader.GetAttribute(Hierarchy.XmlNames.ObjectId) ?? throw new ArgumentNullException(Hierarchy.XmlNames.ObjectId));
                    var path = hierarchiesXmlReader.GetAttribute(Hierarchy.XmlNames.Path) ?? throw new ArgumentNullException(Hierarchy.XmlNames.Path);

                    yield return new Hierarchy(id, objectId, path);
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
            _zipArchive.Dispose();

            foreach (var (_, reader) in _readers)
            {
                reader.Dispose();
            }
        }

        _disposed = true;
    }

    private XmlReader CreateAsyncXmlReader(string element)
    {
        var regex = _regexes[element];

        var archiveEntry = _zipArchive.Entries.First(file => regex.IsMatch(file.FullName));
        var stream = archiveEntry.Open();

        return XmlReader.Create(stream, new XmlReaderSettings { Async = true });
    }
}
