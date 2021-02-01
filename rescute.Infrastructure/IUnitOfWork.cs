using rescute.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace rescute.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        IAnimalRepository Animals { get; }
        ISamaritanRepository Samaritans { get; }
        ITimelineEventRepository TimelineEvents { get; }
        ICommentRepository Comments { get; }
        Task Complete();
    }
}
