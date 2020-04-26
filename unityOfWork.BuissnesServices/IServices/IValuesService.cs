using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using unityOfWork.DTO;

namespace unityOfWork.BuissnesServices.IServices
{
    public  interface IValuesService
    {
       Task< IEnumerable<Values>> getAllValues();
        Task<Values> getValue(int id);
    }
}
