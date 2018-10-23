using LZRNS.DomainModel.Context;
using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;

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
			_context.SaveChanges();
			return entity;
		}

		public void Delete(T item)
		{
			try
			{
				_context.Set<T>().Remove(item);
				_context.SaveChanges();
			}
			catch(Exception ex)
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
