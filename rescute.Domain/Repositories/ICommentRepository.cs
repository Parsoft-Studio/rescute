using rescute.Domain.Aggregates;
using rescute.Domain.Aggregates.TimelineItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace rescute.Domain.Repositories
{
    public interface ICommentRepository: IRepository<Comment>
    {
    }
}
