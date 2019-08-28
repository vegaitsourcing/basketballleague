using LZRNS.DomainModel.Context;
using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Repository.Interfaces;
using LZRNS.DomainModels.Repository.Interfaces.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace LZRNS.DomainModels.Repository.Implementations
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : AbstractModel
    {
        public BasketballDbContext _context;
        public RepositoryBase(BasketballDbContext context)
        {
            _context = context;
        }

        public T Add(T item)
        {
            item.Id = Guid.NewGuid();
            var entity = _context.Set<T>().Add(item);
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {

                string exceptionMessage = ex.InnerException.InnerException.Message;
                if (exceptionMessage.Contains("UNIQUE KEY"))
                {

                    if (exceptionMessage.Contains("League"))
                    {
                        throw new DalException("Ime lige mora biti jedinstveno.", DalExceptionCode.UNIQUE_LEAGUE_NAME);
                    }
                    else if (exceptionMessage.Contains("Season"))
                    {
                        throw new DalException("Naziv sezone i godina zajedno moraju biti jedinstveni.", DalExceptionCode.UNIQUE_SEASON_DATA);

                    }

                }


            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("debug");
            }
            return entity;
        }

        public IEnumerable<T> AddRange(IEnumerable<T> items)
        {
            var entities = _context.Set<T>().AddRange(items);
            _context.SaveChanges();

            return entities;
        }

        public void Delete(T item)
        {
            try
            {
                _context.Set<T>().Remove(item);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new UpdateException("Brisanje neuspešno, proverite veze između tabela.", ex);
            }
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>();
        }

        public T GetById(Guid id)
        {
            return _context.Set<T>().Find(id);
        }

        public bool Update(T item)
        {
            try
            {
                _context.Entry(item).State = EntityState.Modified;
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
