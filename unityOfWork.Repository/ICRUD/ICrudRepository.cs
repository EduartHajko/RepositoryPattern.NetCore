using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace unityOfWork.Repository.ICRUD
{
    public interface ICrudRepository<in TKey, TE> : IReadOnlyRepository<TKey, TE>
      where TE : class
    {
        Task CreateAsync(TE entity);
        void Update(TE entity);
        void Delete(TE entity);
        Task CommitAsync();
    }
}
