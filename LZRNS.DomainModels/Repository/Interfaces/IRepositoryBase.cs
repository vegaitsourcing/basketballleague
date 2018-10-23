﻿using System;
using System.Collections.Generic;

namespace LZRNS.DomainModels.Repository.Interfaces
{
    public interface IRepositoryBase<T> : IDisposable
    {
        IEnumerable<T> GetAll();
        T GetById(Guid id);
        T Add(T item);
        bool Update(T item);
        void Delete(T id);
    }
}
