using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace unityOfWork.Repository.ICRUD
{
    public interface IReadOnlyRepository<in TKey, TE>
      where TE : class
    {

        Task<TE> Find(TKey id);
        IQueryable<TE> Entity();
        long Count(Expression<Func<TE, bool>> predicate);
        void Dispose();
    }
}
