namespace GAR.Services.ReaderApi.Services;

using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Xml;
using GAR.Services.ReaderApi.Entities;
using GAR.Services.ReaderApi.Models;

/// <summary>
/// Service for reading XML files from a ZIP archive.
/// </summary>
public partial class ZipXmlReaderService
    : IDisposable
{
    private readonly IConfiguration _configuration;
    private readonly ZipArchive _zipArchive;

    private bool _disposedValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="ZipXmlReaderService"/> class.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public ZipXmlReaderService(
        IConfiguration configuration)
    {
        _configuration = configuration;
        _zipArchive = ZipFile.OpenRead(@$"Data/gar_xml.zip");

        FillTypesRegexes();
        FillDataRegexes();

        FillTypesProperties();
        FillDataProperties();
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    /// <summary>
    /// Gets the XML reader for the Addresses.
    /// </summary>
    public XmlReader Addresses { get; private set; }

    /// <summary>
    /// Gets the XML reader for the Houses.
    /// </summary>
    public XmlReader Houses { get; private set; }

    /// <summary>
    /// Gets the XML reader for the Apartments.
    /// </summary>
    public XmlReader Apartments { get; private set; }

    /// <summary>
    /// Gets the XML reader for the Rooms.
    /// </summary>
    public XmlReader Rooms { get; private set; }

    /// <summary>
    /// Gets the XML reader for the Steads.
    /// </summary>
    public XmlReader Steads { get; private set; }

    /// <summary>
    /// Gets the XML reader for the Hierarchies.
    /// </summary>
    public XmlReader Hierarchies { get; private set; }

    /// <summary>
    /// Gets the XML reader for the AddressTypes.
    /// </summary>
    public XmlReader AddressTypes { get; private set; }

    /// <summary>
    /// Gets the XML reader for the HouseTypes.
    /// </summary>
    public XmlReader HouseTypes { get; private set; }

    /// <summary>
    /// Gets the XML reader for the ApartmentTypes.
    /// </summary>
    public XmlReader ApartmentTypes { get; private set; }

    /// <summary>
    /// Gets the XML reader for the RoomTypes.
    /// </summary>
    public XmlReader RoomTypes { get; private set; }

    private Dictionary<string, Regex> Regexes { get; set; } = [];

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes the resources used by the <see cref="ZipXmlReaderService"/>.
    /// </summary>
    /// <param name="disposing">A value indicating whether the method is being called from the <see cref="Dispose"/> method.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposedValue)
        {
            return;
        }

        if (disposing)
        {
            _zipArchive.Dispose();

            Addresses.Dispose();
            Houses.Dispose();
            Apartments.Dispose();
            Rooms.Dispose();
            Steads.Dispose();
            Hierarchies.Dispose();
            AddressTypes.Dispose();
            HouseTypes.Dispose();
            ApartmentTypes.Dispose();
            RoomTypes.Dispose();
        }

        _disposedValue = true;
    }

    [GeneratedRegex(@"^AS_ADDR_OBJ_TYPES_(\d{8})_.+\.XML$")]
    private static partial Regex AddressObjectTypeRegex();

    [GeneratedRegex(@"^AS_HOUSE_TYPES_(\d{8})_.+\.XML$")]
    private static partial Regex HouseTypeRegex();

    [GeneratedRegex(@"^AS_APARTMENT_TYPES_(\d{8})_.+\.XML$")]
    private static partial Regex ApartmentTypeRegex();

    [GeneratedRegex(@"^AS_ROOM_TYPES_(\d{8})_.+\.XML$")]
    private static partial Regex RoomTypeRegex();

    private XmlReader CreateAsyncXmlReader(string elementName)
    {
        var regex = Regexes[elementName];

        var archiveEntry = _zipArchive.Entries.First(file => regex.IsMatch(file.FullName));
        var stream = archiveEntry.Open();

        return XmlReader.Create(stream, new XmlReaderSettings { Async = true });
    }

    private void FillTypesRegexes()
    {
        var folderPath = _configuration["GAR:Folder"]
            ?? throw new ArgumentNullException("FolderPath", "FolderPath should be configured in Configuration.");

        Regexes.Add(nameof(AddressObjectType), AddressObjectTypeRegex());
        Regexes.Add(nameof(HouseType), HouseTypeRegex());
        Regexes.Add(nameof(ApartmentType), ApartmentTypeRegex());
        Regexes.Add(nameof(RoomType), RoomTypeRegex());
    }

    private void FillDataRegexes()
    {
        var folderPath = _configuration["GAR:Folder"]
            ?? throw new ArgumentNullException("FolderPath", "FolderPath should be configured in Configuration.");

        Regexes.Add(nameof(AddressObject), new Regex($@"^{folderPath}/AS_ADDR_OBJ_(\d{{8}})_.+\.XML$", RegexOptions.Compiled));
        Regexes.Add(nameof(House), new Regex($@"^{folderPath}/AS_HOUSES_(\d{{8}})_.+\.XML$", RegexOptions.Compiled));
        Regexes.Add(nameof(Apartment), new Regex($@"^{folderPath}/AS_APARTMENTS_(\d{{8}})_.+\.XML$", RegexOptions.Compiled));
        Regexes.Add(nameof(Room), new Regex($@"^{folderPath}/AS_ROOMS_(\d{{8}})_.+\.XML$", RegexOptions.Compiled));
        Regexes.Add(nameof(Stead), new Regex($@"^{folderPath}/AS_STEADS_(\d{{8}})_.+\.XML$", RegexOptions.Compiled));
        Regexes.Add(nameof(Hierarchy), new Regex($@"^{folderPath}/AS_ADM_HIERARCHY_(\d{{8}})_.+\.XML$", RegexOptions.Compiled));
    }

    private void FillTypesProperties()
    {
        AddressTypes = CreateAsyncXmlReader(nameof(AddressObjectType));
        HouseTypes = CreateAsyncXmlReader(nameof(HouseType));
        ApartmentTypes = CreateAsyncXmlReader(nameof(ApartmentType));
        RoomTypes = CreateAsyncXmlReader(nameof(RoomType));
    }

    private void FillDataProperties()
    {
        Addresses = CreateAsyncXmlReader(nameof(AddressObject));
        Houses = CreateAsyncXmlReader(nameof(House));
        Apartments = CreateAsyncXmlReader(nameof(Apartment));
        Rooms = CreateAsyncXmlReader(nameof(Room));
        Steads = CreateAsyncXmlReader(nameof(Stead));
        Hierarchies = CreateAsyncXmlReader(nameof(Hierarchy));
    }
}
