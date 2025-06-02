using Microsoft.EntityFrameworkCore;
using Task10.Models;

namespace Task10.Data;

public class PrescriptionDbContext : DbContext
{
    public PrescriptionDbContext(DbContextOptions<PrescriptionDbContext> options) : base(options)
    {
    }

    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PrescriptionMedicament>()
            .HasKey(pm => new { pm.IdPrescription, pm.IdMedicament });
        
        modelBuilder.Entity<Doctor>().HasData(
            new Doctor
            {
                IdDoctor = 1,
                FirstName = "Andrii",
                LastName = "Meshcheriakov",
                Email = "gg@gmail.com"
            }
        );
        
        modelBuilder.Entity<Medicament>().HasData(
            new Medicament
            {
                IdMedicament = 1,
                Name = "Shiiit",
                Description = "the best shii in the world",
                Type = "Tablet"
            }
        );
    }
    
    

}
