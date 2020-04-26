using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using unityOfWork.Repository.ICRUD;

namespace unityOfWork.Repository.BASE
{
    public abstract class RawCrudRepository<TKey, TE> : RawReadOnlyRepository<TKey, TE>, ICrudRepository<TKey, TE>
  where TE : class
    {
        protected RawCrudRepository(DbContext context) : base(context)
        {

        }

        public  async Task CreateAsync(TE entity)
        {
            if (entity.GetType().GetProperty("CreatedOn") != null)
                entity.GetType().GetProperty("CreatedOn").SetValue(entity, DateTime.Now);

            if (entity.GetType().GetProperty("ModifiedOn") != null)
                entity.GetType().GetProperty("ModifiedOn").SetValue(entity, DateTime.Now);

            if (entity.GetType().GetProperty("CreatedBy") != null)
                entity.GetType().GetProperty("CreatedBy").SetValue(entity, Environment.UserName);

            if (entity.GetType().GetProperty("ModifiedBy") != null)
                entity.GetType().GetProperty("ModifiedBy").SetValue(entity, Environment.UserName);

            if (entity.GetType().GetProperty("DeletionStateCode") != null)
                entity.GetType().GetProperty("DeletionStateCode").SetValue(entity, 0);

           await Entities.AddAsync(entity);
        }

        public void Update(TE entity)
        {
            if (entity.GetType().GetProperty("ModifiedOn") != null)
                entity.GetType().GetProperty("ModifiedOn").SetValue(entity, DateTime.Now);

            if (entity.GetType().GetProperty("ModifiedBy") != null)
                entity.GetType().GetProperty("ModifiedBy").SetValue(entity, Environment.UserName);

            //DbEntityEntry<TE> entry = Context.Entry(entity.SetDateTimeGMT0(SharedData.timeZoneId));  
            Entities.Update(entity);
        }

        public void Delete(TE entity)
        {
            if (entity.GetType().GetProperty("DeletionStateCode") != null)
                entity.GetType().GetProperty("DeletionStateCode").SetValue(entity, 1);

            if (entity.GetType().GetProperty("ModifiedOn") != null)
                entity.GetType().GetProperty("ModifiedOn").SetValue(entity, DateTime.Now);

            if (entity.GetType().GetProperty("ModifiedBy") != null)
                entity.GetType().GetProperty("ModifiedBy").SetValue(entity, Environment.UserName);

            Entities.Update(entity);
        }

        public async Task CommitAsync()
        {
          await  Context.SaveChangesAsync();
        }

        public void DetachAllEntities(DbContext Context)
        {
            foreach (var entity in Context.ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted))
                Context.Entry(entity.Entity).State = EntityState.Detached;
        }


    }

}
