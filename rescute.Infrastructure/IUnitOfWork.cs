using rescute.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace rescute.Infrastructure
{
    interface IUnitOfWork
    {
        IAnimalRepository Animals { get; }
        IReportRepository Reports { get; }
        ISamaritanRepository Samaritans { get; }

        Task Complete();
    }
}
