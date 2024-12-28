using System;
using System.Threading.Tasks;
using rescute.Domain.Repositories;

namespace rescute.Infrastructure;

public interface IUnitOfWork : IAsyncDisposable
{
    IAnimalRepository Animals { get; }
    ISamaritanRepository Samaritans { get; }
    ITimelineItemRepository TimelineItems { get; }

    ICommentRepository Comments { get; }
    Task Complete();
}