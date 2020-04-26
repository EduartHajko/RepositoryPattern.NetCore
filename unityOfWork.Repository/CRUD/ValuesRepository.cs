using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using unityOfWork.DTO;
using unityOfWork.Repository.BASE;

namespace unityOfWork.Repository.CRUD
{
    public class ValuesRepsitory : RawCrudRepository<int, Values>
    {
        public ValuesRepsitory(DbContext context) : base(context)
        {

        }
    }
}
