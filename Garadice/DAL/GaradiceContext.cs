using Garadice.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Garadice.DAL
{
    public class GaradiceContext : DbContext
    {
        public GaradiceContext() : base("GaradiceContext") { }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobTrack> JobTracks { get; set; }
        public DbSet<JobTimeEntry> JobTimeEntry { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelbuilder)
        {
            modelbuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }
}