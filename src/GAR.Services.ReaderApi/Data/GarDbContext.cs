namespace GAR.Services.ReaderApi.Data;

using GAR.Services.ReaderApi.Models;
using Microsoft.EntityFrameworkCore;

public class GarDbContext(DbContextOptions options)
    : DbContext(options)
{
    public DbSet<AddressObject> Addresses { get; set; }

    public DbSet<House> Houses { get; set; }

    public DbSet<Apartment> Apartments { get; set; }

    public DbSet<Room> Rooms { get; set; }

    public DbSet<Stead> Steads { get; set; }

    public DbSet<Hierarchy> Hierarchies { get; set; }
}
