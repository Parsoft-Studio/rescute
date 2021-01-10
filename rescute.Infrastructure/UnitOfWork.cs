using rescute.Domain.Repositories;
using rescute.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace rescute.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private rescuteContext context;

        private SamaritanRepository samaritans;
        private AnimalRepository animals;

        public UnitOfWork(rescuteContext c)
        {
            context = c;

            samaritans = new SamaritanRepository(context);
            animals = new AnimalRepository(context);

        }

        public IAnimalRepository Animals => animals;


        public ISamaritanRepository Samaritans => samaritans;

        public async Task Complete()
        {
            await context.SaveChangesAsync();
        }
    }
}
