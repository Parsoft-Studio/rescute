using Microsoft.EntityFrameworkCore;
using rescute.Domain.Aggregates;
using rescute.Domain.Aggregates.TimelineEvents;
using System;

namespace rescute.Infrastructure
{
    public class rescuteContext : DbContext
    {
        public rescuteContext(DbContextOptions<rescuteContext> options) : base(options)
        {
            ConnectionString = "Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=rescute;Integrated Security=SSPI;";
        }
        public rescuteContext()
        {
            ConnectionString = "Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=rescute;Integrated Security=SSPI;";
        }
        public rescuteContext(string connectionString)
        {
            ConnectionString = connectionString;
        }
        public DbSet<Animal> Animals { get; private set; }
        public DbSet<Samaritan> Samaritans { get; private set; }
        public DbSet<TimelineEvent> TimelineEvents { get; private set; }
        public DbSet<Comment> Comments { get; private set; }

        public string ConnectionString { get; set; }

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
            modelBuilder.Entity<Animal>(b => b.OwnsOne(animal => animal.Type));
            modelBuilder.Entity<Animal>(b => b.HasOne<Samaritan>().WithMany().HasForeignKey(animal => animal.IntroducedBy));
            modelBuilder.Entity<Animal>(b => b.Ignore(animal => animal.AcceptableAttachmentTypes));
            modelBuilder.Entity<Animal>(b => b.OwnsMany(animal => animal.Attachments, re =>
            {
                re.OwnsOne(attachment => attachment.Type);
                re.ToTable("AnimalAttachments");
            }));

            modelBuilder.Entity<Animal>(b => b.ToTable("Animals"));

            // TimelineEvent
            modelBuilder.Entity<TimelineEvent>(b => b.HasKey(tEvent => tEvent.Id));
            modelBuilder.Entity<TimelineEvent>(b => b.Property(tEvent => tEvent.Id).HasConversion(v => v.Value.ToString(), v => Shared.Id<TimelineEvent>.Generate(Guid.Parse(v))));
            modelBuilder.Entity<TimelineEvent>(b => b.HasOne<Samaritan>().WithMany().HasForeignKey(tEvent => tEvent.CreatedBy).OnDelete(DeleteBehavior.Cascade));
            modelBuilder.Entity<TimelineEvent>(b => b.HasOne<Animal>().WithMany().HasForeignKey(tEvent => tEvent.AnimalId).OnDelete(DeleteBehavior.Cascade));
            modelBuilder.Entity<TimelineEvent>(b => b.ToTable("TimelineEvents"));

            // TimelineEventWithAttachment
            modelBuilder.Entity<TimelineEventWithAttachments>(b => b.Ignore(tEventWithAttachments => tEventWithAttachments.AcceptableAttachmentTypes));
            modelBuilder.Entity<TimelineEventWithAttachments>(b => b.OwnsMany(tEventWithAttachment => tEventWithAttachment.Attachments, re =>
            {
                re.OwnsOne(attachment => attachment.Type);
                re.ToTable("TimelineEventAttachments");
            }));


            // BillAttached
            modelBuilder.Entity<BillAttached>(b => b.Property(billAttached => billAttached.Total));
            modelBuilder.Entity<BillAttached>(b => b.Property(billAttached => billAttached.ContributionRequested));
            modelBuilder.Entity<BillAttached>(b => b.OwnsMany(billAttached => billAttached.Contributions,re=>
            {
                re.HasOne<Samaritan>().WithMany().HasForeignKey(contribution => contribution.ContributorId);
                re.ToTable("BillContributions");
            }

            ));

            // StatusReported
            modelBuilder.Entity<StatusReported>(b => b.OwnsOne(statusReported => statusReported.EventLocation));

            // TestResultAttached
            // No configurable property


            // TransportRequested
            modelBuilder.Entity<TransportRequested>(b => b.OwnsOne(transportRequested => transportRequested.EventLocation));
            modelBuilder.Entity<TransportRequested>(b => b.OwnsOne(transportRequested => transportRequested.ToLocation));

            // Comment
            modelBuilder.Entity<Comment>(c => c.HasKey(comment => comment.Id));
            modelBuilder.Entity<Comment>(c => c.Property(comment => comment.Id).HasConversion(v => v.Value.ToString(), v => Shared.Id<Comment>.Generate(Guid.Parse(v))));
            modelBuilder.Entity<Comment>(b => b.HasOne<TimelineEvent>().WithMany().HasForeignKey(comment=>comment.RepliesTo));
            modelBuilder.Entity<Comment>(b => b.HasOne<Samaritan>().WithMany().HasForeignKey(comment=> comment.CreatedBy));
            modelBuilder.Entity<Comment>(b => b.ToTable("Comments"));

        }
    }
}
