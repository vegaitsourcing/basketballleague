using LZRNS.DomainModel.Context;
using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Exceptions;
using LZRNS.DomainModels.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;

namespace LZRNS.DomainModels.Repository.Implementations
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : AbstractModel
    {
        public BasketballDbContext Context;

        public RepositoryBase(BasketballDbContext context)
        {
            Context = context;
        }

        public T Add(T item)
        {
            item.Id = Guid.NewGuid();
            var entity = Context.Set<T>().Add(item);
            try
            {
                Context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                string exceptionMessage = ex.InnerException?.InnerException?.Message;

                if (exceptionMessage?.Contains("UNIQUE KEY") != true)
                    return entity;

                if (exceptionMessage.Contains("League"))
                {
                    throw new DalException("Ime lige mora biti jedinstveno.", DalExceptionCode.UNIQUE_LEAGUE_NAME);
                }

                if (exceptionMessage.Contains("Season"))
                {
                    throw new DalException("Naziv sezone i godina zajedno moraju biti jedinstveni.", DalExceptionCode.UNIQUE_SEASON_DATA);
                }
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("debug");
            }
            return entity;
        }

        public IEnumerable<T> AddRange(IEnumerable<T> items)
        {
            var entities = Context.Set<T>().AddRange(items);
            Context.SaveChanges();

            return entities;
        }

        public void Delete(T item)
        {
            try
            {
                Context.Set<T>().Remove(item);
                Context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new UpdateException("Brisanje neuspešno, proverite veze između tabela.", ex);
            }
        }

        public IEnumerable<T> GetAll()
        {
            return Context.Set<T>();
        }

        public T GetById(Guid id)
        {
            return Context.Set<T>().Find(id);
        }

        public bool Update(T item)
        {
            try
            {
                Context.Entry(item).State = EntityState.Modified;
                Context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                Context.Dispose();
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}