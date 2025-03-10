﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using InstrumentHub.DataAccess.Abstract;
using Microsoft.EntityFrameworkCore;

namespace InstrumentHub.DataAccess.Concrate.EfCore
{
	public class EfCoreGenericRepository<T, TContext> : IRepository<T> where T : class where TContext : DbContext, new()
	{
		public void Create(T entity)
		{
			using (var context = new TContext())
			{
				context.Set<T>().Add(entity);
				context.SaveChanges();
			}
		}

		public virtual void Delete(T entity)
		{
			using (var context = new TContext())
			{
				context.Set<T>().Remove(entity);
				context.SaveChanges();
			}
		}

		public virtual List<T> GetAll(Expression<Func<T, bool>> filter = null)
		{
			using (var context = new TContext())
			{
				return filter == null? context.Set<T>().ToList() : context.Set<T>().Where(filter).ToList();
			}
		}

		public T GetbyId(int id)
		{
			using (var context = new TContext())
			{
				return context.Set<T>().Find(id);
			}
		}

		public virtual T GetOne(Expression<Func<T, bool>> filter = null)
		{
			using (var context = new TContext())
			{
				return context.Set<T>().Where(filter).FirstOrDefault();
			}
		}

		public virtual void Update(T entity)
		{
			using (var context = new TContext())
			{
				context.Entry(entity).State = EntityState.Modified;
				context.SaveChanges();
			}
		}
	}
}
