﻿using rescute.Domain.Repositories;
using rescute.Infrastructure.Repositories;
using System;
using System.Threading.Tasks;

namespace rescute.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly rescuteContext context;

        private readonly SamaritanRepository samaritans;
        private readonly AnimalRepository animals;
        private readonly TimelineItemRepository timelineItems;
        private readonly CommentRepository comments;

        public UnitOfWork(rescuteContext c)
        {
            if (c is null) throw new ArgumentNullException("Context is required.");

            context = c;

            this.samaritans = new SamaritanRepository(c);
            this.animals = new AnimalRepository(c);
            this.timelineItems = new TimelineItemRepository(c);
            this.comments = new CommentRepository(c);
        }
        private UnitOfWork() { }
        public IAnimalRepository Animals => animals;


        public ISamaritanRepository Samaritans => samaritans;

        public ITimelineItemRepository TimelineItems => timelineItems;

        public ICommentRepository Comments => comments;

        public async Task Complete()
        {
            await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
