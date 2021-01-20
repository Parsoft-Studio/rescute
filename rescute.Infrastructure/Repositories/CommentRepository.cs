using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using rescute.Domain.Aggregates;
using rescute.Domain.Aggregates.TimelineEvents;
using rescute.Domain.Repositories;

namespace rescute.Infrastructure.Repositories
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public CommentRepository(rescuteContext c) : base(c)
        {
        }
    }
}
