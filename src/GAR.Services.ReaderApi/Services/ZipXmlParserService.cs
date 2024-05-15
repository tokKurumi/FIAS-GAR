namespace GAR.Services.ReaderApi.Services;

using System.Collections.Generic;
using GAR.Services.ReaderApi.Data;
using GAR.Services.ReaderApi.Entities;
using GAR.Services.ReaderApi.Models;

/// <summary>
/// Provides a service for reading objects from a ZIP file containing XML data.
/// </summary>
public partial class ZipXmlParserService(
    ZipXmlReaderService zipXmlReaderService,
    XmlTypesCopyHelpers xmlTypesCopyHelpers,
    XmlDataCopyHelpers xmlDataCopyHelpers)
    : IDisposable
{
    private readonly ZipXmlReaderService _zipXmlReaderService = zipXmlReaderService;
    private readonly XmlTypesCopyHelpers _xmlTypesCopyHelpers = xmlTypesCopyHelpers;
    private readonly XmlDataCopyHelpers _xmlDataCopyHelpers = xmlDataCopyHelpers;

    private Dictionary<string, AddressObjectType> _addressObjectTypes = [];
    private Dictionary<int, HouseType> _houseTypes = [];
    private Dictionary<int, ApartmentType> _apartmentTypes = [];
    private Dictionary<int, RoomType> _roomTypes = [];

    private bool _disposed;

    /// <summary>
    /// Initializes the types asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task InitTypesAsync()
    {
        _addressObjectTypes = await _xmlTypesCopyHelpers.AddressTypes.GetAsync(_zipXmlReaderService.AddressTypes)
            .Distinct()
            .ToDictionaryAsync(keySelector: i => i.ShortName, elementSelector: i => i);

        _houseTypes = await _xmlTypesCopyHelpers.HouseTypes.GetAsync(_zipXmlReaderService.HouseTypes)
            .Distinct()
            .ToDictionaryAsync(keySelector: item => item.Id, elementSelector: item => item);

        _apartmentTypes = await _xmlTypesCopyHelpers.ApartmentTypes.GetAsync(_zipXmlReaderService.ApartmentTypes)
            .Distinct()
            .ToDictionaryAsync(keySelector: item => item.Id, elementSelector: item => item);

        _apartmentTypes.Add(0, new ApartmentType { Id = 0, Name = string.Empty });

        _roomTypes = await _xmlTypesCopyHelpers.RoomTypes.GetAsync(_zipXmlReaderService.RoomTypes)
            .Distinct()
            .ToDictionaryAsync(keySelector: item => item.Id, elementSelector: item => item);

        ApplyTypesMapping();
    }

    /// <summary>
    /// Reads the objects asynchronously.
    /// </summary>
    /// <returns>An asynchronous enumerable of <see cref="AddressObject"/>.</returns>
    public IAsyncEnumerable<AddressObject> ReadObjectsAsync()
    {
        var addressesXmlReader = _zipXmlReaderService.Addresses;
        return _xmlDataCopyHelpers.Addresses.GetAsync(addressesXmlReader);
    }

    /// <summary>
    /// Reads the houses asynchronously.
    /// </summary>
    /// <returns>An asynchronous enumerable of <see cref="House"/>.</returns>
    public IAsyncEnumerable<House> ReadHousesAsync()
    {
        var housesXmlReader = _zipXmlReaderService.Houses;
        return _xmlDataCopyHelpers.Houses.GetAsync(housesXmlReader);
    }

    /// <summary>
    /// Reads the apartments asynchronously.
    /// </summary>
    /// <returns>An asynchronous enumerable of <see cref="Apartment"/>.</returns>
    public IAsyncEnumerable<Apartment> ReadApartmentsAsync()
    {
        var apartmentsXmlReader = _zipXmlReaderService.Apartments;
        return _xmlDataCopyHelpers.Apartments.GetAsync(apartmentsXmlReader);
    }

    /// <summary>
    /// Reads the rooms asynchronously.
    /// </summary>
    /// <returns>An asynchronous enumerable of <see cref="Room"/>.</returns>
    public IAsyncEnumerable<Room> ReadRoomsAsync()
    {
        var roomsXmlReader = _zipXmlReaderService.Rooms;
        return _xmlDataCopyHelpers.Rooms.GetAsync(roomsXmlReader);
    }

    /// <summary>
    /// Reads the steads asynchronously.
    /// </summary>
    /// <returns>An asynchronous enumerable of <see cref="Stead"/>.</returns>
    public IAsyncEnumerable<Stead> ReadSteadsAsync()
    {
        var steadsXmlReader = _zipXmlReaderService.Steads;
        return _xmlDataCopyHelpers.Steads.GetAsync(steadsXmlReader);
    }

    /// <summary>
    /// Reads the hierarchies asynchronously.
    /// </summary>
    /// <returns>An asynchronous enumerable of <see cref="Hierarchy"/>.</returns>
    public IAsyncEnumerable<Hierarchy> ReadHierarchiesAsync()
    {
        var hierarchiesXmlReader = _zipXmlReaderService.Hierarchies;
        return _xmlDataCopyHelpers.Hierarchies.GetAsync(hierarchiesXmlReader);
    }

    /// <inheritdoc/>
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
            _zipXmlReaderService.Dispose();
        }

        _disposed = true;
    }

    private void ApplyTypesMapping()
    {
        _xmlDataCopyHelpers.Addresses
            .Map([AddressObject.XmlNames.TypeName, AddressObject.XmlNames.Name], (ao, values) => ao.FullName = $"{_addressObjectTypes[values[0]].Name} {values[1]}");

        _xmlDataCopyHelpers.Houses
            .Map([House.XmlNames.HouseType, House.XmlNames.HouseNum], (h, values) => h.FullName = $"{_houseTypes[int.Parse(values[0])].Name} {values[1]}");

        _xmlDataCopyHelpers.Apartments
            .Map([Apartment.XmlNames.ApartType, Apartment.XmlNames.Number], (a, values) => a.FullName = $"{_apartmentTypes[int.Parse(values[0])].Name} {values[1]}");

        _xmlDataCopyHelpers.Rooms
            .Map([Room.XmlNames.RoomType, Room.XmlNames.Number], (r, values) => r.FullName = $"{_roomTypes[int.Parse(values[0])].Name} {values[1]}");
    }
}
