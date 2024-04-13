namespace GAR.Services.ReaderApi.Data;

using GAR.Services.ReaderApi.Entities;
using PostgreSQLCopyHelper;

public class SqlCopyHelpers
{
    public IPostgreSQLCopyHelper<AddressObject> Addresses =>
        new PostgreSQLCopyHelper<AddressObject>("public", nameof(AddressObject))
            .UsePostgresQuoting()
            .MapInteger(nameof(AddressObject.Id), x => x.Id)
            .MapInteger(nameof(AddressObject.ObjectId), x => x.ObjectId)
            .MapText(nameof(AddressObject.FullName), x => x.FullName);

    public IPostgreSQLCopyHelper<Apartment> Apartments =>
        new PostgreSQLCopyHelper<Apartment>("public", nameof(Apartment))
            .UsePostgresQuoting()
            .MapInteger(nameof(Apartment.Id), x => x.Id)
            .MapInteger(nameof(Apartment.ObjectId), x => x.ObjectId)
            .MapText(nameof(AddressObject.FullName), x => x.FullName);

    public IPostgreSQLCopyHelper<Hierarchy> Hierarchies =>
        new PostgreSQLCopyHelper<Hierarchy>("public", nameof(Hierarchy))
            .UsePostgresQuoting()
            .MapInteger(nameof(Hierarchy.Id), x => x.Id)
            .MapInteger(nameof(Hierarchy.ObjectId), x => x.ObjectId)
            .MapText(nameof(Hierarchy.Path), x => x.Path);

    public IPostgreSQLCopyHelper<House> Houses =>
        new PostgreSQLCopyHelper<House>("public", nameof(House))
            .UsePostgresQuoting()
            .MapInteger(nameof(House.Id), x => x.Id)
            .MapInteger(nameof(House.ObjectId), x => x.ObjectId)
            .MapText(nameof(AddressObject.FullName), x => x.FullName);

    public IPostgreSQLCopyHelper<Room> Rooms =>
        new PostgreSQLCopyHelper<Room>("public", nameof(Room))
            .UsePostgresQuoting()
            .MapInteger(nameof(Room.Id), x => x.Id)
            .MapInteger(nameof(Room.ObjectId), x => x.ObjectId)
            .MapText(nameof(AddressObject.FullName), x => x.FullName);

    public IPostgreSQLCopyHelper<Stead> Steads =>
        new PostgreSQLCopyHelper<Stead>("public", nameof(Stead))
            .UsePostgresQuoting()
            .MapInteger(nameof(Stead.Id), x => x.Id)
            .MapInteger(nameof(Stead.ObjectId), x => x.ObjectId)
            .MapText(nameof(AddressObject.FullName), x => x.FullName);
}
