using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using Repository.UnitOfWork;
using System.ComponentModel.DataAnnotations;

namespace Repository
{
    public class GenericRepository<T> : IGenericRepository<T>, IDisposable where T : class
    {
        private DbSet<T> _entities;
        private string _errorMessage = string.Empty;
        private bool _isDisposed;
        public GenericRepository(IUnitOfWork<PlayGround_DbContext> unitOfWork)
        : this(unitOfWork.Context)
        {
        }
        public GenericRepository(PlayGround_DbContext context)
        {
            _isDisposed = false;
            Context = context;
        }
        public PlayGround_DbContext Context { get; set; }
        public virtual IQueryable<T> Table
        {
            get { return Entities; }
        }
        protected virtual DbSet<T> Entities
        {
            get { return _entities ?? (_entities = Context.Set<T>()); }
        }
        public void Dispose()
        {
            if (Context != null)
                Context.Dispose();
            _isDisposed = true;
        }
        public virtual IEnumerable<T> GetAll()
        {
            return Entities.ToList();
        }
        public virtual T GetById(object id)
        {
            return Entities.Find(id);
        }
        public virtual void Insert(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");
                Entities.Add(entity);
                if (Context == null || _isDisposed)
                    Context = new PlayGround_DbContext();
                //Context.SaveChanges(); commented out call to SaveChanges as Context save changes will be 
                //called with Unit of work
            }
            catch (ValidationException dbEx)
            {
              throw new Exception(dbEx.Message, dbEx);
            }
        }
        public void BulkInsert(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                {
                    throw new ArgumentNullException("entities");
                }
                Context.ChangeTracker.AutoDetectChangesEnabled = false;
                Context.Set<T>().AddRange(entities);
                Context.SaveChanges();
            }
            catch (ValidationException dbEx)
            {
                throw new Exception(dbEx.Message, dbEx);
            }
        }
        public virtual void Update(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");
                if (Context == null || _isDisposed)
                    Context = new PlayGround_DbContext();
                SetEntryModified(entity);
                //Context.SaveChanges(); commented out call to SaveChanges as Context save changes will be called with Unit of work
            }
            catch (ValidationException dbEx)
            {
                throw new Exception(dbEx.Message, dbEx);
            }
        }
        public virtual void Delete(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");
                if (Context == null || _isDisposed)
                    Context = new PlayGround_DbContext();
                Entities.Remove(entity);
                //Context.SaveChanges(); commented out call to SaveChanges as Context save changes will be called with Unit of work
            }
            catch (ValidationException dbEx)
            {
                throw new Exception(dbEx.Message, dbEx);
            }
        }
        public virtual void SetEntryModified(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(object id)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
