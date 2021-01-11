using Microsoft.EntityFrameworkCore;
using rescute.Domain.Aggregates;
using rescute.Domain.Entities.LogItems;
using rescute.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace rescute.Infrastructure
{
    public class rescuteContext : DbContext
    {
        public rescuteContext(DbContextOptions<rescuteContext> options) : base(options)
        {

        }
        public rescuteContext() { }
        public DbSet<Animal> Animals { get; private set; }
        public DbSet<Samaritan> Samaritans { get; private set; }

        public static string ConnectionString => "Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=rescute;Integrated Security=SSPI;";
        //AttachDBFilename=rescute.mdf
        public static string Schema => "rescute";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schema);


            // Samaritan
            modelBuilder.Entity<Samaritan>(b => b.HasKey(samaritan => samaritan.Id));
            modelBuilder.Entity<Samaritan>(b => b.Property(samaritan => samaritan.Id).HasConversion(v => v.Value.ToString(), v => Shared.Id<Samaritan>.Generate(Guid.Parse(v))));
            modelBuilder.Entity<Samaritan>(b => b.OwnsOne(samaritan => samaritan.Mobile));
            modelBuilder.Entity<Samaritan>(b => b.OwnsOne(samaritan => samaritan.FirstName));
            modelBuilder.Entity<Samaritan>(b => b.OwnsOne(samaritan => samaritan.LastName));
            modelBuilder.Entity<Samaritan>(b => b.ToTable("Samaritans"));

            // Animal
            modelBuilder.Entity<Animal>(b => b.HasKey(animal => animal.Id));
            modelBuilder.Entity<Animal>(b => b.Property(animal => animal.Id).HasConversion(v => v.Value.ToString(), v => Shared.Id<Animal>.Generate(Guid.Parse(v))));
            modelBuilder.Entity<Animal>(b => b.HasMany(animal => animal.Log));
            modelBuilder.Entity<Animal>(b => b.OwnsOne(animal => animal.Type));
            modelBuilder.Entity<Animal>(b => b.HasOne(animal => animal.IntroducedBy));
            modelBuilder.Entity<Animal>(b => b.Ignore(animal => animal.AcceptableAttachmentTypes));
            modelBuilder.Entity<Animal>(b => b.OwnsMany(animal => animal.Attachments, re =>
            {
                re.OwnsOne(attachment => attachment.Type);
                re.ToTable("AnimalAttachments");
            }));

            modelBuilder.Entity<Animal>(b => b.ToTable("Animals"));

            // ReportLogItem
            modelBuilder.Entity<LogItem>(b => b.HasKey(ReportLogItem => ReportLogItem.Id));
            modelBuilder.Entity<LogItem>(b => b.Property(ReportLogItem => ReportLogItem.Id).HasConversion(v => v.Value.ToString(), v => Shared.Id<LogItem>.Generate(Guid.Parse(v))));
            modelBuilder.Entity<LogItem>(b => b.HasOne(ReportLogItem => ReportLogItem.Samaritan));
            modelBuilder.Entity<LogItem>(b => b.ToTable("AnimalLogs"));

            // LogItemWithAttachment
            modelBuilder.Entity<LogItemWithAttachments>(b => b.Ignore(logItemWithAttachments => logItemWithAttachments.AcceptableAttachmentTypes));
            modelBuilder.Entity<LogItemWithAttachments>(b => b.OwnsMany(logItemWithAttachment => logItemWithAttachment.Attachments, re => 
            {
                re.OwnsOne(attachment => attachment.Type);
                re.ToTable("AnimalLogAttachments");
            }));
           

           


            // BillAttached
            modelBuilder.Entity<BillAttached>(b => b.Property(billAttached => billAttached.Total));
            modelBuilder.Entity<BillAttached>(b => b.Property(billAttached => billAttached.ContributionRequested));
           

            // Commented
            modelBuilder.Entity<Commented>(b => b.HasOne(commented => commented.RepliesTo));

            // Contributed
            modelBuilder.Entity<SamaritanContributed>(b => b.HasOne(contributed => contributed.Bill));


            // ImageAttached


            // StatusReported
            modelBuilder.Entity<StatusReported>(b => b.OwnsOne(statusReported => statusReported.EventLocation));

            // TransportRequested
            modelBuilder.Entity<TransportRequested>(b => b.OwnsOne(transportRequested => transportRequested.EventLocation));
            modelBuilder.Entity<TransportRequested>(b => b.OwnsOne(transportRequested => transportRequested.ToLocation));

            // VideoAttached
        }
    }
}
