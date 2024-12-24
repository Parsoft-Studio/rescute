using rescute.Domain.Aggregates;
using rescute.Domain.Repositories;

namespace rescute.Infrastructure.Repositories;

public class CommentRepository : Repository<Comment>, ICommentRepository
{
    public CommentRepository(rescuteContext c) : base(c)
    {
    }
}