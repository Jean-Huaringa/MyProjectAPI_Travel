using Microsoft.EntityFrameworkCore;
using MyProjectAPI_Travel.Models;

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

    public virtual DbSet<Seat> TbSeating { get; set; }

    public virtual DbSet<Ticket> TbTickets { get; set; }

    public virtual DbSet<Bus> TbBus { get; set; }

    public virtual DbSet<Station> TbStations { get; set; }

    public virtual DbSet<Itinerary> TbItineraries { get; set; }

    public virtual DbSet<Worker> TbWorkers { get; set; }

    public virtual DbSet<User> TbUsers { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Seat>(entity =>
        {
            entity.HasKey(e => new { e.IdBus, e.Column, e.Row });

            entity.Property(e => e.State).HasDefaultValue(true);
            entity.Property(e => e.Busy).HasDefaultValue(false);

            entity.HasOne(d => d.Bus)
                .WithMany(p => p.Seating)
                .HasForeignKey(d => d.IdBus)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tb_seating_bus");

        });

        modelBuilder.Entity<Bus>(entity =>
        {
            entity.HasKey(e => e.IdBus);

            entity.Property(e => e.State).HasDefaultValue(true);
            entity.Property(e => e.Availability).HasDefaultValue(true);
        });

        modelBuilder.Entity<Station>(entity =>
        {
            entity.Property(e => e.State).HasDefaultValue(true);
            entity.HasKey(e => e.IdStn);
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.IdTck);

            entity.Property(e => e.State).HasDefaultValue(true);
            entity.Property(e => e.Problem).HasDefaultValue(false);
            entity.Property(e => e.CreationDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(u => u.User).WithMany(b => b.Tickets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tb_ticket_user");
        });

        modelBuilder.Entity<Itinerary>(entity =>
        {
            entity.HasKey(e => e.IdItn);

            entity.Property(e => e.State).HasDefaultValue(true);
            entity.Property(e => e.Availability).HasDefaultValue(true);

            entity.HasOne(d => d.Bus).WithMany(p => p.Itineraries)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tb_itinerary_bus");

            entity.HasOne(d => d.Destination).WithMany(p => p.Destinations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tb_itinerary_destination");

            entity.HasOne(d => d.Origin).WithMany(p => p.Origins)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tb_itinerary_origin");

            entity.HasOne(d => d.Worker).WithMany(p => p.Itineraries)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tb_itinerary_worker");
        });

        modelBuilder.Entity<Worker>(entity =>
        {
            entity.HasKey(e => e.IdWrk);

            entity.Property(e => e.IdWrk).ValueGeneratedNever();
            entity.Property(e => e.State).HasDefaultValue(true);
            entity.Property(e => e.Availability).HasDefaultValue(true);
            entity.Property(e => e.RegistrationDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Users)
                .WithOne(p => p.Worker)
                .HasForeignKey<Worker>(d => d.IdWrk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tb_worker_user");

        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUsr);

            entity.Property(e => e.RegistrationDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.State).HasDefaultValue(true);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

