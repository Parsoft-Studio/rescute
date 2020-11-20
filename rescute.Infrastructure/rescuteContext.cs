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
        public DbSet<Report> Reports { get; private set; }
        public DbSet<Animal> Animals { get; private set; }
        public DbSet<Domain.Aggregates.Samaritan> Samaritans { get; private set; }

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
            modelBuilder.Entity<Domain.Aggregates.Samaritan>(b => b.HasKey(samaritan => samaritan.Id));
            modelBuilder.Entity<Domain.Aggregates.Samaritan>(b => b.Property(samaritan => samaritan.Id).HasConversion(v => v.Value.ToString(), v => Shared.Id<rescute.Domain.Aggregates.Samaritan>.Generate(Guid.Parse(v))));
            modelBuilder.Entity<Domain.Aggregates.Samaritan>(b => b.OwnsOne(samaritan => samaritan.Mobile));
            modelBuilder.Entity<Domain.Aggregates.Samaritan>(b => b.OwnsOne(samaritan => samaritan.FirstName));
            modelBuilder.Entity<Domain.Aggregates.Samaritan>(b => b.OwnsOne(samaritan => samaritan.LastName));
            modelBuilder.Entity<Domain.Aggregates.Samaritan>(b => b.ToTable("Samaritans"));

            // Animal
            modelBuilder.Entity<Animal>(b => b.HasKey(Animal => Animal.Id));
            modelBuilder.Entity<Animal>(b => b.Property(Animal => Animal.Id).HasConversion(v => v.Value.ToString(), v => Shared.Id<Animal>.Generate(Guid.Parse(v))));
            modelBuilder.Entity<Animal>(b => b.OwnsOne(animal => animal.Type));
            modelBuilder.Entity<Animal>(b => b.ToTable("Animals"));

            // ReportLogItem
            modelBuilder.Entity<ReportLogItem>(b => b.HasKey(ReportLogItem => ReportLogItem.Id));
            modelBuilder.Entity<ReportLogItem>(b => b.Property(ReportLogItem => ReportLogItem.Id).HasConversion(v => v.Value.ToString(), v => Shared.Id<ReportLogItem>.Generate(Guid.Parse(v))));
            modelBuilder.Entity<ReportLogItem>(b => b.HasOne(ReportLogItem => ReportLogItem.Samaritan));
            modelBuilder.Entity<ReportLogItem>(b => b.ToTable("ReportLogs"));

            // DocumentedLogItem
            modelBuilder.Entity<DocumentedLogItem>(b => b.OwnsMany(documentedLogItem => documentedLogItem.Documents, re => 
            {
                re.OwnsOne(document => document.Type);
                re.ToTable("Documents");
            }));
            

            // Report
            modelBuilder.Entity<Report>(b => b.HasKey(report => report.Id));
            modelBuilder.Entity<Report>(b => b.Property(report => report.Id).HasConversion(v => v.Value.ToString(), v => Shared.Id<Report>.Generate(Guid.Parse(v))));
            modelBuilder.Entity<Report>(b => b.HasMany(report => report.Logs));
            modelBuilder.Entity<Report>(b => b.HasMany(report => report.Animals));
            modelBuilder.Entity<Report>(b => b.HasOne(report => report.Samaritan));
            modelBuilder.Entity<Report>(b => b.ToTable("Reports"));


            // BillAttached
            modelBuilder.Entity<BillAttached>(b => b.Property(billAttached => billAttached.Total));
            modelBuilder.Entity<BillAttached>(b => b.Property(billAttached => billAttached.ContributionRequested));
           

            // Commented
            modelBuilder.Entity<Commented>(b => b.HasOne(commented => commented.RepliesTo));

            // Contributed
            modelBuilder.Entity<Contributed>(b => b.HasOne(contributed => contributed.Bill));


            // ImageAttached


            // ReportCreated
            modelBuilder.Entity<ReportCreated>(b => b.OwnsOne(reportCreated => reportCreated.EventLocation));

            // TransportRequested
            modelBuilder.Entity<TransportRequested>(b => b.OwnsOne(transportRequested => transportRequested.EventLocation));
            modelBuilder.Entity<TransportRequested>(b => b.OwnsOne(transportRequested => transportRequested.ToLocation));

            // VideoAttached
        }
    }
}
