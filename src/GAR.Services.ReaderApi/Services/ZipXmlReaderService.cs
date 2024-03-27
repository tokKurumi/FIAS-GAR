namespace GAR.Services.ReaderApi.Services;

using System.Collections.Generic;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using GAR.Services.ReaderApi.Models;

/// <summary>
/// Provides a service for reading objects from a ZIP file containing XML data.
/// </summary>
public partial class ZipXmlReaderService : IDisposable
{
    private readonly string _folderPath;
    private readonly int _bucketSize;

    private readonly ZipArchive _zipArchive;
    private readonly Dictionary<string, Regex> _regexes;
    private readonly Dictionary<string, XmlReader> _readers;

    private Dictionary<string, AddressObjectType> _addressObjectTypes = [];
    private Dictionary<int, HouseType> _houseTypes = [];
    private Dictionary<int, ApartmentType> _apartmentTypes = [];
    private Dictionary<int, RoomType> _roomTypes = [];

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
            [AddressObjectType.XmlElementName] = AddressObjectTypeRegex(),
            [HouseType.XmlElementName] = HouseTypeRegex(),
            [ApartmentType.XmlElementName] = ApartmentTypeRegex(),
            [RoomType.XmlElementName] = RoomTypeRegex(),

            [AddressObject.XmlElementName] = new Regex($@"^{folderPath}/AS_ADDR_OBJ_(\d{{8}})_.+\.XML$"),
            [House.XmlElementName] = new Regex($@"^{_folderPath}/AS_HOUSES_(\d{{8}})_.+\.XML$"),
            [Apartment.XmlElementName] = new Regex($@"^{_folderPath}/AS_APARTMENTS_(\d{{8}})_.+\.XML$"),
            [Room.XmlElementName] = new Regex($@"^{_folderPath}/AS_ROOMS_(\d{{8}})_.+\.XML$"),
            [Stead.XmlElementName] = new Regex($@"^{_folderPath}/AS_STEADS_(\d{{8}})_.+\.XML$"),
            [Hierarchy.XmlElementName] = new Regex($@"^{_folderPath}/AS_ADM_HIERARCHY_(\d{{8}})_.+\.XML$"),
        };

        _readers = new Dictionary<string, XmlReader>
        {
            [AddressObjectType.XmlElementName] = CreateAsyncXmlReader(AddressObjectType.XmlElementName),
            [HouseType.XmlElementName] = CreateAsyncXmlReader(HouseType.XmlElementName),
            [ApartmentType.XmlElementName] = CreateAsyncXmlReader(ApartmentType.XmlElementName),
            [RoomType.XmlElementName] = CreateAsyncXmlReader(RoomType.XmlElementName),

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

    private bool CanReadAddressObjectTypes { get; set; } = true;

    private bool CanReadHouseTypes { get; set; } = true;

    private bool CanReadApartmentTypes { get; set; } = true;

    private bool CanReadRoomTypes { get; set; } = true;

    /// <summary>
    /// Initializes the types asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task InitTypesAsync()
    {
        _addressObjectTypes = await ReadAddressObjectTypesAsync()
            .GroupBy(t => t.ShortName)
            .SelectAwait(async group => await group.FirstAsync())
            .ToDictionaryAsync(keySelector: item => item.ShortName, elementSelector: item => item);

        _houseTypes = await ReadHouseTypesAsync()
            .ToDictionaryAsync(keySelector: item => item.Id, elementSelector: item => item);

        _apartmentTypes = await ReadApartmentTypesAsync()
            .ToDictionaryAsync(keySelector: item => item.Id, elementSelector: item => item);

        _roomTypes = await ReadRoomTypesAsync()
            .ToDictionaryAsync(keySelector: item => item.Id, elementSelector: item => item);
    }

    /// <summary>
    /// Reads the objects asynchronously.
    /// </summary>
    /// <returns>An asynchronous enumerable of <see cref="AddressObject"/>.</returns>
    public async IAsyncEnumerable<AddressObject> ReadObjectsAsync()
    {
        var addressesXmlReader = _readers[AddressObject.XmlElementName];
        var fullNameBuilder = new StringBuilder();

        for (int i = 0; i < _bucketSize && CanReadObjects; i++)
        {
            CanReadObjects = await addressesXmlReader.ReadAsync();

            if (addressesXmlReader is { NodeType: XmlNodeType.Element, Name: AddressObject.XmlElementName })
            {
                var (isActual, isActive) = GetActualAndActive(addressesXmlReader);

                if (isActual && isActive)
                {
                    var id = int.Parse(addressesXmlReader.GetAttribute(AddressObject.XmlNames.Id) ?? throw new ArgumentNullException(AddressObject.XmlNames.Id));
                    var objectId = int.Parse(addressesXmlReader.GetAttribute(AddressObject.XmlNames.ObjectId) ?? throw new ArgumentNullException(AddressObject.XmlNames.ObjectId));
                    var typeName = addressesXmlReader.GetAttribute(AddressObject.XmlNames.TypeName) ?? throw new ArgumentNullException(AddressObject.XmlNames.TypeName);
                    var name = addressesXmlReader.GetAttribute(AddressObject.XmlNames.Name) ?? throw new ArgumentNullException(AddressObject.XmlNames.Name);

                    if (_addressObjectTypes.TryGetValue(typeName, out var addressObjectType))
                    {
                        fullNameBuilder.Clear();
                        fullNameBuilder.Append(addressObjectType.Name).Append(' ').Append(name);
                    }
                    else
                    {
                        fullNameBuilder.Clear();
                        fullNameBuilder.Append(name);
                    }

                    yield return new AddressObject(id, objectId, fullNameBuilder.ToString());
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
        var fullNameBuilder = new StringBuilder();

        for (int i = 0; i < _bucketSize && CanReadHouses; i++)
        {
            CanReadHouses = await housesXmlReader.ReadAsync();

            if (housesXmlReader is { NodeType: XmlNodeType.Element, Name: House.XmlElementName })
            {
                var (isActual, isActive) = GetActualAndActive(housesXmlReader);

                if (isActual && isActive)
                {
                    var id = int.Parse(housesXmlReader.GetAttribute(House.XmlNames.Id) ?? throw new ArgumentNullException(House.XmlNames.Id));
                    var objectId = int.Parse(housesXmlReader.GetAttribute(House.XmlNames.ObjectId) ?? throw new ArgumentNullException(House.XmlNames.ObjectId));
                    var houseNum = housesXmlReader.GetAttribute(House.XmlNames.HouseNum) ?? throw new ArgumentNullException(House.XmlNames.HouseNum);
                    var houseTypeId = int.Parse(housesXmlReader.GetAttribute(House.XmlNames.HouseType) ?? throw new ArgumentNullException(House.XmlNames.HouseType));

                    if (_houseTypes.TryGetValue(houseTypeId, out var houseType))
                    {
                        fullNameBuilder.Clear();
                        fullNameBuilder.Append(houseType.Name).Append(' ').Append(houseNum);
                    }
                    else
                    {
                        fullNameBuilder.Clear();
                        fullNameBuilder.Append(houseNum);
                    }

                    yield return new House(id, objectId, fullNameBuilder.ToString());
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
        var fullNameBuilder = new StringBuilder();

        for (int i = 0; i < _bucketSize && CanReadApartments; i++)
        {
            CanReadApartments = await apartmentsXmlReader.ReadAsync();

            if (apartmentsXmlReader is { NodeType: XmlNodeType.Element, Name: Apartment.XmlElementName })
            {
                var (isActual, isActive) = GetActualAndActive(apartmentsXmlReader);

                if (isActual && isActive)
                {
                    var id = int.Parse(apartmentsXmlReader.GetAttribute(Apartment.XmlNames.Id) ?? throw new ArgumentNullException(Apartment.XmlNames.Id));
                    var objectId = int.Parse(apartmentsXmlReader.GetAttribute(Apartment.XmlNames.ObjectId) ?? throw new ArgumentNullException(Apartment.XmlNames.ObjectId));
                    var number = apartmentsXmlReader.GetAttribute(Apartment.XmlNames.Number) ?? throw new ArgumentNullException(Apartment.XmlNames.Number);
                    var apartType = int.Parse(apartmentsXmlReader.GetAttribute(Apartment.XmlNames.ApartType) ?? throw new ArgumentNullException(Apartment.XmlNames.ApartType));

                    if (_apartmentTypes.TryGetValue(apartType, out var apartmentType))
                    {
                        fullNameBuilder.Clear();
                        fullNameBuilder.Append(apartmentType.Name).Append(' ').Append(number);
                    }
                    else
                    {
                        fullNameBuilder.Clear();
                        fullNameBuilder.Append(number);
                    }

                    yield return new Apartment(id, objectId, fullNameBuilder.ToString());
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
        var fullNameBuilder = new StringBuilder();

        for (int i = 0; i < _bucketSize && CanReadRooms; i++)
        {
            CanReadRooms = await roomsXmlReader.ReadAsync();

            if (roomsXmlReader is { NodeType: XmlNodeType.Element, Name: Room.XmlElementName })
            {
                var (isActual, isActive) = GetActualAndActive(roomsXmlReader);

                if (isActual && isActive)
                {
                    var id = int.Parse(roomsXmlReader.GetAttribute(Room.XmlNames.Id) ?? throw new ArgumentNullException(Room.XmlNames.Id));
                    var objectId = int.Parse(roomsXmlReader.GetAttribute(Room.XmlNames.ObjectId) ?? throw new ArgumentNullException(Room.XmlNames.ObjectId));
                    var number = roomsXmlReader.GetAttribute(Room.XmlNames.Number) ?? throw new ArgumentNullException(Room.XmlNames.Number);
                    var roomTypeId = int.Parse(roomsXmlReader.GetAttribute(Room.XmlNames.RoomType) ?? throw new ArgumentNullException(Room.XmlNames.RoomType));

                    if (_roomTypes.TryGetValue(roomTypeId, out var roomType))
                    {
                        fullNameBuilder.Clear();
                        fullNameBuilder.Append(roomType.Name).Append(' ').Append(number);
                    }
                    else
                    {
                        fullNameBuilder.Clear();
                        fullNameBuilder.Append(number);
                    }

                    yield return new Room(id, objectId, fullNameBuilder.ToString());
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

        for (int i = 0; i < _bucketSize && CanReadSteads; i++)
        {
            CanReadSteads = await steadsXmlReader.ReadAsync();

            if (steadsXmlReader is { NodeType: XmlNodeType.Element, Name: Stead.XmlElementName })
            {
                var (isActual, isActive) = GetActualAndActive(steadsXmlReader);

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

        for (int i = 0; i < _bucketSize && CanReadHierarchies; i++)
        {
            CanReadHierarchies = await hierarchiesXmlReader.ReadAsync();

            if (hierarchiesXmlReader is { NodeType: XmlNodeType.Element, Name: Hierarchy.XmlElementName })
            {
                var (isActual, isActive) = GetActualAndActive(hierarchiesXmlReader);

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

    [GeneratedRegex(@"^AS_ADDR_OBJ_TYPES_(\d{8})_.+\.XML$")]
    private static partial Regex AddressObjectTypeRegex();

    [GeneratedRegex(@"^AS_HOUSE_TYPES_(\d{8})_.+\.XML$")]
    private static partial Regex HouseTypeRegex();

    [GeneratedRegex(@"^AS_APARTMENT_TYPES_(\d{8})_.+\.XML$")]
    private static partial Regex ApartmentTypeRegex();

    [GeneratedRegex(@"^AS_ROOM_TYPES_(\d{8})_.+\.XML$")]
    private static partial Regex RoomTypeRegex();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static (bool IsActual, bool IsActive) GetActualAndActive(XmlReader reader)
    {
        var isActualAttribute = reader.GetAttribute("ISACTUAL");
        var isActiveAttribute = reader.GetAttribute("ISACTIVE");

        bool isActual = isActualAttribute == null || isActualAttribute == "1" || (bool.TryParse(isActualAttribute, out var isActualValue) && isActualValue);
        bool isActive = isActiveAttribute == null || isActiveAttribute == "1" || (bool.TryParse(isActiveAttribute, out var isActiveValue) && isActiveValue);

        return (isActual, isActive);
    }

    private async IAsyncEnumerable<AddressObjectType> ReadAddressObjectTypesAsync()
    {
        var addressObjectTypesXmlReader = _readers[AddressObjectType.XmlElementName];

        while (CanReadAddressObjectTypes)
        {
            CanReadAddressObjectTypes = await addressObjectTypesXmlReader.ReadAsync();

            if (addressObjectTypesXmlReader is { NodeType: XmlNodeType.Element, Name: AddressObjectType.XmlElementName })
            {
                var isActualAttribute = addressObjectTypesXmlReader.GetAttribute("ISACTUAL");
                var isActiveAttribute = addressObjectTypesXmlReader.GetAttribute("ISACTIVE");

                var isActual = string.IsNullOrEmpty(isActualAttribute) || isActualAttribute == "true";
                var isActive = string.IsNullOrEmpty(isActiveAttribute) || isActiveAttribute == "true";

                if (isActual && isActive)
                {
                    var shortName = addressObjectTypesXmlReader.GetAttribute(AddressObjectType.XmlNames.ShortName) ?? throw new ArgumentNullException(AddressObjectType.XmlNames.ShortName);
                    var name = addressObjectTypesXmlReader.GetAttribute(AddressObjectType.XmlNames.Name) ?? throw new ArgumentNullException(AddressObjectType.XmlNames.Name);

                    yield return new AddressObjectType(shortName, name);
                }
            }
        }
    }

    private async IAsyncEnumerable<HouseType> ReadHouseTypesAsync()
    {
        var houseTypesXmlReader = _readers[HouseType.XmlElementName];

        while (CanReadHouseTypes)
        {
            CanReadHouseTypes = await houseTypesXmlReader.ReadAsync();

            if (houseTypesXmlReader is { NodeType: XmlNodeType.Element, Name: HouseType.XmlElementName })
            {
                var isActualAttribute = houseTypesXmlReader.GetAttribute("ISACTUAL");
                var isActiveAttribute = houseTypesXmlReader.GetAttribute("ISACTIVE");

                var isActual = string.IsNullOrEmpty(isActualAttribute) || isActualAttribute == "true";
                var isActive = string.IsNullOrEmpty(isActiveAttribute) || isActiveAttribute == "true";

                if (isActual && isActive)
                {
                    var id = int.Parse(houseTypesXmlReader.GetAttribute(HouseType.XmlNames.Id) ?? throw new ArgumentNullException(HouseType.XmlNames.Id));
                    var name = houseTypesXmlReader.GetAttribute(HouseType.XmlNames.Name) ?? throw new ArgumentNullException(HouseType.XmlNames.Name);

                    yield return new HouseType(id, name);
                }
            }
        }
    }

    private async IAsyncEnumerable<ApartmentType> ReadApartmentTypesAsync()
    {
        var apartmentTypesXmlReader = _readers[ApartmentType.XmlElementName];

        while (CanReadApartmentTypes)
        {
            CanReadApartmentTypes = await apartmentTypesXmlReader.ReadAsync();

            if (apartmentTypesXmlReader is { NodeType: XmlNodeType.Element, Name: ApartmentType.XmlElementName })
            {
                var isActualAttribute = apartmentTypesXmlReader.GetAttribute("ISACTUAL");
                var isActiveAttribute = apartmentTypesXmlReader.GetAttribute("ISACTIVE");

                var isActual = string.IsNullOrEmpty(isActualAttribute) || isActualAttribute == "true";
                var isActive = string.IsNullOrEmpty(isActiveAttribute) || isActiveAttribute == "true";

                if (isActual && isActive)
                {
                    var id = int.Parse(apartmentTypesXmlReader.GetAttribute(ApartmentType.XmlNames.Id) ?? throw new ArgumentNullException(ApartmentType.XmlNames.Id));
                    var name = apartmentTypesXmlReader.GetAttribute(ApartmentType.XmlNames.Name) ?? throw new ArgumentNullException(ApartmentType.XmlNames.Name);

                    yield return new ApartmentType(id, name);
                }
            }
        }
    }

    private async IAsyncEnumerable<RoomType> ReadRoomTypesAsync()
    {
        var roomTypesXmlReader = _readers[RoomType.XmlElementName];

        while (CanReadRoomTypes)
        {
            CanReadRoomTypes = await roomTypesXmlReader.ReadAsync();

            if (roomTypesXmlReader is { NodeType: XmlNodeType.Element, Name: RoomType.XmlElementName })
            {
                var isActualAttribute = roomTypesXmlReader.GetAttribute("ISACTUAL");
                var isActiveAttribute = roomTypesXmlReader.GetAttribute("ISACTIVE");

                var isActual = string.IsNullOrEmpty(isActualAttribute) || isActualAttribute == "true";
                var isActive = string.IsNullOrEmpty(isActiveAttribute) || isActiveAttribute == "true";

                if (isActual && isActive)
                {
                    var id = int.Parse(roomTypesXmlReader.GetAttribute(RoomType.XmlNames.Id) ?? throw new ArgumentNullException(RoomType.XmlNames.Id));
                    var name = roomTypesXmlReader.GetAttribute(RoomType.XmlNames.Name) ?? throw new ArgumentNullException(RoomType.XmlNames.Name);

                    yield return new RoomType(id, name);
                }
            }
        }
    }

    private XmlReader CreateAsyncXmlReader(string element)
    {
        var regex = _regexes[element];

        var archiveEntry = _zipArchive.Entries.First(file => regex.IsMatch(file.FullName));
        var stream = archiveEntry.Open();

        return XmlReader.Create(stream, new XmlReaderSettings { Async = true });
    }
}
