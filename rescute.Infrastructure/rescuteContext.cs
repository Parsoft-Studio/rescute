using Microsoft.EntityFrameworkCore;
using rescute.Domain.Aggregates;
using rescute.Domain.Aggregates.TimelineItems;
using rescute.Shared;
using System;

namespace rescute.Infrastructure
{
    public class rescuteContext : DbContext
    {
        private const string defaultConnectionString = "Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=rescute;Integrated Security=SSPI;";
        public static string Schema => "rescute";

        public rescuteContext(DbContextOptions<rescuteContext> options) : base(options)
        {
            ConnectionString = defaultConnectionString;
        }
        public rescuteContext()
        {
            ConnectionString = defaultConnectionString;
        }
        public rescuteContext(string connectionString)
        {
            ConnectionString = connectionString;
        }
        public DbSet<Animal> Animals { get; private set; }
        public DbSet<Samaritan> Samaritans { get; private set; }

        public DbSet<TimelineItem> TimelineItems { get; private set; }
        public DbSet<Comment> Comments { get; private set; }

        public string ConnectionString { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schema);
            modelBuilder.Ignore<Id<TimelineItem>>();

            // Samaritan
            modelBuilder.Entity<Samaritan>(b => b.HasKey(samaritan => samaritan.Id).IsClustered(false));
            modelBuilder.Entity<Samaritan>(b => b.Property(samaritan => samaritan.Id).HasConversion(v => v.Value.ToString(), v => Id<Samaritan>.Generate(Guid.Parse(v))));
            modelBuilder.Entity<Samaritan>(b => b.OwnsOne(samaritan => samaritan.Mobile));
            modelBuilder.Entity<Samaritan>(b => b.OwnsOne(samaritan => samaritan.FirstName));
            modelBuilder.Entity<Samaritan>(b => b.OwnsOne(samaritan => samaritan.LastName));
            modelBuilder.Entity<Samaritan>(b => b.ToTable("Samaritans"));

            // Animal
            modelBuilder.Entity<Animal>(b => b.HasKey(animal => animal.Id).IsClustered(false));
            modelBuilder.Entity<Animal>(b => b.Property(animal => animal.Id).HasConversion(v => v.Value.ToString(), v => Id<Animal>.Generate(Guid.Parse(v))));
            modelBuilder.Entity<Animal>(b => b.OwnsOne(animal => animal.Type));
            modelBuilder.Entity<Animal>(b => b.HasOne<Samaritan>().WithMany().HasForeignKey(animal => animal.IntroducedBy));
            modelBuilder.Entity<Animal>(b => b.Ignore(animal => animal.AcceptableAttachmentTypes));
            modelBuilder.Entity<Animal>(b => b.OwnsMany(animal => animal.Attachments, re =>
            {
                re.Ignore(attachment => attachment.Type);
                re.ToTable("AnimalAttachments");
            }));

            modelBuilder.Entity<Animal>(b => b.ToTable("Animals"));

            // TimelineItem
            modelBuilder.Entity<TimelineItem>(b => b.HasKey(tItem => tItem.Id).IsClustered(false));
            modelBuilder.Entity<TimelineItem>(b => b.Property(tItem => tItem.Id).HasConversion(v => v.Value.ToString(), v => Id<TimelineItem>.Generate(Guid.Parse(v))));
            modelBuilder.Entity<TimelineItem>(b => b.HasOne<Samaritan>().WithMany().HasForeignKey(tItem => tItem.CreatedBy).OnDelete(DeleteBehavior.Cascade));
            modelBuilder.Entity<TimelineItem>(b => b.HasOne<Animal>().WithMany().HasForeignKey(tItem => tItem.AnimalId).OnDelete(DeleteBehavior.Cascade));
            modelBuilder.Entity<TimelineItem>(b => b.ToTable("TimelineItems"));

            // TimelineItemWithAttachment
            modelBuilder.Entity<TimelineItemWithAttachments>(b => b.Ignore(tEventWithAttachments => tEventWithAttachments.AcceptableAttachmentTypes));
            modelBuilder.Entity<TimelineItemWithAttachments>(b => b.OwnsMany(tEventWithAttachment => tEventWithAttachment.Attachments, re =>
            {
                re.Ignore(attachment => attachment.Type);
                re.ToTable("TimelineItemAttachments");
            }));

            // Bill
            modelBuilder.Entity<Bill>(b => b.HasOne(bill => bill.ContributionRequest).WithOne().HasForeignKey<ContributionRequest>(contribRequest => contribRequest.BillId).OnDelete(DeleteBehavior.Cascade));

            // ContributionRequest
            modelBuilder.Entity<ContributionRequest>(b => b.HasKey(tItem => tItem.Id).IsClustered(false));
            modelBuilder.Entity<ContributionRequest>(b => b.Property(tItem => tItem.Id).HasConversion(v => v.Value.ToString(), v => Id<ContributionRequest>.Generate(Guid.Parse(v))));
            modelBuilder.Entity<ContributionRequest>(b => b.OwnsMany(contReq => contReq.Contributions, re =>
             {
                 re.HasOne<Samaritan>().WithMany().HasForeignKey(contribution => contribution.ContributorId);
                 re.ToTable("Contributions");
             }
            ));
            modelBuilder.Entity<ContributionRequest>(b => b.Ignore(contReq => contReq.ContributionsTotal));
            modelBuilder.Entity<ContributionRequest>(b => b.ToTable("ContributionRequests"));

            // StatusReport
            modelBuilder.Entity<StatusReport>(b => b.OwnsOne(statusReport => statusReport.EventLocation));

            // TransportRequest
            modelBuilder.Entity<TransportRequest>(b => b.OwnsOne(transportRequested => transportRequested.EventLocation));
            modelBuilder.Entity<TransportRequest>(b => b.OwnsOne(transportRequested => transportRequested.ToLocation));

            // Comment
            modelBuilder.Entity<Comment>(c => c.HasKey(comment => comment.Id));
            modelBuilder.Entity<Comment>(c => c.Property(comment => comment.Id).HasConversion(v => v.Value.ToString(), v => Shared.Id<Comment>.Generate(Guid.Parse(v))));
            modelBuilder.Entity<Comment>(b => b.HasOne<TimelineItem>().WithMany().HasForeignKey(comment => comment.RepliesTo));
            modelBuilder.Entity<Comment>(b => b.HasOne<Samaritan>().WithMany().HasForeignKey(comment => comment.CreatedBy));
            modelBuilder.Entity<Comment>(b => b.ToTable("Comments"));

        }
    }
}
