using System.Data.Entity;

namespace ForSibintek.Models
{
    class ApplicationContext:DbContext
    {
        public ApplicationContext()
            :base("DbEntityConnection")
        { }

        public DbSet<File> Files { get; set; }
        public DbSet<FileError> FileErrors { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<File>()
                .Property(p => p.Path)
                .IsRequired();
            modelBuilder
                .Entity<FileError>()
                .Property(p => p.ErrorMessage)
                .IsRequired();
            base.OnModelCreating(modelBuilder);
        }
    }
}