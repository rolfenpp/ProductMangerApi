using Microsoft.EntityFrameworkCore;
using ECSProductWebAPI.Data.Entites;

namespace ECSProductWebAPI.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
       : base(options)
    { }

    // Tabeller
    public DbSet<Product> Product { get; set; }
    public DbSet<User> Users { get; set; }


    
}