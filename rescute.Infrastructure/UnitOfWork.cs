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
        private ReportRepository reports;

        public UnitOfWork(rescuteContext c)
        {
            context = c;

            samaritans = new SamaritanRepository(context);
            animals = new AnimalRepository(context);
            reports = new ReportRepository(context);

        }

        public IAnimalRepository Animals => animals;

        public IReportRepository Reports => reports;

        public ISamaritanRepository Samaritans => samaritans;

        public async Task Complete()
        {
            await context.SaveChangesAsync();
        }
    }
}
