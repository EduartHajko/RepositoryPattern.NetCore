using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using unityOfWork.Repository.ICRUD;

namespace unityOfWork.Repository.BASE
{
    public abstract class RawReadOnlyRepository<TKey, TE> : IReadOnlyRepository<TKey, TE>, IAsyncDisposable, IDisposable
 where TE : class
    {
        protected DbContext Context { get; }
        protected DbSet<TE> Entities { get; }

        protected RawReadOnlyRepository(DbContext context)
        {

            Context = context;
            Entities = context.Set<TE>();

            // Per queueitem, non nascondere di default i record eliminati
            // Quando visualizzo la cronologia degli spostamenti di coda, devono essere visualizzati ancche i record eliminati
            // la coda corrente di un caso, corrisponde al record non eliminato.


            //Context.Database.Log = (dbLog => log.Debug(dbLog));
        }
        public IQueryable<TE> Entity() => Entities.AsQueryable();
        //public async Task<IEnumerable<TE>> Entity() =>await Entities.ToListAsync();
        //public virtual async Task<List<TE>> Entity()
        //{
        //    return await Entities.ToListAsync();

        //}

        public virtual async Task<TE> Find(TKey id)
        {
            TE output = await Entities.FindAsync(id);
            return output;
        }


        public virtual long Count(Expression<Func<TE, bool>> predicate)
        {
            long output = Entities.Count(predicate);
            return output;
        }

        public void Dispose()
        {
            if (Context != null) Context.Dispose();
        }

        public ValueTask DisposeAsync()
        {
            return ((IAsyncDisposable)Context).DisposeAsync();
        }
    }

}
