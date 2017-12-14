namespace SasTokenService.Entities
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    /// <summary>
    /// Class To Laod The Context
    /// </summary>
    public partial class PersistenceContext : DbContext
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PersistenceContext() : base("name=DeviceModel")
        {
        }
        /// <summary>
        /// Db Set for Device
        /// </summary>
        public virtual DbSet<Device> Devices { get; set; }
        /// <summary>
        /// Method To update the Context
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Device>()
                .Property(e => e.Name)
                .IsUnicode(true);

            modelBuilder.Entity<Device>()
                .Property(e => e.DeviceKey)
                .IsUnicode(true);

            modelBuilder.Entity<Device>()
                .Property(e => e.IP)
                .IsUnicode(false);

            modelBuilder.Entity<Device>()
                .Property(e => e.Owner)
                .IsUnicode(true);

            modelBuilder.Entity<Device>()
                .Property(e => e.GeoPosition)
                .IsUnicode(false);

            modelBuilder.Entity<Device>()
                .Property(e => e.Uri)
                .IsUnicode(false);
        }
    }
}
