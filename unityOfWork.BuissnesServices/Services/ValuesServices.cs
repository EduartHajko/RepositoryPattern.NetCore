using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using unityOfWork.BuissnesServices.IServices;
using unityOfWork.DTO;
using unityOfWork.Repository.ICRUD;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace unityOfWork.BuissnesServices.Services
{
    public class ValuesService : IValuesService
    {

        public  ICrudRepository<int, Values> ValuesRepository;
        public ValuesService(ICrudRepository<int, Values> ValuesRepository)
        {
            this.ValuesRepository = ValuesRepository;
        }
        public async Task <IEnumerable<Values>> getAllValues()
        {

            Values Values = new Values()
            {   
                Name = "Beqo"
                
            };
           await ValuesRepository.CreateAsync(Values);
            await ValuesRepository.CommitAsync();
            var result =  await ValuesRepository.Entity().Where(a => a.DeletionStateCode == 0).ToListAsync();
            
            return  result;
        }

        public async Task<Values> getValue(int id)
        {
            var result = await ValuesRepository.Find(id);

            return result;
        }
    }
}
