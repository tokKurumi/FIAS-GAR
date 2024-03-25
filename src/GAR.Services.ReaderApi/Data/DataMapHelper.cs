namespace GAR.Services.ReaderApi.Data;

using GAR.Services.ReaderApi.Models;
using PostgreSQLCopyHelper;

public class DataMapHelper
{
    public IPostgreSQLCopyHelper<AddressObject> Addresses =>
        new PostgreSQLCopyHelper<AddressObject>("public", AddressObject.TableEntityName)
            .UsePostgresQuoting()
            .MapInteger(nameof(AddressObject.Id), x => x.Id)
            .MapInteger(nameof(AddressObject.ObjectId), x => x.ObjectId)
            .MapText(nameof(AddressObject.TypeName), x => x.TypeName)
            .MapText(nameof(AddressObject.Name), x => x.Name);

    public IPostgreSQLCopyHelper<Apartment> Apartments =>
        new PostgreSQLCopyHelper<Apartment>("public", Apartment.TableEntityName)
            .UsePostgresQuoting()
            .MapInteger(nameof(Apartment.Id), x => x.Id)
            .MapInteger(nameof(Apartment.ObjectId), x => x.ObjectId)
            .MapText(nameof(Apartment.Number), x => x.Number)
            .MapInteger(nameof(Apartment.ApartType), x => x.ApartType);

    public IPostgreSQLCopyHelper<Hierarchy> Hierarchies =>
        new PostgreSQLCopyHelper<Hierarchy>("public", Hierarchy.TableEntityName)
            .UsePostgresQuoting()
            .MapInteger(nameof(Hierarchy.Id), x => x.Id)
            .MapInteger(nameof(Hierarchy.ObjectId), x => x.ObjectId)
            .MapText(nameof(Hierarchy.Path), x => x.Path);

    public IPostgreSQLCopyHelper<House> Houses =>
        new PostgreSQLCopyHelper<House>("public", House.TableEntityName)
            .UsePostgresQuoting()
            .MapInteger(nameof(House.Id), x => x.Id)
            .MapInteger(nameof(House.ObjectId), x => x.ObjectId)
            .MapText(nameof(House.HouseNum), x => x.HouseNum)
            .MapInteger(nameof(House.HouseType), x => x.HouseType);

    public IPostgreSQLCopyHelper<Room> Rooms =>
        new PostgreSQLCopyHelper<Room>("public", Room.TableEntityName)
            .UsePostgresQuoting()
            .MapInteger(nameof(Room.Id), x => x.Id)
            .MapInteger(nameof(Room.ObjectId), x => x.ObjectId)
            .MapText(nameof(Room.Number), x => x.Number)
            .MapInteger(nameof(Room.RoomType), x => x.RoomType);

    public IPostgreSQLCopyHelper<Stead> Steads =>
        new PostgreSQLCopyHelper<Stead>("public", Stead.TableEntityName)
            .UsePostgresQuoting()
            .MapInteger(nameof(Stead.Id), x => x.Id)
            .MapInteger(nameof(Stead.ObjectId), x => x.ObjectId)
            .MapText(nameof(Stead.Number), x => x.Number);
}
