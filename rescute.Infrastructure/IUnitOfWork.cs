using rescute.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace rescute.Infrastructure
{
    interface IUnitOfWork : IDisposable
    {
        IAnimalRepository Animals { get; }
        ISamaritanRepository Samaritans { get; }

        Task Complete();
    }
}
