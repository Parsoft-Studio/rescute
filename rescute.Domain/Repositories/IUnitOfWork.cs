using System;
using System.Threading.Tasks;

namespace rescute.Domain.Repositories;

public interface IUnitOfWork : IAsyncDisposable
{
    IAnimalRepository Animals { get; }
    ISamaritanRepository Samaritans { get; }
    ITimelineItemRepository TimelineItems { get; }
    ICommentRepository Comments { get; }
    Task Complete();
}