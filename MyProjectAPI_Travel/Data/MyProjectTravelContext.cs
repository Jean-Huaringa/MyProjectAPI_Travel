using Microsoft.EntityFrameworkCore;

namespace MyProjectAPI_Travel.Data;

public partial class MyProjectTravelContext : DbContext
{
    public MyProjectTravelContext()
    {
    }

    public MyProjectTravelContext(DbContextOptions<MyProjectTravelContext> options)
    : base(options)
    {
    }


    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

