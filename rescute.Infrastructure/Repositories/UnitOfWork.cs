using System.Threading.Tasks;
using rescute.Domain.Repositories;

namespace rescute.Infrastructure.Repositories;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly rescuteContext context;

    public IAnimalRepository Animals { get; }

    public ISamaritanRepository Samaritans { get; }

    public ITimelineItemRepository TimelineItems { get; }

    public ICommentRepository Comments { get; }

    public UnitOfWork(rescuteContext context)
    {
        this.context = context;
        Samaritans = new SamaritanRepository(context);
        Animals = new AnimalRepository(context);
        TimelineItems = new TimelineItemRepository(context);
        Comments = new CommentRepository(context);
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