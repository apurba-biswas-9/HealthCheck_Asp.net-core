using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HealthChecksSample.Entity
{
    public partial class EliteAttachmentContext : DbContext
    {
        public EliteAttachmentContext()
        {
        }

        public EliteAttachmentContext(DbContextOptions<EliteAttachmentContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AttachmentList> AttachmentList { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseNpgsql("Host=sgscaiu0658.in623.corpintra.net;Database=EliteAttachment;Username=eliteUser;Password=eliteUser$");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.3-servicing-35854");

            modelBuilder.Entity<AttachmentList>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("nextval('attachment_seq'::regclass)");

                entity.Property(e => e.AttachmentDesc).HasMaxLength(200);

                entity.Property(e => e.AttachmentGuid)
                    .IsRequired()
                    .HasColumnName("AttachmentGUID")
                    .HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("timestamp(6) with time zone");
            });

            modelBuilder.HasSequence("attachment_seq");
        }
    }
}
