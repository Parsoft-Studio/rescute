using rescute.Domain.Aggregates;
using rescute.Domain.Repositories;

namespace rescute.Infrastructure.Repositories;

internal class CommentRepository : Repository<Comment>, ICommentRepository
{
    public CommentRepository(rescuteContext context) : base(context)
    {
    }
}