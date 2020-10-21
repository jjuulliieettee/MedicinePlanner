using MedicinePlanner.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicinePlanner.Data
{
    public class ApplicationContext : DbContext
    {
        //DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<PharmaceuticalForm> PharmaceuticalForms { get; set; }
        public DbSet<FoodRelation> FoodRelations { get; set; }
        public DbSet<MedicineSchedule> MedicineSchedules { get; set; }
        public DbSet<FoodSchedule> FoodSchedules { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> opt) : base(opt)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}
