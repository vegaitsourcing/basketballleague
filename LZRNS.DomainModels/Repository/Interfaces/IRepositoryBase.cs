using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LZRNS.DomainModels.Repository.Interfaces
{
    public interface IRepositoryBase<T> : IDisposable
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        T Add(T item);
        bool Update(T item);
        bool Delete(T id);
    }
}
