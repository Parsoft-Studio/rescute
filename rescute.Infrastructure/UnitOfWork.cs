using System;
using System.Threading.Tasks;
using rescute.Domain.Repositories;
using rescute.Infrastructure.Repositories;

namespace rescute.Infrastructure;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly rescuteContext context;
    
    private readonly AnimalRepository animals;
    private readonly CommentRepository comments;
    private readonly SamaritanRepository samaritans;
    private readonly TimelineItemRepository timelineItems;

    public IAnimalRepository Animals => animals;
    public ISamaritanRepository Samaritans => samaritans;
    public ITimelineItemRepository TimelineItems => timelineItems;
    public ICommentRepository Comments => comments;

    public UnitOfWork(rescuteContext context)
    {
        if (context is null) throw new ArgumentNullException(nameof(context));

        this.context = context;

        samaritans = new SamaritanRepository(context);
        animals = new AnimalRepository(context);
        timelineItems = new TimelineItemRepository(context);
        comments = new CommentRepository(context);
    }

    private UnitOfWork()
    {
    }

    public async Task Complete()
    {
        await context.SaveChangesAsync();
    }
    
    public async ValueTask DisposeAsync()
    {
        await Complete();
        if (context != null) await context.DisposeAsync();
    }
}